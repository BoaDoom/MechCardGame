using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic; //Needed for Lists
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using System.Xml.Linq; //Needed for XDocument
using System.Linq;

public class XMLWeaponHitLoaderScript : MonoBehaviour {
	bool finishedLoading = false;

	XDocument xmlDoc; //create Xdocument. Will be used later to read XML file 
	IEnumerable<XElement> items; //Create an Ienumerable list. Will be used to store XML Items. 
	IEnumerable<XElement> linesOfHit;
	public List <XMLWeaponHitData> data = new List <XMLWeaponHitData>(); //Initialize List of XMLWeaponData objects.

	//int iteration = 0;
	//bool newName = true;
	//int tempvar = 0;
	//bool finishedLoading = false;

	string nameOfAttack = "none"; 
	string placement;
	XElement designShape;
	int[][] gridOfHit; 
	//int attackDamageOfCard; 
	//string typeOfAttack;



	void Start ()
	{
		//DontDestroyOnLoad (gameObject); //Allows Loader to carry over into new scene 
		LoadXML (); //Loads XML File. Code below. 
		StartCoroutine(AssignData()); //Starts assigning XML data to data List. Code below
	}

	void LoadXML()

	{
		//Assigning Xdocument xmlDoc. Loads the xml file from the file path listed. 
		xmlDoc = XDocument.Load("assets/XMLdata/WeaponHitBox.xml");

		//This basically breaks down the XML Document into XML Elements. Used later. 
		items = xmlDoc.Descendants("hitbox").Elements ();
	}

	//this is our coroutine that will actually read and assign the XML data to our List 
	IEnumerator AssignData()
	{

		/*foreach allows us to look at every Element of our XML file and do something with each one. Basically, this line is saying “for each element in the xml document, do something.*/ 
		foreach (var item in items)
		{

			if (nameOfAttack != item.Parent.Attribute ("name").Value.Trim ()) {		//if the next element has a parent with a new name, select that parrent and assign all it's children to these values
				nameOfAttack = item.Parent.Attribute ("name").Value.Trim ();
				placement = item.Parent.Element ("placement").Value.Trim ();

				int interationX = 0;

				int numberXCord = item.Parent.Element("designShape").Element("line").Value.Trim().Length;	//the length of the design shape line of 1's and 0's
				linesOfHit = item.Parent.Element("designShape").Descendants();
				int numberYCord = item.Parent.Element("designShape").Descendants().Count();		//counts how many lines there are in the targeting grid, giving Y cords size
				gridOfHit = new int[(int)numberXCord][];
				foreach (XElement line in linesOfHit){
					int interationY = 0;
					gridOfHit[interationX] = new int[(int)numberYCord];
					string lineOfNUmbers = line.Value;
					foreach (char num in lineOfNUmbers) {
						int newNum = (int)char.GetNumericValue(num);
						gridOfHit [interationX] [interationY] = newNum;
						interationY++;
					}
					interationX++;
				}
				//Debug.Log(gridOfHit[0][0]);
				data.Add (new XMLWeaponHitData(nameOfAttack, placement, gridOfHit));
//
			}
		}
		finishedLoading = true;
//		print ("weaponXML finished");
		yield return null;
	}
	public bool checkIfFinishedLoading(){
		return finishedLoading;
	}
}

// This class is used to assign our XML Data to objects in a list so we can call on them later. 
public class XMLWeaponHitData {
	public string nameOfAttack, placement;
	public int[][] gridOfHit;
// Create a constructor that will accept multiple arguments that can be assigned to our variables. 
	public XMLWeaponHitData (string nameOfAttackT, string placementT, int[][] gridOfHitT)
	{
		nameOfAttack = nameOfAttackT;
		placement = placementT;
		gridOfHit = gridOfHitT;
	}

}