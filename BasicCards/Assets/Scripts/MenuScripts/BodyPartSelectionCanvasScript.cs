using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;

public class BodyPartSelectionCanvasScript : MonoBehaviour {

	public EventSystem eventSystem;	
	public SceneTransferVariablesScript sceneTransferVariablesScript;

	BodyPartPickerPanel[] listOfPickerPanels;
	PartPickerAreaScript partPickerAreaScript;
//	BodyPartDataHolder partData = null;
	public BPartGenericScript bPartGenericScript;

	BPartGenericScript tempBodyPart;

	BPartXMLReaderScript bPartXMLReader;
	XMLModuleLoaderScript XMLModuleLoader;
//	public int what;
	BodyPartDataHolder headSelection = new BodyPartDataHolder();
	BodyPartDataHolder leftArmSelection = new BodyPartDataHolder();
	BodyPartDataHolder rightArmSelection = new BodyPartDataHolder();
	BodyPartDataHolder torsoSelection = new BodyPartDataHolder();
	BodyPartDataHolder leftShoulderSelection = new BodyPartDataHolder();
	BodyPartDataHolder rightShoulderSelection = new BodyPartDataHolder();
	BodyPartDataHolder legSelection = new BodyPartDataHolder();
//	TransferBodyPartInfo[] listOfPickedBodyParts = new TransferBodyPartInfo[5];

	public XMLModuleData[] listOfWeaponModules;
	public XMLModuleData[] listOfUtilityModules;

//	BodyPartPreviewWindowScript headWindow;
//	BodyPartPreviewWindowScript armWindow;
//	BodyPartPreviewWindowScript torsoWindow;
//	BodyPartPreviewWindowScript shoulderWindow;
//	BodyPartPreviewWindowScript legWindow;
//	BodyPartPreviewWindowScript[] allBPartWindows = new BodyPartPreviewWindowScript[5];		//5 is the number of type of parts, head, arm, torso, legs, shoulders;

	public List<int> alreadySelectedModules = new List<int>();


