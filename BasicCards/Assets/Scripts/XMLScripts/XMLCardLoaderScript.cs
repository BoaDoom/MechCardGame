using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic; //Needed for Lists
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using System.Xml.Linq; //Needed for XDocument

public class XMLCardLoaderScript : MonoBehaviour {
	bool finishedLoading = false;

	XDocument xmlDoc; //create Xdocument. Will be used later to read XML file 
	IEnumerable<XElement> items; //Create an Ienumerable list. Will be used to store XML Items. 
	public List <XMLData> data = new List <XMLData>(); //Initialize List of XMLData objects.

	int iteration = 0;
	//int tempvar = 0;
	//bool finishedLoading = false;

	int cardSpriteNum = 0;
	string nameOfCard; 
	int rankOfCard; 
	float attackDamageOfCard; 
	string typeOfAttack;

	void Start ()
	{
		//DontDestroyOnLoad (gameObject); //Allows Loader to carry over into new scene 
		LoadXML (); //Loads XML File. Code below. 
		StartCoroutine(AssignData()); //Starts assigning XML data to data List. Code below

	}
	void Update ()
	{

	}

	void LoadXML()

	{
		//Assigning Xdocument xmlDoc. Loads the xml file from the file path listed. 
		xmlDoc = XDocument.Load("assets/XMLdata/cards.xml");
		//This basically breaks down the XML Document into XML Elements. Used later. 
		items = xmlDoc.Descendants("card").Elements ();
	}
//	int loaderTest = 0;
	//this is our coroutine that will actually read and assign the XML data to our List 
	IEnumerator AssignData()
	{

		/*foreach allows us to look at every Element of our XML file and do something with each one. Basically, this line is saying “for each element in the xml document, do something.*/ 
		foreach (var item in items)
		{

//			Debug.Log (item.Parent.Attribute("number").Value);
//			Debug.Log (iteration.ToString ());
			/*Determine if the <page number> attribute in the XML is equal to whatever our current iteration of the loop is. If it is, then we want to assign our variables to the value of the XML Element that we need.*/
			//Debug.Log ("XML card loader: " +item.Name);
			if(item.Parent.Attribute("number").Value == iteration.ToString ())
			{
				cardSpriteNum = int.Parse (item.Parent.Attribute ("number").Value.Trim ()); 
				nameOfCard = item.Parent.Element("name").Value.Trim (); 
				rankOfCard = int.Parse (item.Parent.Element("rank").Value.Trim ()); 
				attackDamageOfCard = int.Parse (item.Parent.Element("attack").Value.Trim ()); 
				typeOfAttack = item.Parent.Element ("attackType").Value.Trim ();
				/*Create a new Index in the List, which will be a new XMLData object and pass the previously assigned variables as arguments so they get assigned to the new object’s variables.*/
				data.Add (new XMLData(cardSpriteNum, nameOfCard, rankOfCard, attackDamageOfCard, typeOfAttack));
				/*To test and make sure the data has been applied to properly, print out the musicClip name from the data list’s current index. This will let us know if the objects in the list have been created successfully and if their variables have been assigned the right values.*/
//				Debug.Log (data[iteration-1].nameOfCard);
//				Debug.Log (data[iteration-1].cardSpriteNum);
				iteration++; //increment the iteration by 1
			}
		}
		finishedLoading = true;
//		print ("cardXML finished");
		yield return null;
	}
	public bool checkIfFinishedLoading(){
		return finishedLoading;
	}
}

// This class is used to assign our XML Data to objects in a list so we can call on them later. 
public class XMLData {
	public string nameOfCard, typeOfAttack;
	public float attackDamageOfCard;
	public int cardSpriteNum, rankOfCard;
// Create a constructor that will accept multiple arguments that can be assigned to our variables. 
	public XMLData (int spriteNum, string name, int rank, float attack, string atkType)
	{
		cardSpriteNum = spriteNum;
		nameOfCard = name;
		rankOfCard = rank;
		attackDamageOfCard = attack;
		typeOfAttack = atkType;
	}

}