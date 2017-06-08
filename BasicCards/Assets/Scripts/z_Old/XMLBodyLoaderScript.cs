using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic; //Needed for Lists
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using System.Xml.Linq; //Needed for XDocument
using System.Linq;

public class XMLBodyLoaderScript : MonoBehaviour {

	XDocument xmlDoc; //create Xdocument. Will be used later to read XML file 
	IEnumerable<XElement> items; //Create an Ienumerable list. Will be used to store XML Items. 
	IEnumerable<XElement> linesOfHitBox;
	public List <XMLBodyHitData> bodyData = new List <XMLBodyHitData>(); //Initialize List of XMLWeaponData objects.

	//int iteration = 0;
	//bool newName = true;
	//int tempvar = 0;
	//bool finishedLoading = false;

	string nameOfBody = "none"; 
	string placement = "none";
	XElement designShape;
	int[][] gridOfBody; 
	//int attackDamageOfCard; 
	//string typeOfAttack;
	int XDimOfBody;
	int YDimOfBody;



	void Start ()
	{
		//DontDestroyOnLoad (gameObject); //Allows Loader to carry over into new scene 
		LoadXML (); //Loads XML File. Code below. 
		StartCoroutine(AssignData()); //Starts assigning XML data to data List. Code below
//		Debug.Log("inside bodydata count "+bodyData.Count);
	}

	void LoadXML()

	{
		//Assigning Xdocument xmlDoc. Loads the xml file from the file path listed. 
		xmlDoc = XDocument.Load("assets/XMLdata/BodyHitBox.xml");

		//This basically breaks down the XML Document into XML Elements. Used later. 
		items = xmlDoc.Descendants("body").Elements ();
	}

	//this is our coroutine that will actually read and assign the XML data to our List 
	IEnumerator AssignData()
	{

		/*foreach allows us to look at every Element of our XML file and do something with each one. Basically, this line is saying “for each element in the xml document, do something.*/ 
		foreach (var item in items)
		{

			if (nameOfBody != item.Parent.Attribute ("name").Value.Trim ()) {		//if the next element has a parent with a new name, select that parrent and assign all it's children to these values
				nameOfBody = item.Parent.Attribute ("name").Value.Trim ();
				placement = item.Parent.Element ("placement").Value.Trim ();
				XDimOfBody = int.Parse (item.Parent.Element("Xdimensions").Value.Trim ());
				YDimOfBody = int.Parse (item.Parent.Element("Ydimensions").Value.Trim ());


				int numberXCord = item.Parent.Element("designShape").Element("line").Value.Trim().Length;	//the length of the design shape line of 1's and 0's
				linesOfHitBox = item.Parent.Element("designShape").Descendants();
				int numberYCord = item.Parent.Element("designShape").Descendants().Count();		//counts how many lines there are in the targeting grid, giving Y cords size
				int interationY = numberYCord-1;
//				Debug.Log(numberXCord);
//				Debug.Log(numberYCord);
				gridOfBody = new int[(int)numberXCord][];
				for (int i = 0; i < numberXCord; i++) {
					gridOfBody[i] = new int[(int)numberYCord];		//instantiating the grid beforehand
				}
				foreach (XElement line in linesOfHitBox){
					int interationX = 0;
//					Debug.Log(interationX);
//					Debug.Log(interationY);
					string lineOfNumbers = line.Value;
					foreach (char num in lineOfNumbers) {
						int newNum = (int)char.GetNumericValue(num);
						gridOfBody [interationX] [interationY] = newNum;
//						Debug.Log(newNum);
//						Debug.Log(interationX);
//						Debug.Log(interationY);
						interationX++;

					}
					interationY--;


				}
				//Debug.Log(gridOfBody[0][0]);
				bodyData.Add (new XMLBodyHitData(nameOfBody, placement, gridOfBody, XDimOfBody, YDimOfBody));

				//
			}
		}
//		Debug.Log ("bodydata after add " + bodyData.Count);
		//finishedLoading = true; //tell the program that we’ve finished loading data. 
		yield return null;
	}
	public void getBodyData(){
//		Debug.Log("Count grab "+bodyData.Count);
	}
}

// This class is used to assign our XML Data to objects in a list so we can call on them later. 
public class XMLBodyHitData {
	public string nameOfBody, placement;
	public int[][] gridOfBody;
	public int XDimOfBody, YDimOfBody;
	// Create a constructor that will accept multiple arguments that can be assigned to our variables. 
	public XMLBodyHitData (string nameOfBodyT, string placementT, int[][] gridOfBodyT, int Xdim, int Ydim)
	{
		nameOfBody = nameOfBodyT;
		placement = placementT;
		gridOfBody = gridOfBodyT;
		XDimOfBody = Xdim;
		YDimOfBody = Ydim;
//		Debug.Log (nameOfBody +" "+ placement +" "+ XDimOfBody +" "+ YDimOfBody );

	}

}