	public Button nextButton;
//	bool thingsChecked;
	public void Start(){
//		print ("bodypicker canvas has started");
		//grab XML data for modules and store it here for the ModulePickerScript to request and grab later
		GameObject loaderScriptTemp = GameObject.FindWithTag("MainLoader");		//grabbing the object with the body part info taken from xml data	
		if (loaderScriptTemp == null) {
//			print ("loader doesn't exist yet");
			SceneManager.LoadScene ("XMLLoaderScene");
			return;
		} 
		else if (loaderScriptTemp != null) {
//			print ("Actual start");
//			tempBodyPart = Instantiate (bPartGenericScript, Vector3.zero, gameObject.GetComponent<Transform>().rotation);

			bPartXMLReader = loaderScriptTemp.GetComponent<BPartXMLReaderScript> ();
			XMLModuleLoader = loaderScriptTemp.GetComponent<XMLModuleLoaderScript> ();

			int tempWeaponInt = 0;
			int tempUtilityInt = 0;
			foreach (XMLModuleData moduleData in XMLModuleLoader.data) {
				if (moduleData.moduleType == "Weapons") {
					tempWeaponInt++;
				}
				if (moduleData.moduleType == "Utility") {
					tempUtilityInt++;
				}
			}
			listOfWeaponModules = new XMLModuleData[tempWeaponInt];
			listOfUtilityModules = new XMLModuleData[tempUtilityInt];

			tempWeaponInt = 0;
			tempUtilityInt = 0;
			foreach(XMLModuleData moduleData in XMLModuleLoader.data){
				if (moduleData.moduleType == "Weapons") {
					listOfWeaponModules[tempWeaponInt] = new XMLModuleData();
					listOfWeaponModules [tempWeaponInt].CopyData (moduleData);
					tempWeaponInt++;
				}
				else if (moduleData.moduleType == "Utility") {
					listOfUtilityModules[tempUtilityInt] = new XMLModuleData();
					listOfUtilityModules [tempUtilityInt].CopyData (moduleData);
					tempUtilityInt++;
				}
			}
		
			GameObject sceneTransferVariablesScriptTemp = GameObject.FindWithTag ("SceneTransferVariables");
			if (sceneTransferVariablesScriptTemp != null) {
				sceneTransferVariablesScript = sceneTransferVariablesScriptTemp.GetComponent<SceneTransferVariablesScript> ();
			} else {
				print ("Couldnt find SceneTransferVariablesScript");
			}

			GameObject eventFinderTemp = GameObject.FindWithTag ("EventSystem");
			if (eventFinderTemp != null) {
				eventSystem = eventFinderTemp.GetComponent<EventSystem> ();
			} else {
				print ("Couldnt find event system");
			}
			nextButton.onClick.AddListener (checkToMoveToPlayScreen);
			//BPartGenericScript tempBodyPart;
			partPickerAreaScript = gameObject.GetComponentInChildren<PartPickerAreaScript> ();
			StartCoroutine (partPickerAreaScript.ManualStart ());

			listOfPickerPanels = gameObject.GetComponentsInChildren<BodyPartPickerPanel> ();
//			print ("list of all the panels" +listOfPickerPanels.Length);
			foreach (BodyPartPickerPanel panel in listOfPickerPanels) {
				panel.ManualStart ();
			}

		}
			
			//		print ("done panel");
	}

//	public void checkThing(int incomingint, int otherthing){
//		thingsChecked = true;
//	}
	public BPartGenericScript markSelectedBodyPart(BodyPartDataHolder incomingPartData, int incomingDesignatedDirection){		//almost the same as PlayerScript method populate body
//		print(incomingIDofPart);
//		print ("name "+ incomingPartData.name + "first moduleID "+ incomingPartData.moduleIDnum [0]);
		BodyPartDataHolder partData = new BodyPartDataHolder();
		partData.makeACopy( incomingPartData);
//		print(partData.name);
		string tempType = partData.typeOfpart;
		string leftOrRight = null;
		switch (incomingDesignatedDirection) {
		case(0):
			{
				leftOrRight = null;
				break;
			}
		case(1):
			{
				leftOrRight = "left";
				break;
			}
		case(2):
			{
				leftOrRight = "right";
				break;
			}
		}
		if (leftOrRight != null) {
			tempType = tempType + " " + leftOrRight;
		}


		//need to swap over the identifying variable from a string to the new class so it can carry the module info and choices. Maybe? needs to convey more info at some point
//		print("incoming selection"+ incomingSelection);
//		print(tempType);
		switch (tempType) {
		case("Head"):
			if (headSelection != null) {
				foreach (int moduleID in headSelection.moduleIDnum) {
					foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
						removeModuleFromList(moduleID);
						StartCoroutine (BPartPicker.downwardsModuleDeselected (moduleID));
						//					print ("trying do delete");
					}
				}
			}
			headSelection = partData; 
			break;
		case("Arm left"):
			if (leftArmSelection != null) {
				foreach (int moduleID in leftArmSelection.moduleIDnum) {
					foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
						removeModuleFromList(moduleID);
						StartCoroutine (BPartPicker.downwardsModuleDeselected (moduleID));
					}
				}
			}

			leftArmSelection =  partData;
//			print ("left arm selection "+leftArmSelection.moduleIDnum [0]);
//			print ("right arm selection "+rightArmSelection.moduleIDnum [0]);
			break;
		case("Arm right"):
			if (rightArmSelection != null) {
				foreach (int moduleID in rightArmSelection.moduleIDnum) {
					foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
						removeModuleFromList (moduleID);
						StartCoroutine (BPartPicker.downwardsModuleDeselected (moduleID));
					}
				}
			}
			rightArmSelection = partData;
//			print ("before right arm " + rightArmSelection.moduleIDnum [0]);
//			print ("before left arm " + leftArmSelection.moduleIDnum [0]);
			rightArmSelection.moduleIDnum [0] = 1;

//			print ("after left arm " + leftArmSelection.moduleIDnum [0]);
//			print ("after right arm " + rightArmSelection.moduleIDnum [0]);
			break;
		case("Torso"):
			if (torsoSelection != null) {
				foreach (int moduleID in torsoSelection.moduleIDnum) {
					foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
						removeModuleFromList(moduleID);
						StartCoroutine (BPartPicker.downwardsModuleDeselected (moduleID));
					}
				}
			}
			torsoSelection = partData;
			break;
		case("Shoulder left"):
			if (leftShoulderSelection != null) {
				foreach (int moduleID in leftShoulderSelection.moduleIDnum) {
					foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
						removeModuleFromList(moduleID);
						StartCoroutine (BPartPicker.downwardsModuleDeselected (moduleID));
					}
				}
			}
			leftShoulderSelection = partData;
			break;
		case("Shoulder right"):
			if (rightShoulderSelection != null) {
				foreach (int moduleID in rightShoulderSelection.moduleIDnum) {
					foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
						removeModuleFromList(moduleID);
						StartCoroutine (BPartPicker.downwardsModuleDeselected (moduleID));
					}
				}
			}
			rightShoulderSelection =  partData;

			break;
		case("Leg"):
			if (legSelection != null) {
				foreach (int moduleID in legSelection.moduleIDnum) {
					foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
						removeModuleFromList(moduleID);
						StartCoroutine (BPartPicker.downwardsModuleDeselected (moduleID));
					}
				}
			}
			legSelection =  partData;
			break;
		default:
			{
				Debug.Log ("Unknown bodypart in bodypartselection.markSelectedBodyPart");
				break;
			}
		}
