using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransferVariablesScript : MonoBehaviour {
	AllPickedBodyParts allPickedBodyParts;
//	List<int> selectedModules;
	void Start(){
		allPickedBodyParts = new AllPickedBodyParts ();
		GameObject loaderScriptTemp = GameObject.FindWithTag("MainLoader");		//grabbing the object with the body part info taken from xml data	
		if (loaderScriptTemp != null) {
			DontDestroyOnLoad (gameObject);
		}
	}
	public void setPartsPicked(AllPickedBodyParts incomingParts){
//		print ("set parts in scene transfer");
		allPickedBodyParts = incomingParts;
	}
//	public void setModulesPicked(List<int> incomingSelectedModules){
//		selectedModules = incomingSelectedModules;
//	}
	public AllPickedBodyParts getAllPartsPicked(){
		return allPickedBodyParts;
	}
//	public List<int> getAllModules(){
//		return selectedModules;
//	}
	public void bleh(){
		print ("bleh");
	}
}
public class AllPickedBodyParts{
	public BodyPartDataHolder pickedHead;
	public BodyPartDataHolder pickedLeftArm;
	public BodyPartDataHolder pickedRightArm;
	public BodyPartDataHolder pickedTorso;
	public BodyPartDataHolder pickedLeftShoulder;
	public BodyPartDataHolder pickedRightShoulder;
	public BodyPartDataHolder pickedLeg;
	public void setAllPickedBodyParts(BodyPartDataHolder head, BodyPartDataHolder leftarm, BodyPartDataHolder rightarm, BodyPartDataHolder torso, BodyPartDataHolder leftshoulder, BodyPartDataHolder rightshoulder, BodyPartDataHolder leg){
		pickedHead = head;
		pickedLeftArm = leftarm;
//		Debug.Log ("leftarm " + pickedLeftArm.moduleIDnum [0]);
		pickedRightArm = rightarm;
//		Debug.Log ("rightarm " + pickedRightArm.moduleIDnum [0]);
		pickedTorso = torso;
		pickedLeftShoulder = leftshoulder;
		pickedRightShoulder = rightshoulder;
		pickedLeg = leg;
	}
}
//public class TransferBodyPartInfo{
//	public string typeOfPart;
//	public string nameOfPart;
//	public int partIDnum;
//	public int[] moduleIDnum = new int[3];
//	bool placeHolder;
//	public void setAllAtributesOfBPart(string incomingNameOfPart, int incomingPartIDnum, int[] incomingModuleIDnum){
//		int tempInt = 0;
//		foreach (int partID in moduleIDnum) {
//			moduleIDnum [tempInt] = incomingModuleIDnum [tempInt];
//			tempInt++;
//		}
//		partIDnum = incomingPartIDnum;
//		nameOfPart = incomingNameOfPart;
//	}
//	public void setAsPlaceHolder(bool incomingBool){
//		placeHolder = incomingBool;
//	}
//	public bool getIfPlaceHolder(){
//		return placeHolder;
//	}
//}

