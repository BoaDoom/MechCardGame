using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic; //Needed for Lists
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using System.Xml.Linq; //Needed for XDocument
using System.Linq;

public class BPartXMLReaderScript : MonoBehaviour {
	bool finishedLoading = false;

//	public BPartGenericScript bPartGenericPrefab;


	XDocument xmlDoc; //create Xdocument. Will be used later to read XML file 
	IEnumerable<XElement> typeOfParts; //Create an Ienumerable list. Will be used to store XML Items. 
	IEnumerable<XElement> listOfParts;
	IEnumerable<XElement> storedAnchorPoints;
	IEnumerable<XElement> storedComplexAnchorPoints;
	public List <BodyPartDataHolder> BPartData = new List <BodyPartDataHolder>(); //Initialize List of XMLWeaponData objects.
	public List <ComplexAnchorPoints> listOfComplexAnchorPoints;
//	private BPartGenericScript LeftViewerArm;
//	private BPartGenericScript RightViewerArm;
//	private BPartGenericScript Arms;
//	private BPartGenericScript Arms;
//	private BPartGenericScript Arms;

	private string BpartName = "none";
	private int BpartIDnum;
	private string BpartType = "none";
	IEnumerable<XElement> gridHitBox;
	IEnumerable<XElement> AnchorPoints;
	private int MaxHealth = 0;
//	private int moduleSlotCount = 0;
	private ModuleSocketCount moduleSocketCount;
	private int[][] gridOfBodyPart;
	private Vector2 anchorVector2;
	private bool complexAnchorPointsBoolCheck;

	void Start ()
	{
//		print ("xml bodypart has started");
		//DontDestroyOnLoad (gameObject); //Allows Loader to carry over into new scene 
		LoadXML (); //Loads XML File. Code below. 
		StartCoroutine(AssignData()); //Starts assigning XML data to data List. Code below
		//		Debug.Log("inside bodydata count "+BPartData.Count);
	}

	void LoadXML()

	{
		//Assigning Xdocument xmlDoc. Loads the xml file from the file path listed. 
		xmlDoc = XDocument.Load("assets/XMLdata/Bparts.xml");

		//This basically breaks down the XML Document into XML Elements. Used later. 
		typeOfParts = xmlDoc.Descendants("BPartsList").Elements ();
	}