//		print ("name "+ partData.name + "first moduleID "+ partData.moduleIDnum [0]);
//		BodyPartDataHolder tempJunk = new BodyPartDataHolder();
		tempBodyPart = Instantiate (bPartGenericScript, Vector3.zero, gameObject.GetComponent<Transform>().rotation);
		tempBodyPart.CreateNewPart (partData,  incomingDesignatedDirection);

		return tempBodyPart;
	}
	public void markSelectedBodyPartAsNull(int incomingIDofPart, int incomingDesignatedDirection){		//for deselecting the body part
		BodyPartDataHolder partData = new BodyPartDataHolder();
		partData = bPartXMLReader.getBodyDataByID (incomingIDofPart);
		//		print(partData.name);
		string tempType = partData.typeOfpart;
		string leftOrRight = null;
		switch (incomingDesignatedDirection) {
		case(0):
			{
				leftOrRight = null;
				break;
			}
		case(1):
			{
				leftOrRight = "left";
				break;
			}
		case(2):
			{
				leftOrRight = "right";
				break;
			}
		}
		if (leftOrRight != null) {
			tempType = tempType + " " + leftOrRight;
		}
		switch (tempType) {
		case("Head"):
			headSelection = null;
			break;
		case("Arm left"):
//			foreach (int moduleID in leftArmSelection.moduleIDnum) {
//				foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
//					StartCoroutine( BPartPicker.downwardsModuleDeselected (moduleID));
//				}
//			}
			leftArmSelection =  null;
			break;
		case("Arm right"):
//			foreach (int moduleID in rightArmSelection.moduleIDnum) {
//				foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
//					StartCoroutine( BPartPicker.downwardsModuleDeselected (moduleID));
//				}
//			}
			rightArmSelection =  null;
			break;
		case("Torso"):
//			foreach (int moduleID in torsoSelection.moduleIDnum) {
//				foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
//					StartCoroutine( BPartPicker.downwardsModuleDeselected (moduleID));
//				}
//			}
			torsoSelection = null;
			break;
		case("Shoulder left"):
//			foreach (int moduleID in leftShoulderSelection.moduleIDnum) {
//				foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
//					StartCoroutine( BPartPicker.downwardsModuleDeselected (moduleID));
//				}
//			}
			leftShoulderSelection = null;
			break;
		case("Shoulder right"):
//			foreach (int moduleID in rightShoulderSelection.moduleIDnum) {
//				foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
//					StartCoroutine( BPartPicker.downwardsModuleDeselected (moduleID));
//				}
//			}
			rightShoulderSelection =  null;
			break;
		case("Leg"):
//			foreach (int moduleID in legSelection.moduleIDnum) {
//				foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
//					StartCoroutine( BPartPicker.downwardsModuleDeselected (moduleID));
//				}
//			}
			legSelection =  null;
			break;
		default:
			Debug.Log ("Unknown bodypart");
			break;
		}
	}
	public bool checkIfBodyIsComplete(){
		return (headSelection != null && rightArmSelection != null && leftArmSelection != null && torsoSelection != null&&  leftShoulderSelection != null && rightShoulderSelection != null && legSelection != null
		&& true);
	}
	public void onClick(){
		print ("clicked");
	}
	public void checkToMoveToPlayScreen(){
		if (checkIfBodyIsComplete() && (alreadySelectedModules.Count > 0)) {
			AllPickedBodyParts allPickedBodyPartsTemp = new AllPickedBodyParts ();
//			print ("final left arm selection "+leftArmSelection.moduleIDnum [0]);
//			print ("final right arm selection "+rightArmSelection.moduleIDnum [0]);
			allPickedBodyPartsTemp.setAllPickedBodyParts(headSelection, leftArmSelection, rightArmSelection, torsoSelection, leftShoulderSelection, rightShoulderSelection, legSelection);
//			print (allPickedBodyPartsTemp.pickedHead);
//			sceneTransferVariablesScript.bleh ();
//			sceneTransferVariablesScript.setModulesPicked(alreadySelectedModules);
			sceneTransferVariablesScript.setPartsPicked(allPickedBodyPartsTemp);
//			print (allPickedBodyPartsTemp.pickedHead.moduleIDnum [0]);
			SceneManager.LoadScene ("_Main");
		} else {
			Debug.Log ("You are missing some body parts or no modules are selected");
		}
	}



	public XMLModuleData[] getListOfModules(string incomingRequestForList){
//		XMLModuleData[] listOfAvailibleModules = new XMLModuleData()[];
		if (incomingRequestForList == "Weapons"){
			return listOfWeaponModules;
		}
		if (incomingRequestForList == "Utility"){
			return listOfUtilityModules;
		}
		if (incomingRequestForList == "Both"){
			int tempWeaponListCount = listOfWeaponModules.Length;
			int tempUtilityListCount = listOfUtilityModules.Length;
			XMLModuleData[] listOfBothTypesOfModules = new XMLModuleData[tempWeaponListCount+tempUtilityListCount];
			for (int i = 0; i < tempWeaponListCount; i++) {
				listOfBothTypesOfModules [i] = listOfWeaponModules[i];
			}
			for (int i = 0; i < tempUtilityListCount; i++) {
				listOfBothTypesOfModules [tempWeaponListCount+i] = listOfUtilityModules[i];
			}
			return listOfBothTypesOfModules;
		}
		print ("something went wrong with gettign the list of modules");
		return null;
	}
	/////////////////////////////////////////////////////////////// 05-25-17
