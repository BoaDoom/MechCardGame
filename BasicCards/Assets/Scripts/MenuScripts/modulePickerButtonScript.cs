using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class modulePickerButtonScript : Selectable{
//	SelectionBaseAttribute selectorScript;
	ModulePickerScript modulePickerScript;
	public int bodyPartNumber;
	int moduleIDnumber;
	public Color startColor = Color.grey;
	public Color startNormalColor;

	public bool completedStart = false;

	public bool selected = false;
	public bool pressable = true;
//	public Color highlightedColor;
//	public Color selectedColor;
	//public string bodyPartName;
//	protected override void Start(){
//		bodyPartVariationPanel = gameObject.GetComponentInParent<BodyPartVariationPanel>();
//		//gameObject.GetComponent<Text>().color =  startColor;
//		//print (startColor);
//		//Changes the button's Disabled color to the new color.
//		ColorBlock cb = gameObject.GetComponent<modulePickerButtonScript>().colors;
//		startNormalColor = cb.normalColor;
////		button.colors = cb;
////		print("Done text");
//		completedStart = true;
//	}
	public IEnumerator ManualStart(int IDnumOfModule, int incomingModuleIDnumber){
//		print ("manually started");
		ModulePickerScript modulePickerScriptTemp = gameObject.GetComponentInParent<ModulePickerScript>();
		if (modulePickerScriptTemp != null) {
			modulePickerScript = modulePickerScriptTemp;
		} else {
			print ("Couldnt find modulePickerScriptTemp");
		}

		modulePickerScript = gameObject.GetComponentInParent<ModulePickerScript>();
//		print ("name: "+ modulePickerScript.name);
		//gameObject.GetComponent<Text>().color =  startColor;
		//print (startColor);
		//Changes the button's Disabled color to the new color.
		ColorBlock cb = gameObject.GetComponent<modulePickerButtonScript>().colors;
		startNormalColor = cb.normalColor;
		//		button.colors = cb;
		//		print("Done text");
		moduleIDnumber = incomingModuleIDnumber;
//		print ("IDnumOfModule: "+ IDnumOfModule);
//		print ("pre set text: "+ gameObject.GetComponent<Text>().text);
		gameObject.GetComponent<Text>().text = "Module " +IDnumOfModule;
//		print ("after set text: "+ gameObject.GetComponent<Text>().text);
		completedStart = true;
//		StartCoroutine( modulePickerScript.setModuleAsSelected (1));
		yield return null;
	}

//	public void setAsSelected(){
//		//selectorScript.
//	}
	public override void OnPointerDown(PointerEventData eventData){
//		print ("pointer down");
		if (pressable) {
			if (selected) { //toggles off the the module if it is already selected
				markAsUnselected ();
//				selected = false;
//				ColorBlock cb = gameObject.GetComponent<modulePickerButtonScript>().colors;
//				cb.normalColor = startNormalColor;
//				gameObject.GetComponent<modulePickerButtonScript>().colors = cb;
				StartCoroutine (modulePickerScript.upwardsModuleDeselected (moduleIDnumber));

			} else if (!selected){
//			print ("test onSelect");
				markAsSelected ();
//				selected = true;
//				ColorBlock cb = gameObject.GetComponent<modulePickerButtonScript> ().colors;
//				cb.normalColor = Color.green;
//				gameObject.GetComponent<modulePickerButtonScript> ().colors = cb;
				StartCoroutine (modulePickerScript.upwardsModuleSelected (moduleIDnumber));
//				print (moduleIDnumber);

			}
		}
	}

//	public override void OnDeselect(BaseEventData eventData){
//		if (modulePickerScript.getCurrentModuleSelectedIDnumber () != bodyPartNumber) {	//only turns off the the body part color on deselection if the parent panel is storing a different number
//			selected = false;
//			turnOffSelectedColor ();
//		}
//		StartCoroutine (modulePickerScript.upwardsModuleDeselected (moduleIDnumber));
//		print ("wtf");
//	}
	public IEnumerator disableButton(){
//		print ("button disabled");
		pressable = false;
		ColorBlock cb = gameObject.GetComponent<modulePickerButtonScript>().colors;
		cb.normalColor = Color.black;
		gameObject.GetComponent<modulePickerButtonScript>().colors = cb;
		yield return null;
	}
	public IEnumerator enableButton(){
//		print ("button enabled");
		pressable = true;
		ColorBlock cb = gameObject.GetComponent<modulePickerButtonScript> ().colors;
		cb.normalColor = startNormalColor;
		gameObject.GetComponent<modulePickerButtonScript> ().colors = cb;
		yield return null;
	}

	public int getBodyPartNumber(){
		return bodyPartNumber;
	}
	public int getModuleIDNumber(){
		return moduleIDnumber;
	}
	public void markAsUnselected(){
		selected = false;
		ColorBlock cb = gameObject.GetComponent<modulePickerButtonScript>().colors;
		cb.normalColor = startNormalColor;
		gameObject.GetComponent<modulePickerButtonScript>().colors = cb;
	}
	public void markAsSelected(){
		selected = true;
		ColorBlock cb = gameObject.GetComponent<modulePickerButtonScript> ().colors;
		cb.normalColor = Color.green;
		gameObject.GetComponent<modulePickerButtonScript> ().colors = cb;
	}

	public bool completedStartQuery(){
		return completedStart;
	}
}