	//this is our coroutine that will actually read and assign the XML data to our List 
	IEnumerator AssignData()
	{
//		print ("reached assigned data");
//		int t = 0;
		/*foreach allows us to look at every Element of our XML file and do something with each one. Basically, this line is saying “for each element in the xml document, do something.*/ 
		foreach (var partType in typeOfParts)
		{
			listOfParts = partType.Elements ();

			foreach (var part in listOfParts) {
				//moduleSlotCount = int.Parse (part.Element ("defaultModuleSlots").Value.Trim ());
//				print(part.Attribute ("name").Value.Trim ());
				if (BpartName != part.Attribute ("name").Value.Trim ()) {		//if the next element has a new name, select that parrent and assign all it's children to these values
					BpartName = part.Attribute ("name").Value.Trim ();	
					BpartIDnum = int.Parse (part.Element ("BPartIDnum").Value.Trim ());
					BpartType = part.Parent.Name.ToString();
					moduleSocketCount = new ModuleSocketCount (
						int.Parse (part.Element ("WeaponModuleSlots").Value.Trim ()), 
						int.Parse (part.Element ("UtilityModuleSlots").Value.Trim ()),
						int.Parse (part.Element ("GenericModuleSlots").Value.Trim ()));
//					moduleSlotCount = int.Parse (part.Element ("defaultModuleSlots").Value.Trim ());
					MaxHealth = int.Parse (part.Element ("Health").Value.Trim ());

					int numberXCord = part.Element ("gridHitBox").Element ("line").Value.Trim ().Length;	//the length of the design shape line of 1's and 0's
					gridHitBox = part.Element ("gridHitBox").Descendants ();
					int numberYCord = part.Element ("gridHitBox").Descendants ().Count ();		//counts how many lines there are in the targeting grid, giving Y cords size
					int interationY = numberYCord - 1;
					string complexBool = part.Element ("ComplexAnchorPoints").Value.Trim ();

					if (complexBool == "true") {
						complexAnchorPointsBoolCheck = true;
					} else {
						complexAnchorPointsBoolCheck = false;
					}

					gridOfBodyPart = new int[(int)numberXCord][];
					for (int i = 0; i < numberXCord; i++) {
						gridOfBodyPart [i] = new int[(int)numberYCord];		//instantiating the grid beforehand
					}
					foreach (XElement line in gridHitBox) {
						int interationX = 0;
						string lineOfNumbers = line.Value;
						foreach (char num in lineOfNumbers) {
							int newNum = (int)char.GetNumericValue (num);
							gridOfBodyPart [interationX] [interationY] = newNum;
							interationX++;
						}
						interationY--;
					}
					
					if (complexAnchorPointsBoolCheck) {
						listOfComplexAnchorPoints = new List <ComplexAnchorPoints>();
						ComplexAnchorPoints uniqueAnchorPoints;
						bool sexOfSocket = false;
						storedComplexAnchorPoints = part.Element ("AnchorPoints").Elements ();
						foreach (XElement point in storedComplexAnchorPoints) {
							//Debug.Log ("complex point: "+point.Name);
							storedAnchorPoints = point.Elements();
							foreach (XElement cord in storedAnchorPoints) {
								//Debug.Log ("cord: "+cord.Name);
								if (cord.Name == "xCord") {
									anchorVector2.x = int.Parse (cord.Value);
								}
								if (cord.Name == "yCord") {
									anchorVector2.y = int.Parse (cord.Value);
								}
							}
							if (point.Attribute ("typeOfSocket").Value.Trim () == "male") {
								sexOfSocket = true;
							} else {
								sexOfSocket = false;
							}
							uniqueAnchorPoints = new ComplexAnchorPoints (point.Name.ToString(), anchorVector2 , sexOfSocket);

							listOfComplexAnchorPoints.Add (uniqueAnchorPoints);
						}
						BPartData.Add (new BodyPartDataHolder().startBodyPartDataHolder (BpartName, BpartIDnum, BpartType, MaxHealth, gridOfBodyPart, listOfComplexAnchorPoints, moduleSocketCount));
						//anchorVector2 = new Vector2 (0.0f, 0.0f);		//placeholder
					} else {
						//Debug.Log ((string)partType.Name.ToString());
						storedAnchorPoints = part.Element ("AnchorPoint").Descendants ();
						foreach (XElement cord in storedAnchorPoints) {
							if (cord.Name == "xCord") {
								anchorVector2.x = int.Parse (cord.Value);
							}
							if (cord.Name == "yCord") {
								anchorVector2.y = int.Parse (cord.Value);
							}
						}
						BPartData.Add (new BodyPartDataHolder().startBodyPartDataHolder (BpartName, BpartIDnum, BpartType, MaxHealth, gridOfBodyPart, anchorVector2, moduleSocketCount));
					}
					//					Debug.Log (BpartName +" "+ BpartType +" "+ MaxHealth +" "+ gridOfBodyPart +" "+ anchorVector2 +" "+ moduleSocketCount);
				}
			}
		}
		finishedLoading = true;
//		print ("BPartXML finished");
		yield return null;
	}
	public bool checkIfFinishedLoading(){
		return finishedLoading;
	}

	public List<BodyPartDataHolder> getAllBodyDataForType(string requestedTypeOfParts){
		return BPartData.FindAll (BodyPartDataHolder => BodyPartDataHolder.typeOfpart == requestedTypeOfParts);
	}

	public BodyPartDataHolder getBodyData(string requestedNameOfPart){			//future efficiency, have each part be catagorized acording to their part type for better searching
		//used by BodyPartMakerScript when asked by enemyscript to makebodypart()
		//Debug.Log("name: "+ requestedNameOfPart); 
		//Debug.Log("requested found "+BPartData.Find (BodyPartDataHolder => BodyPartDataHolder.name == requestedNameOfPart).name);
		return BPartData.Find (BodyPartDataHolder => BodyPartDataHolder.name == requestedNameOfPart);
	}
	public BodyPartDataHolder getBodyDataByID(int requestedIDOfPart){			//future efficiency, have each part be catagorized acording to their part type for better searching
		//used by BodyPartMakerScript when asked by enemyscript to makebodypart()
		//Debug.Log("name: "+ requestedNameOfPart); 
		//Debug.Log("requested found "+BPartData.Find (BodyPartDataHolder => BodyPartDataHolder.name == requestedNameOfPart).name);
		return BPartData.Find (BodyPartDataHolder => BodyPartDataHolder.BpartIDnum == requestedIDOfPart);
	}

}

public class BodyPartDataHolder{
	public string name;
	public int BpartIDnum;
	public string typeOfpart;
	public int maxHealth;
	public int[][] bodyPartGrid;
	public Vector2 anchor;
	public List<ComplexAnchorPoints> listOfComplexAnchorPoints;
	public bool simpleAnchorPoints;

	public ModuleSocketCount moduleSocketCount;
	public int[] moduleIDnum = new int[3];

