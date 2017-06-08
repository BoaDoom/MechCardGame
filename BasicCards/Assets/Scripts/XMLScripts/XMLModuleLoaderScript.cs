using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic; //Needed for Lists
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using System.Xml.Linq; //Needed for XDocument

public class XMLModuleLoaderScript : MonoBehaviour {

	// Use this for initialization
	bool finishedLoading = false;

	XDocument xmlDoc; //create Xdocument. Will be used later to read XML file 
	IEnumerable<XElement> items; //Create an Ienumerable list. Will be used to store XML Items. 
	public List <XMLModuleData> data = new List <XMLModuleData>(); //Initialize List of XMLData objects.

	int iteration = 0;
	//int tempvar = 0;
	//bool finishedLoading = false;


	public string moduleType;
	public int cardNumber, IDnum;

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
		xmlDoc = XDocument.Load("assets/XMLdata/Module.xml");
		//This basically breaks down the XML Document into XML Elements. Used later. 
		items = xmlDoc.Descendants("Module").Elements ();
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
//			int tempIDNum = int.Parse (item.Parent.Attribute ("IDnum").Value.Trim ()); 
//			print (tempIDNum);
			if(item.Parent.Attribute("IDnum").Value == iteration.ToString ())
			{
				IDnum = int.Parse (item.Parent.Attribute ("IDnum").Value.Trim ()); 
//				print ("nameofmodule: "+IDnum);
				cardNumber = int.Parse(item.Parent.Element("cardNumber").Value.Trim ()); 
				moduleType = item.Parent.Parent.Name.ToString(); 
//				attackDamageOfCard = int.Parse (item.Parent.Element("attack").Value.Trim ()); 
//				typeOfAttack = item.Parent.Element ("attackType").Value.Trim ();
				/*Create a new Index in the List, which will be a new XMLData object and pass the previously assigned variables as arguments so they get assigned to the new object’s variables.*/
				XMLModuleData tempXMLModule = new XMLModuleData();
				tempXMLModule.MakeNewData (IDnum, cardNumber, moduleType);
				data.Add (tempXMLModule);
					
//				print ("IDnum "+ IDnum + " cardNumber "+ cardNumber+ " moduleType "+moduleType);
				/*To test and make sure the data has been applied to properly, print out the musicClip name from the data list’s current index. This will let us know if the objects in the list have been created successfully and if their variables have been assigned the right values.*/
				//				Debug.Log (data[iteration-1].nameOfCard);
				//				Debug.Log (data[iteration-1].cardSpriteNum);
				iteration++; //increment the iteration by 1
			}
		}
		finishedLoading = true;
//		print ("moduleXML finished");
		yield return null;
	}
	public bool checkIfFinishedLoading(){
		return finishedLoading;
	}
}

// This class is used to assign our XML Data to objects in a list so we can call on them later. 
public class XMLModuleData {
	public string moduleType;
	public int IDnum, cardNumber;
	// Create a constructor that will accept multiple arguments that can be assigned to our variables. 
	public void MakeNewData(int incIDnumOfModule, int incCardNumber, string incModuleType)
	{
		IDnum = incIDnumOfModule;
		cardNumber = incCardNumber;
		moduleType = incModuleType;
	}
	public void CopyData(XMLModuleData incXMLModuleData)
	{
		IDnum = incXMLModuleData.IDnum;
		cardNumber = incXMLModuleData.cardNumber;
		moduleType = incXMLModuleData.moduleType;
	}
}