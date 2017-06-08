using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPartGenericScript : MonoBehaviour {

	private BodyPartDataHolder bodyPartData;

	//completely static, non dependent variables
	private string bPartType;				//arm,head,legs,shoulder, or torso
	private string bPartName;
	private BodyPartNode[][] nodesOfBP;		//given an open grid, the list of active hitable points by list of vectors
	private Vector2[][] internalGlobalCords;
	private Vector2 anchorPoint;			//the location in which all parts will be located and placed
	private List<ComplexAnchorPoints> listOfComplexAnchorPoints = new List<ComplexAnchorPoints> ();
	private float maxHealth;

	//dependent but static variables
	private Vector2 globalOriginPoint;	//the anchor point location in the game hit area
	private Vector2 dimensions;		//dependent on the farthest location from the source (0,0) of the list of binaryDimensions
	public bool leftSide;		//default is left side
	private int cardLocationNumber;		//location of the card in the list of sprites/xml file, dependent on the component installed
//	private BodyPartDataHolder transferBodyPartInfo;
	public int[] moduleIDnum;
	public ModuleSocketCount moduleSocketCount;
//	private List<ModulePart> modulePartList = new List<ModulePart>();

	//dependent and changable variables
	private float currentHealth;
	private bool active;
	private bool fullyDeactivated;
	private bool stillAlive = true;

//	private bool visualStartupOnly;

	private bool underThreat = false;

	private PlayerScript playerScript;

////////////////obsolete after changing damage from per square to per body part
//	public void takeDamage(float incomingDamage){
//		currentHealth -= incomingDamage;
//		if (currentHealth <= 0) {
//			playerScript.outgoingBrokenPartNodes (internalGlobalCords);
////			playerScript.flagABrokenPart ();
//		}
//	}
////////////////////
	public void takeDamage(CurrentWeaponHitBox incomingWeaponHitData){
//		print (incomingWeaponHitData.weaponDamage);
		currentHealth -= incomingWeaponHitData.weaponDamage;
//		print (incomingWeaponHitData.weaponDamage);
			if (currentHealth <= 0) {
				playerScript.outgoingBrokenPartNodes (internalGlobalCords);
				stillAlive = false;
			}
	}
	/// 
	public void setBPartThreatenedOn(){
		underThreat = true;
		PlayAreaScript playerAreaTemp = playerScript.getPlayAreaOfPlayer ();
		int x = 0;
		foreach(BodyPartNode[] bodyPartCollum in nodesOfBP){
			
			int y = 0;
			foreach(BodyPartNode bodyPartSquare in bodyPartCollum){
				playerAreaTemp.getSmallSquare((int)internalGlobalCords[x][y].x, (int)internalGlobalCords[x][y].y).setBPartUnderThreat (); //sending cords of the bodypart cords
				y++;
			}
		x++;
		}
	}
	public void setBPartThreatenedOff(){
		underThreat = false;
		PlayAreaScript playerAreaTemp = playerScript.getPlayAreaOfPlayer ();
		int x = 0;
		foreach(BodyPartNode[] bodyPartCollum in nodesOfBP){
			x++;
			int y = 0;
			foreach(BodyPartNode bodyPartSquare in bodyPartCollum){
				playerAreaTemp.getSmallSquare (x, y).setBPartNotUnderThreat ();
				y++;
			}
		}
	}
	public bool getIfUnderThreat(){
		return underThreat;
	}

	public void Start(){

		playerScript = gameObject.GetComponentInParent<PlayerScript>();
	}

	public void CreateNewPart(BodyPartDataHolder incomingBodyPartDataFromXML, int directionDesignation){		//used by bodypartmakerscript
		bPartType = incomingBodyPartDataFromXML.typeOfpart;				//arm,head,legs,shoulder, or torso
		bPartName = incomingBodyPartDataFromXML.name;
//		print ("bPartType " + directionDesignation + " bPartName " + bPartName);
//		visualStartupOnly = false;

		//transferBodyPartInfo = incomingTransferBodyPartInfo;

		maxHealth = incomingBodyPartDataFromXML.maxHealth;
		if (directionDesignation == 0) {
			leftSide = true;
		}
		if (directionDesignation == 1) {
			leftSide = true;
		}
		if (directionDesignation == 2) {
			leftSide = false;
		}
		nodesOfBP = new BodyPartNode[incomingBodyPartDataFromXML.bodyPartGrid.Length][];
		if (directionDesignation == 0 || directionDesignation== 1) {
			leftSide = true;		//default is left side
		} else {
			leftSide = false;
		}
		for(int i=0; i < incomingBodyPartDataFromXML.bodyPartGrid.Length; i++){	//transfering the int[][] grid
			int g = incomingBodyPartDataFromXML.bodyPartGrid.Length - i-1;
			if (leftSide) {
				nodesOfBP [i] = new BodyPartNode[incomingBodyPartDataFromXML.bodyPartGrid[0].Length];
			}
			else{									//mirroring the body part for right hand pieces
				nodesOfBP [g] = new BodyPartNode[incomingBodyPartDataFromXML.bodyPartGrid[0].Length];
			}
			//nodesOfBP [i] = new BodyPartNode[incomingBodyPartDataFromXML.bodyPartGrid[0].Length];
			for(int j=0; j < incomingBodyPartDataFromXML.bodyPartGrid[0].Length; j++){
				BodyPartNode bodyPartNode = new BodyPartNode ();
				if (incomingBodyPartDataFromXML.bodyPartGrid [i] [j] == 1) {
					bodyPartNode.turnOn ();
				}
				if (leftSide) {
					nodesOfBP [i] [j] = bodyPartNode;
				}
				else{									//mirroring the body part for right hand pieces
					nodesOfBP [g] [j] = bodyPartNode;
				}
			}
		}
		dimensions = new Vector2(nodesOfBP.Length, nodesOfBP[0].Length);		//dependent on the farthest location from the source (0,0) of the list of binaryDimensions

		if (incomingBodyPartDataFromXML.simpleAnchorPoints) {			//checking to see if there is one anchor point or more
			if (leftSide){										//if left side (default design), then transfer anchor point normally
				
				anchorPoint = new Vector2 (incomingBodyPartDataFromXML.anchor.x, incomingBodyPartDataFromXML.anchor.y);			//the location in which all parts will be located and placed
//				print("anchorPoint "+ anchorPoint);
			}
			else if(!leftSide){								//if right side, mirror the anchor point across the X axis
				anchorPoint = new Vector2 (((dimensions.x) - (incomingBodyPartDataFromXML.anchor.x+1)), incomingBodyPartDataFromXML.anchor.y);
			}
		} 
		else {
			if (leftSide) {										//if left side (default design), then transfer anchor point normally
				listOfComplexAnchorPoints = incomingBodyPartDataFromXML.listOfComplexAnchorPoints;			//the location in which all parts will be located and placed
			} 
			else if (!leftSide) {								//if right side, mirror the anchor point across the X axis
				int tempCount = incomingBodyPartDataFromXML.listOfComplexAnchorPoints.Count;
				for (int i = 0; i < tempCount; i++) {
//					print ("complex anchor name "+incomingBodyPartDataFromXML.listOfComplexAnchorPoints[i].nameOfPoint);
//						print("1 "+incomingBodyPartDataFromXML.listOfComplexAnchorPoints[i].nameOfPoint);
//						print("2 "+new Vector2((dimensions.x-1) - (incomingBodyPartDataFromXML.listOfComplexAnchorPoints[i].anchorPoint.x), incomingBodyPartDataFromXML.listOfComplexAnchorPoints[i].anchorPoint.y));
//						print("3 "+incomingBodyPartDataFromXML.listOfComplexAnchorPoints[i].male);
						listOfComplexAnchorPoints.Add (new ComplexAnchorPoints(
							incomingBodyPartDataFromXML.listOfComplexAnchorPoints[i].nameOfPoint,
							new Vector2((dimensions.x-1) - (incomingBodyPartDataFromXML.listOfComplexAnchorPoints[i].anchorPoint.x), incomingBodyPartDataFromXML.listOfComplexAnchorPoints[i].anchorPoint.y),
							incomingBodyPartDataFromXML.listOfComplexAnchorPoints[i].male));
//					print (new ComplexAnchorPoints (
//						incomingBodyPartDataFromXML.listOfComplexAnchorPoints [i].nameOfPoint,
//						new Vector2 ((dimensions.x - 1) - (incomingBodyPartDataFromXML.listOfComplexAnchorPoints [i].anchorPoint.x), incomingBodyPartDataFromXML.listOfComplexAnchorPoints [i].anchorPoint.y),
//						incomingBodyPartDataFromXML.listOfComplexAnchorPoints [i].male));
//					the dimension.x+1 is to account for the origin of the points being at 1,1 rather than 0,0.
				}
			}
		}
//		print ("incomoing bodypart datata moduleidnum length "+incomingBodyPartDataFromXML.moduleIDnum.Length);
		moduleIDnum = new int[incomingBodyPartDataFromXML.moduleIDnum.Length];
		int tempint = 0;
		foreach (int incomingModuleIDnumber in incomingBodyPartDataFromXML.moduleIDnum) {
			moduleIDnum [tempint] = incomingModuleIDnumber;
//			print("module creation in Bpartgeneric "+ incomingModuleIDnumber);
			tempint++;
		}
//		moduleIDnum = incomingBodyPartDataFromXML.moduleIDnum;
		moduleSocketCount = incomingBodyPartDataFromXML.moduleSocketCount;
		currentHealth = incomingBodyPartDataFromXML.maxHealth;
		resetHealthToFull();
		active = true;
		fullyDeactivated = false;

	}

	public ModuleSocketCount getModuleSocketCount(){
		return moduleSocketCount;
	}

	public void resetHealthToFull(){
		currentHealth = maxHealth;
		stillAlive = true;
	}
	public void setInternalGlobalCords(){			//sets the specific global locations for each node
//		print("dimensions of body part "+getDimensionsOfPart ());
		internalGlobalCords = new Vector2[(int)getDimensionsOfPart ().x][];
//		print ("getDimensionsOfPart () " +getName()+" " + getDimensionsOfPart ());
		for (int x = 0; x < getDimensionsOfPart ().x; x++) {				
			internalGlobalCords[x] = new Vector2[(int)getDimensionsOfPart ().y];
			for (int y = 0; y < getDimensionsOfPart ().y; y++) {			
				int outGoingXCord = ((int)getGlobalOriginPoint ().x) + x;
				int outGoingYCord = ((int)getGlobalOriginPoint ().y) + y;
//				print ("setting internal cords x: " + outGoingXCord + " y: " + outGoingYCord);
//				print ("at x: " + x + " y: " + y);
				internalGlobalCords [x] [y] = new Vector2 (outGoingXCord, outGoingYCord);
//				print (getName()+  " internal global cords: "+internalGlobalCords [x] [y]);
			}
		}
	}
	public Vector2 getInternalGlobalCord(Vector2 incomingInternalVector){
		if (incomingInternalVector.x < 0 || incomingInternalVector.y < 0 || incomingInternalVector.x >= internalGlobalCords.Length || incomingInternalVector.y >= internalGlobalCords [0].Length) {	//testing to see if the incoming request is within the internal dimensions of the body part
			Debug.Log ("requested dimensions are outside of the internal stored dimensions of the body part");
			return Vector2.zero;
		} else {
			
			return internalGlobalCords [(int)incomingInternalVector.x] [(int)incomingInternalVector.y];
		}
	}



	public void setTorsoOriginPosition(Vector2 incomingTorsoOriginPoint){
		//Debug.Log ("setting custom torso origin");
//		print(listOfComplexAnchorPoints.Count);
//		print(listOfComplexAnchorPoints.Find (ComplexAnchorPoints => ComplexAnchorPoints.nameOfPoint == "LeftLegPoint"));
		globalOriginPoint = new Vector2 (incomingTorsoOriginPoint.x, incomingTorsoOriginPoint.y - (listOfComplexAnchorPoints.Find (ComplexAnchorPoints => ComplexAnchorPoints.nameOfPoint == "LeftLegPoint").anchorPoint.y));
		setInternalGlobalCords ();
		//print (getInternalGlobalCord(new Vector2(0.0f, 0.0f)));
	}

	public void setGlobalPosition(Vector2 incomingGlobalAnchorPoint){
		globalOriginPoint = incomingGlobalAnchorPoint - anchorPoint;
		setInternalGlobalCords ();

	}
	public Vector2 getGlobalOriginPoint(){
		return globalOriginPoint;
	}

	public void setGlobalPositionOffComplexAnchor(Vector2 incomingGlobalAnchorPoint, string pointOfConnection){
		globalOriginPoint = incomingGlobalAnchorPoint - (listOfComplexAnchorPoints.Find (ComplexAnchorPoints => ComplexAnchorPoints.nameOfPoint == pointOfConnection).anchorPoint);
//		Debug.Log ("ouput point: " +globalOriginPoint);
		setInternalGlobalCords ();
		//print (getInternalGlobalCord(new Vector2(0.0f, 0.0f)));
	}
	public Vector2 getGlobalAnchorPoint(string requestedAnchorPointName){
//		Debug.Log ("globalOriginpoint: "+globalOriginPoint+" requested anchor point:"+ getComplexAnchorPoint(requestedAnchorPointName).anchorPoint);
		return (globalOriginPoint + getComplexAnchorPoint(requestedAnchorPointName).anchorPoint);
	}

	public bool destroyedCheck(){		//a check to see if the part still exists or is destroyed
		return !stillAlive;
	}
	public string getName(){
		return bPartName;
	}
	public string getType(){
		return bPartType;
	}
	public float getCurrentHealth(){
		if (currentHealth < 0) {
			return 0;
		} else {
			return currentHealth;
		}
	}

	public Vector2 getDimensionsOfPart(){
		return dimensions;
	}
	public Vector2 getAnchorPoint(){
//		print ("anchorPoint for " +getName() +" " + anchorPoint);
		return anchorPoint;
	}
	public ComplexAnchorPoints getComplexAnchorPoint(string incomingRequest){
		return listOfComplexAnchorPoints.Find (ComplexAnchorPoints => ComplexAnchorPoints.nameOfPoint == incomingRequest);
	}
	public List<ComplexAnchorPoints> getComplexAllAnchorPoints(){
		return listOfComplexAnchorPoints;
	}
		
	public bool getSide(){
		return leftSide;
	}
	public bool getActive(){
		return active;
	}
	public void setAsActive(){
		active = true;
	}
	public void setAsInactive(){
		active = false;
	}

	public bool getFullyDeactivated(){
		return fullyDeactivated;
	}
	public void setFullyDeactivated(){
		fullyDeactivated  = true;
	}
	public bool getGridPoint(Vector2 incomingPoint){
		return nodesOfBP [(int)incomingPoint.x] [(int)incomingPoint.y].getState ();
	}


	public int[] getModules(){
		return moduleIDnum;
	}
	public void destroyCompletely(){
		Destroy (gameObject);
	}
}

public class BodyPartNode{
	private bool exists;
	public BodyPartNode(){
		exists = false;
	}
	public bool getState(){
		return exists;
	}


	public void turnOn(){
		exists = true;
	}
	public void turnOff(){
		exists = false;
	}
}