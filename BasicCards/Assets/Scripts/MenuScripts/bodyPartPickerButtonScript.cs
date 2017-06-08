using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class bodyPartPickerButtonScript : Selectable{
//	SelectionBaseAttribute selectorScript;
	BodyPartPickerPanel bodyPartPickerPanel;
	int bodyPartIDnumber;
	int pickerListNumber;
	public Color startColor = Color.grey;
	public Color startNormalColor;

	public bool completedStart = false;
//	public Color highlightedColor;
//	public Color selectedColor;
	//public string bodyPartName;
//	protected override void Start(){
//		bodyPartPickerPanel = gameObject.GetComponentInParent<BodyPartPickerPanel>();
//		//gameObject.GetComponent<Text>().color =  startColor;
//		//print (startColor);
//		//Changes the button's Disabled color to the new color.
//		ColorBlock cb = gameObject.GetComponent<bodyPartPickerButtonScript>().colors;
//		startNormalColor = cb.normalColor;
////		button.colors = cb;
////		print("Done text");
//		completedStart = true;
//	}
	public IEnumerator ManualStart(string nameOfPart, int incomingBodyPartID, int incomingListNumber){
//		print(nameOfPart+" " +incomingBodyPartID+" " +incomingListNumber);
		bodyPartPickerPanel = gameObject.GetComponentInParent<BodyPartPickerPanel>();
		pickerListNumber = incomingListNumber;
//		print (bodyPartPickerPanel.name);
		gameObject.GetComponent<Text>().text =  nameOfPart;
//		print (nameOfPart);
		bodyPartIDnumber =incomingBodyPartID;
		//print (startColor);
		//Changes the button's Disabled color to the new color.
		ColorBlock cb = gameObject.GetComponent<bodyPartPickerButtonScript>().colors;
		startNormalColor = cb.normalColor;
		//		button.colors = cb;
		//		print("Done text");
		completedStart = true;
		yield return null;
	}

//	public void setAsSelected(){
//		//selectorScript.
//	}
	public override void OnPointerDown(PointerEventData eventData){
//		print ("pointer down");
//		print("bodyPartPickerPanel "+ bodyPartPickerPanel);
//		print ("numbers checked " + bodyPartPickerPanel.getPickerListNumber () +" "+pickerListNumber );
		if (bodyPartPickerPanel.getPickerListNumber () == pickerListNumber) { //toggles off the the body part color on deselection if the parent panel is storing the same number

			StartCoroutine (bodyPartPickerPanel.partDeselected(bodyPartIDnumber));
//			print ("deselected " + bodyPartIDnumber);
			turnOffSelectedColor ();
		} else {
//			print ("test onSelect");
//			print("body id"+bodyPartIDnumber);
			StartCoroutine (bodyPartPickerPanel.partSelected (bodyPartIDnumber, pickerListNumber));
//			print ("selected " + bodyPartIDnumber +" "+pickerListNumber );
			turnOnActiveGreen ();
		}
	}
//	public override void OnSelect(BaseEventData eventData){
//		if (bodyPartPickerPanel.getPartSelected () == bodyPartIDnumber) { //toggles off the the body part color on deselection if the parent panel is storing the same number
//			bodyPartPickerPanel.partSelected(0);
//			turnOffSelectedColor ();
//		} else {
//			print ("test onSelect");
//			StartCoroutine (bodyPartPickerPanel.partSelected (bodyPartIDnumber));
//			ColorBlock cb = gameObject.GetComponent<bodyPartPickerButtonScript> ().colors;
//			cb.normalColor = Color.green;
//			gameObject.GetComponent<bodyPartPickerButtonScript> ().colors = cb;
//		}
//	}
	public override void OnDeselect(BaseEventData eventData){
		if (bodyPartPickerPanel.getPartSelectedIDnum () != bodyPartIDnumber) {	//only turns off the the body part color on deselection if the parent panel is storing a different number
			turnOffSelectedColor ();
		}
//		}
	}
	public int getBodyPartIDnum(){
		return bodyPartIDnumber;
	}
	public int getPickerListNumber(){
		return pickerListNumber;
	}
	public void turnOffSelectedColor(){
//		print ("Deseltected");
		ColorBlock cb = gameObject.GetComponent<bodyPartPickerButtonScript>().colors;
		cb.normalColor = startNormalColor;
		gameObject.GetComponent<bodyPartPickerButtonScript>().colors = cb;
	}
	public void turnOnActiveGreen(){
		ColorBlock cb = gameObject.GetComponent<bodyPartPickerButtonScript> ().colors;
		cb.normalColor = Color.green;
		gameObject.GetComponent<bodyPartPickerButtonScript> ().colors = cb;
	}

	public bool completedStartQuery(){
		return completedStart;
	}
//	private SelectionBaseAttribute selectorScript;

//	public void chooseButton(int choice){
//
//		//gameObject.GetComponent<BodyPartSelectionCanvasScript>().markSelectedBodyPart()
//	}
//	public void onClick(){
//		print ("clicked");
//	}
}
