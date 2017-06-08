using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePickerScript : MonoBehaviour {

	public modulePickerButtonScript buttonPrefab;
	public BodyPartSelectionCanvasScript partSelectionCanvas;
//	BodyPartPreviewWindowScript parentBodyPartWindow;

	public bool ignoreLayout = true;
	private string socketType;
	int currentSelectedModuleIDnumber = -1;
//	int currentAssignedModulePickerIDnumber;
	int moduleSocketCountInBP; 		//the socket count for each limb is a max of 3, each module picker script has a label of which socket it is, 0,1 or 2
	BodyPartPickerPanel bodyPartPickerPanelParent;
	XMLModuleData[] weaponModules;
	XMLModuleData[] utilityModules;
	XMLModuleData[] genericModules;

	modulePickerButtonScript[] listOfAllTheText;
	public IEnumerator ManualStart(BodyPartPickerPanel incomingParentObject){
		bodyPartPickerPanelParent = incomingParentObject;
		GameObject canvasFinderTemp = GameObject.FindWithTag ("PartSelectionCanvas");
		if (canvasFinderTemp != null) {
			partSelectionCanvas = canvasFinderTemp.GetComponent<BodyPartSelectionCanvasScript> ();
		}

		yield return null;
	}
	public IEnumerator takeModuleInfo(string incomingSocketType,  int incomingSocketNumber){
//		parentBodyPartWindow = incomingParentWindow;
//		currentAssignedModulePickerIDnumber = incomingModulePickerIDnumber;
		moduleSocketCountInBP = incomingSocketNumber;
//		partSelectionCanvas = incomingPartSelectionCanvas;
		socketType = incomingSocketType;
//		print (partSelectionCanvas.name);
		List<int> listOfModulesInUse = partSelectionCanvas.getModulesAlreadyInUse ();

		if (incomingSocketType == "weapon") {
			weaponModules = partSelectionCanvas.getListOfModules ("Weapons");		//grabs all availible modules of the weapon type
			foreach (XMLModuleData modularData in weaponModules) {
				modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);

				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
//				newButton1.GetComponent<Transform> ().localScale = Vector3.one;
				StartCoroutine( newButton1.ManualStart(modularData.IDnum, modularData.cardNumber));		//needs to start after assigning of parent because the button grabs it's parent to be able to send info back
//				print("modulardata " + modularData.moduleType);
			}
		}
		if (incomingSocketType == "utility") {
			utilityModules = partSelectionCanvas.getListOfModules ("Utility");		//grabs all availible modules of the utility type
			foreach (XMLModuleData modularData in utilityModules) {
				modulePickerButtonScript newButton1 = Instantiate (buttonPrefab, gameObject.GetComponent<Transform> ().position, gameObject.GetComponent<Transform> ().rotation);

				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
//				newButton1.GetComponent<Transform> ().localScale = Vector3.one;
//				newButton1.GetComponent<Transform> ().
				StartCoroutine( newButton1.ManualStart(modularData.IDnum, modularData.cardNumber));		//needs to start after assigning of parent because the button grabs it's parent to be able to send info back
//								print("modulardata " + modularData.moduleType);
			}
		}
		if (incomingSocketType == "both") {
			genericModules = partSelectionCanvas.getListOfModules ("Both");			//grabs all availible modules of both types
			foreach (XMLModuleData modularData in genericModules) {
				modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);

				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
//				newButton1.GetComponent<Transform> ().localScale = Vector3.one;
				StartCoroutine( newButton1.ManualStart(modularData.IDnum, modularData.cardNumber));		//needs to start after assigning of parent because the button grabs it's parent to be able to send info back
//								print("modulardata " + modularData.moduleType);
			}
		}
		listOfAllTheText = gameObject.GetComponentsInChildren<modulePickerButtonScript> ();	
		foreach(modulePickerButtonScript buttonText in listOfAllTheText){
			foreach(int moduleIDinUse in listOfModulesInUse){
				if (moduleIDinUse == buttonText.getModuleIDNumber ()) {
					StartCoroutine(buttonText.disableButton ());
				}
			}
		}
		yield return null;

//		StartCoroutine (setPartSelected (1));
//		modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//		modulePickerButtonScript newButton2 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//		newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
//		newButton2.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
		
	}
	public int getCurrentModuleSelectedIDnumber(){
		return currentSelectedModuleIDnumber;
	}

	public IEnumerator upwardsModuleSelected(int incomingModuleIDnumber){		//sending the value to the greater UI canvas to get the info about the modules
		currentSelectedModuleIDnumber = incomingModuleIDnumber;		//setting current part selected number value
		foreach(modulePickerButtonScript moduleText in listOfAllTheText){		//turns off all the text buttons if they are not the currently selected option
//			print("loops");
			if (moduleText.selected && (incomingModuleIDnumber != moduleText.getModuleIDNumber())){
//				moduleText.markAsUnselected ();
//				StartCoroutine( downwardsModuleDeselected(incomingModuleIDnumber));
//				StartCoroutine( upwardsModuleDeselected(incomingModuleIDnumber));
				moduleText.markAsUnselected();
				StartCoroutine(bodyPartPickerPanelParent.upwardsModuleDeselected (moduleText.getModuleIDNumber(), moduleSocketCountInBP));
			}
			if (moduleText.selected && (incomingModuleIDnumber == moduleText.getModuleIDNumber())) {
//				moduleText.markAsUnselected ();
//				print("inside");
				StartCoroutine(bodyPartPickerPanelParent.upwardsModuleSelected (currentSelectedModuleIDnumber, moduleSocketCountInBP));	//turns off the selection of the module that was selected before this one
			}
//			else if(moduleText.getModuleIDNumber () == currentSelectedModuleIDnumber){
//				StartCoroutine(bodyPartPickerPanelParent.upwardsModuleDeselected (currentSelectedModuleIDnumber, moduleSocketCountInBP));		//sending the signal up the chain that a button was selected
//			}
		}

		yield return null;
	}
	public IEnumerator upwardsModuleDeselected(int incomingModuleIDnumber){
		StartCoroutine(bodyPartPickerPanelParent.upwardsModuleDeselected (incomingModuleIDnumber, moduleSocketCountInBP));
		yield return null;
	}

	public IEnumerator downwardsModuleSelected(int incomingModuleIDnumber){		//signal coming down the chain that a button was selected
		foreach (modulePickerButtonScript moduleText in listOfAllTheText) {
			if (moduleText.getModuleIDNumber () == incomingModuleIDnumber){
				if (!moduleText.selected) {
					StartCoroutine (moduleText.disableButton());
				}
			}
		}
		yield return null;
	}
	public IEnumerator downwardsModuleDeselected(int incomingModuleIDnumber){		//signal coming down the chain that a button was deselected
		foreach (modulePickerButtonScript moduleText in listOfAllTheText) {
			if (moduleText.getModuleIDNumber () == incomingModuleIDnumber){
//				if (bodyPartPickerPanelParent.markedForChange || moduleText.pressable) {
					StartCoroutine (moduleText.enableButton ());
//				}
			}
//			print ("turned back on module picker");
		}
		yield return null;
//		print ("turned back on outside module picker");
		//currentSelectedModuleIDnumber = -1;
	}
	public string getSocketType(){
		return socketType;
	}
	// Update is called once per frame
	public IEnumerator destroyCompletely(){
		StartCoroutine( upwardsModuleDeselected (currentSelectedModuleIDnumber));
		Destroy (gameObject);
		yield return null;
	}
}
