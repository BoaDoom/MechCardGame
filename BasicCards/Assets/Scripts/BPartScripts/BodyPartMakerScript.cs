using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartMakerScript : MonoBehaviour {
//	BPartXMLReaderScript bPartXMLReader;
	public BPartGenericScript bodyPartObject;
	BodyPartDataHolder partData = null;
	public Transform placeHolder;
	public bool startupDone = false;

	public IEnumerator ManualStart(){
		GameObject loaderScriptTemp = GameObject.FindWithTag("MainLoader");	
		GameObject LoaderMainTemp = GameObject.FindWithTag ("MainLoader");
//		if (LoaderMainTemp != null) {
//			bPartXMLReader = LoaderMainTemp.GetComponent<BPartXMLReaderScript> ();
//			//Debug.Log ("GOT Bpart XML");
//			//startupDone = true;
//		}
		if (LoaderMainTemp == null && loaderScriptTemp != null) {
			Debug.Log ("Cannot find 'MainLoader'object");
		}
		//bPartXMLReader = gameObject.GetComponent<BPartXMLReaderScript>();
		//bodyPartObject = gameObject.GetComponent<>();
//		GameObject EnemyScriptTemp = GameObject.FindWithTag ("EnemyController");
//		if (LoaderMainTemp != null) {
//			EnemyScriptTemp.GetComponent<EnemyScript> ().signalThatBodyPartIsDone();
//		} else if (LoaderMainTemp != null) {
//			Debug.Log ("could not find enemy script");
//		}
		yield return null;
	}

	public BPartGenericScript makeBodyPart(BodyPartDataHolder incomingBodyPartInfo, int leftOrRight){		//used by playerscript
//		Debug.Log ("check: " + nameOfpart + " " + leftOrRight);
		//Debug.Log("name: "+ nameOfpart); 
		//Debug.Log("leftor right: "+ leftOrRight);
		//BodyPartDataHolder partData = new BodyPartDataHolder();
		partData = incomingBodyPartInfo; //bPartXMLReader.getBodyData (incomingBodyPartInfo.name);
//		print("Bpmaker data transfer "+partData.moduleIDnum[0]);
		BPartGenericScript instaBodypart = Instantiate (bodyPartObject, Vector3.zero, bodyPartObject.GetComponent<Transform>().rotation);
		//Debug.Log ("body data check: "+bPartXMLReader.getBodyData (nameOfpart).name);
		instaBodypart.CreateNewPart (partData, leftOrRight);		//formatting is partData and 'Left' or 'Right'
		//Debug.Log ("instantiated after: "+instaBodypart.getName());
//		print("Bpmaker bpart "+instaBodypart.moduleIDnum[0]);

		return instaBodypart;
	}
	public WholeBodyOfParts createWholeBody(WholeBodyOfParts incomingWholeBodyOfParts, Vector2 incomingDimensionsOfPlayArea){
//		for (int i=0; i < 5; i++) {
////			Debug.Log (incomingWholeBodyOfParts.leftArm.getName());
////			Debug.Log (incomingWholeBodyOfParts.rightArm.getName());
////			Debug.Log (incomingWholeBodyOfParts.torso.getName());
//		}
		if (incomingWholeBodyOfParts.bodyPartCheck ()) {
//			print ("body part check: " + incomingWholeBodyOfParts.bodyPartCheck ());
			//(int)Mathf.Round(
			int halfwayLengthOfBoard = (int)Mathf.Round((incomingDimensionsOfPlayArea.x/2) -incomingWholeBodyOfParts.torso.getDimensionsOfPart().x/2);
			//float halfwayLengthOfTorso = ;
			int heightOfLegAnchorPoint = (int)incomingWholeBodyOfParts.leftLeg.getAnchorPoint ().y;
						//the far left point that the torso needs to be center, it's origin x point
//			print("halfwayLengthOfBoard: "+halfwayLengthOfBoard +" halfwayLengthOfTorso: " +halfwayLengthOfTorso+" heightOfLegAnchorPoint: "+heightOfLegAnchorPoint);
			Vector2 offSetToCenter = new Vector2((halfwayLengthOfBoard),heightOfLegAnchorPoint);

			incomingWholeBodyOfParts.torso.setTorsoOriginPosition (offSetToCenter);
//			print ("offSetToCenter "+offSetToCenter);
//			incomingWholeBodyOfParts.torso.setInternalGlobalCords ();
//			print (incomingWholeBodyOfParts.torso.getInternalGlobalCord(new Vector2(0.0f, 0.0f)));
//			print("incomingWholeBodyOfParts.torso.getGlobalAnchorPoint (LeftLegPoint)"+ incomingWholeBodyOfParts.torso.getGlobalAnchorPoint ("LeftLegPoint"));
			incomingWholeBodyOfParts.leftLeg.setGlobalPosition (incomingWholeBodyOfParts.torso.getGlobalAnchorPoint ("LeftLegPoint"));
//			incomingWholeBodyOfParts.leftLeg.setInternalGlobalCords ();
//			print (incomingWholeBodyOfParts.leftLeg.getInternalGlobalCord(new Vector2(0.0f, 0.0f)));

			incomingWholeBodyOfParts.rightLeg.setGlobalPosition (incomingWholeBodyOfParts.torso.getGlobalAnchorPoint ("RightLegPoint"));
//			incomingWholeBodyOfParts.rightLeg.setInternalGlobalCords ();
//			print (incomingWholeBodyOfParts.rightLeg.getInternalGlobalCord(new Vector2(0.0f, 0.0f)));

			incomingWholeBodyOfParts.head.setGlobalPosition (incomingWholeBodyOfParts.torso.getGlobalAnchorPoint ("HeadPoint"));
//			incomingWholeBodyOfParts.head.setInternalGlobalCords ();
//			print (incomingWholeBodyOfParts.head.getInternalGlobalCord(new Vector2(0.0f, 0.0f)));

			incomingWholeBodyOfParts.leftShoulder.setGlobalPositionOffComplexAnchor (incomingWholeBodyOfParts.torso.getGlobalAnchorPoint("LeftShoulderPoint"), "TorsoPoint");
//			incomingWholeBodyOfParts.leftShoulder.setInternalGlobalCords ();
//			print (incomingWholeBodyOfParts.leftShoulder.getInternalGlobalCord(new Vector2(0.0f, 0.0f)));

			incomingWholeBodyOfParts.rightShoulder.setGlobalPositionOffComplexAnchor (incomingWholeBodyOfParts.torso.getGlobalAnchorPoint("RightShoulderPoint"), "TorsoPoint");
//			incomingWholeBodyOfParts.rightShoulder.setInternalGlobalCords ();
//			print (incomingWholeBodyOfParts.rightShoulder.getInternalGlobalCord(new Vector2(0.0f, 0.0f)));

			incomingWholeBodyOfParts.leftArm.setGlobalPosition (incomingWholeBodyOfParts.leftShoulder.getGlobalAnchorPoint ("ArmPoint"));
//			incomingWholeBodyOfParts.leftArm.setInternalGlobalCords ();
//			print (incomingWholeBodyOfParts.leftArm.getInternalGlobalCord(new Vector2(0.0f, 0.0f)));

			incomingWholeBodyOfParts.rightArm.setGlobalPosition (incomingWholeBodyOfParts.rightShoulder.getGlobalAnchorPoint ("ArmPoint"));
//			incomingWholeBodyOfParts.rightArm.setInternalGlobalCords ();
//			print (incomingWholeBodyOfParts.rightArm.getInternalGlobalCord(new Vector2(0.0f, 0.0f)));


		} else {
			Debug.Log ("There are body parts missing");
		}
		return incomingWholeBodyOfParts;
	}
}
public class WholeBodyOfParts{
	public BPartGenericScript leftArm;
	public BPartGenericScript rightArm;
	public BPartGenericScript head;
	public BPartGenericScript leftLeg;
	public BPartGenericScript rightLeg;
	public BPartGenericScript leftShoulder;
	public BPartGenericScript rightShoulder;
	public BPartGenericScript torso;