//	public void previewWindowTransfer(BodyPartPreviewWindowScript incomingBPartWindow){		//on startup each bpartpreviewwindow sends a reference to itself to the canvas
//		string tempNameString = incomingBPartWindow.getTypeOfBPartOnDisplay();
//		switch (tempNameString) {
//		case("Head"):
//			headWindow = incomingBPartWindow;
//			allBPartWindows [0] = headWindow;
//			break;
//		case("Arm"):
//			armWindow = incomingBPartWindow;
//			allBPartWindows [1] = armWindow;
//			break;
//		case("Torso"):
//			torsoWindow = incomingBPartWindow;
//			allBPartWindows [2] = torsoWindow;
//			break;
//		case("Shoulder"):
//			shoulderWindow = incomingBPartWindow;
//			allBPartWindows [3] = shoulderWindow;
//			break;
//		case("Leg"):
//			legWindow = incomingBPartWindow;
//			allBPartWindows [4] = legWindow;
//			break;
//		default:
//			Debug.Log ("preview window did not transfer correctly");
//			break;
//		}
//	}
	public IEnumerator upwardsModuleSelected(int incomingModuleIDnumber, string incomingModuleBPartName, int incomingModuleSocketCountInBP){		//coming from
		alreadySelectedModules.Add (incomingModuleIDnumber);
//		print ("before right arm " + rightArmSelection.moduleIDnum [incomingModuleSocketCountInBP]);
//		print ("before left arm " + leftArmSelection.moduleIDnum [incomingModuleSocketCountInBP]);
//		rightArmSelection.moduleIDnum [incomingModuleSocketCountInBP] = incomingModuleIDnumber;
//
//		print ("after left arm " + leftArmSelection.moduleIDnum [incomingModuleSocketCountInBP]);
//		print ("after right arm " + rightArmSelection.moduleIDnum [incomingModuleSocketCountInBP]);
//		print (incomingModuleIDnumber +" "+ incomingModuleBPartName +" "+ incomingModuleSocketCountInBP);
		switch (incomingModuleBPartName) {
		case("Head"):
			headSelection.moduleIDnum [incomingModuleSocketCountInBP] = incomingModuleIDnumber;
			break;
		case("LeftArm"):
			leftArmSelection.moduleIDnum [incomingModuleSocketCountInBP] = incomingModuleIDnumber;
//			print ("right arm " + rightArmSelection.moduleIDnum [incomingModuleSocketCountInBP]);
//			print ("left arm " + leftArmSelection.moduleIDnum [incomingModuleSocketCountInBP]);
			break;
		case("RightArm"):
			rightArmSelection.moduleIDnum[incomingModuleSocketCountInBP] = incomingModuleIDnumber;
//			print ("right arm " + rightArmSelection.moduleIDnum [incomingModuleSocketCountInBP]);
//			print ("left arm " + leftArmSelection.moduleIDnum [incomingModuleSocketCountInBP]);
			break;
		case("Torso"):
			torsoSelection.moduleIDnum[incomingModuleSocketCountInBP] = incomingModuleIDnumber;
			break;
		case("LeftShoulder"):
			leftShoulderSelection.moduleIDnum[incomingModuleSocketCountInBP] = incomingModuleIDnumber;
			break;
		case("RightShoulder"):
			rightShoulderSelection.moduleIDnum[incomingModuleSocketCountInBP] = incomingModuleIDnumber;
			break;
		case("Leg"):
			legSelection.moduleIDnum [incomingModuleSocketCountInBP] = incomingModuleIDnumber;
//			print (incomingModuleIDnumber);
			break;
		default:
			Debug.Log (" upwardsModuleSelected Unknown bodypart");
			break;
		}
//		print ("after left arm " + leftArmSelection.moduleIDnum [incomingModuleSocketCountInBP]);
//		print ("after right arm " + rightArmSelection.moduleIDnum [incomingModuleSocketCountInBP]);
		foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
			StartCoroutine( BPartPicker.downwardsModuleSelected (incomingModuleIDnumber));
		}