	public BodyPartDataHolder startBodyPartDataHolder(string BpartName, int incIDnum, string incBpartName, int incMaxHealth, int[][] incomingBodyPartGrid, Vector2 AnchorPoint, ModuleSocketCount incModuleSocketCount){
		BpartIDnum = incIDnum;
		simpleAnchorPoints = true;
		name = BpartName;
		typeOfpart = incBpartName;
		maxHealth = incMaxHealth;
		anchor = AnchorPoint;
		bodyPartGrid = new int[incomingBodyPartGrid.Length][];

		moduleSocketCount = incModuleSocketCount;
//		moduleIDnum = new int[3];

		for(int i=0; i < incomingBodyPartGrid.Length; i++){	//transfering the int[][] grid
			bodyPartGrid [i] = new int[incomingBodyPartGrid[0].Length];
			for(int j=0; j < incomingBodyPartGrid[0].Length; j++){
				bodyPartGrid [i][j] = incomingBodyPartGrid[i][j];
			}
		}
		return this;

	}
	public BodyPartDataHolder startBodyPartDataHolder(string BpartName, int incIDnum, string incBpartName, int incMaxHealth, int[][] incomingBodyPartGrid, List<ComplexAnchorPoints> incomingListOfComplexAnchorPoints, ModuleSocketCount incModuleSocketCount){
		BpartIDnum = incIDnum;
		simpleAnchorPoints = false;
		name = BpartName;
		typeOfpart = incBpartName;
		maxHealth = incMaxHealth;
		listOfComplexAnchorPoints = incomingListOfComplexAnchorPoints;
		bodyPartGrid = new int[incomingBodyPartGrid.Length][];

		moduleSocketCount = incModuleSocketCount;
//		moduleIDnum = new int[3];
		for(int i=0; i < incomingBodyPartGrid.Length; i++){	//transfering the int[][] grid
			bodyPartGrid [i] = new int[incomingBodyPartGrid[0].Length];
			for(int j=0; j < incomingBodyPartGrid[0].Length; j++){
				bodyPartGrid [i][j] = incomingBodyPartGrid[i][j];
			}
		}
		return this;
	}
	public void makeACopy(BodyPartDataHolder incomingData){
		BpartIDnum = incomingData.BpartIDnum;
		simpleAnchorPoints = incomingData.simpleAnchorPoints;
		name = incomingData.name;
		typeOfpart = incomingData.typeOfpart;
		maxHealth = incomingData.maxHealth;
		if (simpleAnchorPoints)
		{
			anchor = incomingData.anchor;
		}
		else if (!simpleAnchorPoints){
			listOfComplexAnchorPoints = incomingData.listOfComplexAnchorPoints;
		}
		bodyPartGrid = new int[incomingData.bodyPartGrid.Length][];

		moduleSocketCount = incomingData.moduleSocketCount;
		//		moduleIDnum = new int[3];
		for(int i=0; i < incomingData.bodyPartGrid.Length; i++){	//transfering the int[][] grid
			bodyPartGrid [i] = new int[incomingData.bodyPartGrid[0].Length];
			for(int j=0; j < incomingData.bodyPartGrid[0].Length; j++){
				bodyPartGrid [i][j] = incomingData.bodyPartGrid[i][j];
			}
		}
	}
	public void setModuleIDnum(int[] incomingModuleIDNumbers){
		moduleIDnum = incomingModuleIDNumbers;
	}
}
public class ComplexAnchorPoints{
	public string nameOfPoint;
	public Vector2 anchorPoint;
	//public Vector2 globalAnchorPointLocation;
	public bool male;
	public ComplexAnchorPoints(string incomingName, Vector2 incomingAnchorPoint, bool incomingType){
		nameOfPoint = incomingName;
		anchorPoint = incomingAnchorPoint;
		male = incomingType;
	}
//	public void setGlobalAnchorPointLocation(Vector2 incomingGlobalLocation){
//		globalAnchorPointLocation = incomingGlobalLocation;
//	}
}
public class ModuleSocketCount{
	public int weaponModuleSocketCount;
	public int utilityModuleSocketCount;
	public int genericModuleSocketCount;
	public int totalModuleSocketCount;
	public ModuleSocketCount(int incomingWeapon, int incomingUtility, int incomingGeneric){
		weaponModuleSocketCount = incomingWeapon;
		utilityModuleSocketCount = incomingUtility;
		genericModuleSocketCount = incomingGeneric;
		totalModuleSocketCount = incomingWeapon + incomingUtility + incomingGeneric;
	}
	public int getTotalCount(){
		return totalModuleSocketCount;
	}
	public int getWeaponCount(){
		return weaponModuleSocketCount;
	}
	public int getUtilityCount(){
		return utilityModuleSocketCount;
	}
	public int getBothCount(){
		return genericModuleSocketCount;
	}

}