	public BPartGenericScript defaultLeftArm;
	public BPartGenericScript defaultRightArm;
	public BPartGenericScript defaultHead;
	public BPartGenericScript defaultLeftLeg;
	public BPartGenericScript defaultRightLeg;
	public BPartGenericScript defaultLeftShoulder;
	public BPartGenericScript defaultRightShoulder;
	public BPartGenericScript defaultTorso;
	public BPartGenericScript[] listOfAllParts = new BPartGenericScript[8];
	public bool hasAnyBodyParts;

	public void setBodyPart(BPartGenericScript incomingBodyPart){
		//Debug.Log (incomingBodyPart.getType());
		hasAnyBodyParts = true;
//		listOfAllParts.Add(incomingBodyPart);
		switch (incomingBodyPart.getType()) {
		case ("Arm"):
			if (incomingBodyPart.getSide ()) {
				leftArm = incomingBodyPart;
				listOfAllParts [0] = incomingBodyPart;
				if (defaultLeftArm == null) {
					defaultLeftArm = incomingBodyPart;
				}
				//Debug.Log("left arm import check: " +leftArm.getName ());
				break;
			} else {
				rightArm = incomingBodyPart;
				listOfAllParts [1] = incomingBodyPart;
				if (defaultRightArm == null) {
					defaultRightArm = incomingBodyPart;
				}
				//Debug.Log("left arm import check: " +leftArm.getName ());
				break;
			}
		case ("Head"):
			head = incomingBodyPart;
			listOfAllParts [2] = incomingBodyPart;
			if (defaultHead == null) {
				defaultHead = incomingBodyPart;
			}
			//Debug.Log("left arm import check: " +leftArm.getName ());
			break;
		case ("Leg"):
			if (incomingBodyPart.getSide ()) {
				leftLeg = incomingBodyPart;
				listOfAllParts [3] = incomingBodyPart;
				if (defaultLeftLeg == null) {
					defaultLeftLeg = incomingBodyPart;
				}
				break;
			} else {
				rightLeg = incomingBodyPart;
				listOfAllParts [4] = incomingBodyPart;
				if (defaultRightLeg == null) {
					defaultRightLeg = incomingBodyPart;
				}
				break;
			}
		case ("Shoulder"):
			if (incomingBodyPart.getSide ()) {
				leftShoulder = incomingBodyPart;
				listOfAllParts [5] = incomingBodyPart;
				if (defaultLeftShoulder == null) {
					defaultLeftShoulder = incomingBodyPart;
				}
				break;
			} else {
				rightShoulder = incomingBodyPart;
				listOfAllParts [6] = incomingBodyPart;
				if (defaultRightShoulder == null) {
					defaultRightShoulder = incomingBodyPart;
				}
				break;
			}
		case ("Torso"):
			torso = incomingBodyPart;
			listOfAllParts [7] = incomingBodyPart;
			if (defaultTorso == null) {
				defaultTorso = incomingBodyPart;
			}
			break;
		}
	}
	public BPartGenericScript getBodyPart(string nameOfPart){
		switch (nameOfPart) {
		case "leftArm":
			return leftArm;
		case "rightArm":
			return rightArm;
		case "head":
			return head;
		case "leftLeg":
			return leftLeg;
		case "rightLeg":
			return rightLeg;
		case "leftShoulder":
			return leftShoulder;
		case "rightShoulder":
			return rightShoulder;
		case "torso":
			return torso;
		}
		return null;
	}
	public int bodyPartCount(){
		int count = 0;
		foreach(BPartGenericScript part in listOfAllParts){
			count++;
		}
		return count;
	}
	public bool bodyPartCheck(){
		if (leftArm != null && rightArm != null && head != null && leftLeg != null && rightLeg != null && leftShoulder != null && rightShoulder != null && torso != null) {
			return true;
		} else {
			return false;
		}
	}
	public bool hasBodyPart(){
		return hasAnyBodyParts;
	}
	public void resetBodyToZero(){
		 leftArm = null;
		rightArm = null;
		head = null;
		leftLeg = null;
		rightLeg = null;
		leftShoulder = null;
		rightShoulder = null;
		torso = null;
//		listOfAllParts.Clear();
		for(int i =0; i < 7; i++){		///magical 7 is the count of body parts
			listOfAllParts[i] = null;
		}
	}
}
public class ModulePart{
	public int[] cardsOfModule;
	public ModulePart(int[] incomingPartInternalLocation){
		int tempInt = 0;
		foreach (int locationNumber in cardsOfModule) {
			cardsOfModule [tempInt] = locationNumber;
			tempInt++;
		}
	}


}