//		print ("after left arm " + leftArmSelection.moduleIDnum [incomingModuleSocketCountInBP]);
//		print ("after right arm " + rightArmSelection.moduleIDnum [incomingModuleSocketCountInBP]);
		yield return null;
	}
	public IEnumerator upwardsModuleDeselected(int incomingModuleIDnumber, string incomingModuleBPartName, int incomingModuleSocketCountInBP){
//		print (incomingModuleBPartName);
		switch (incomingModuleBPartName) {
		case("Head"):
//			print (headSelection.GetType());
			headSelection.moduleIDnum[incomingModuleSocketCountInBP] = -1;
			break;
		case("LeftArm"):
			leftArmSelection.moduleIDnum[incomingModuleSocketCountInBP] = -1;
			break;
		case("RightArm"):
			rightArmSelection.moduleIDnum[incomingModuleSocketCountInBP] = -1;
			break;
		case("Torso"):
			torsoSelection.moduleIDnum[incomingModuleSocketCountInBP] = -1;
			break;
		case("LeftShoulder"):
			leftShoulderSelection.moduleIDnum[incomingModuleSocketCountInBP] = -1;
			break;
		case("RightShoulder"):
			rightShoulderSelection.moduleIDnum[incomingModuleSocketCountInBP] = -1;
			break;
		case("Legs"):
			legSelection.moduleIDnum[incomingModuleSocketCountInBP] = -1;
			break;
		default:
			Debug.Log ("Unknown module");
			break;
		}

		int tempCount = alreadySelectedModules.Count;
		for (int i = 0; i < tempCount; i++) {
			if (alreadySelectedModules[i] == incomingModuleIDnumber) {
				alreadySelectedModules.Remove(incomingModuleIDnumber);
				i--;
				tempCount--;
			}
		}

		foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
			StartCoroutine(BPartPicker.downwardsModuleDeselected (incomingModuleIDnumber));
//			print("trying to deselect "+incomingModuleIDnumber);
		}
		yield return null;
//		print ("outside?");
	}
	public List<int> getModulesAlreadyInUse(){		//used for any new module pickers buttons to check to see if their module is turned off
		return alreadySelectedModules;
	}
	public List<BodyPartDataHolder> getAllBodyDataForType(string BpartType){
		return bPartXMLReader.getAllBodyDataForType (BpartType);
	}
	public void removeModuleFromList(int incomingModuleID){
		int tempCount = alreadySelectedModules.Count;
		for (int i = 0; i < tempCount; i++) 
		{
			if (alreadySelectedModules[i] == incomingModuleID) 
			{
				alreadySelectedModules.Remove(incomingModuleID);
				i--;
				tempCount--;
			}
		}
	}
}
//public class TransferBodyPartInfo{
//	public string nameOfPart{ get; set; }
//	int[] listOfSelectedModules;
//	public TransferBodyPartInfo(){
//	}
//}

