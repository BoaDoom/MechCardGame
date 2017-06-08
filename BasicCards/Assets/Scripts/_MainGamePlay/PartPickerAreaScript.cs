using System.Collections;
using System.Collections.Generic;
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using UnityEngine;

public class PartPickerAreaScript: MonoBehaviour {

	public List<XMLBodyHitData> bodyLoaderData;

	public TargetSquareScript smallSquare; //added manually inside unity from prefabs
	Transform transformOriginal;
	public BodyPartSelectionCanvasScript bodyPartSelectionCanvasScript; //added manually inside unity

	public int boxCountX;
	public int boxCountY;

	public float sizeRatioOfSmallBox = 1.0f;

	private TargetSquareScript[][] grid;
	private Vector2 gridDimensions;
	//private TargetSquareState[][] gridOfStates;		//tracks the states of the squares in the targeting box. Boolean of Occupied, HardTargeted, SoftTargeted

	Vector3 zeroCord = Vector3.zero;
	Vector3 framingBoxSize;
	public Vector3 firstBoxCord;

	WholeBodyOfParts  wholeBodyOfParts;
	BodyPartMakerScript BpartMaker;

//	private CurrentWeaponHitBox currentClickedOnCardWeaponMatrix;
//	private CurrentWeaponHitBox storedWeaponHitBox;
//	private bool isCardClickedOn;
//	private BPartGenericScript[] storedHitBodyParts;
	private BPartGenericScript firstBodyPartSentIn;
//	BodyPartPickerPanel partPickerPanel;
	ModulePickerScript[] modulePanels; 
//	bool completedStartup = false;

//	private WholeBodyOfParts wholeBodyOfParts;

	public IEnumerator ManualStart () {
//		print ("picker area script start");
		wholeBodyOfParts = new WholeBodyOfParts ();
		GameObject bodyPartSelectionCanvasScriptTemp =  GameObject.FindWithTag("PartSelectionCanvas");
		if (bodyPartSelectionCanvasScriptTemp != null) {
			bodyPartSelectionCanvasScript = bodyPartSelectionCanvasScriptTemp.GetComponent<BodyPartSelectionCanvasScript>();
		} else { 
			print ("cant find main UI canvas"); 
		}
//		gameObject.GetComponentInParent<BodyPartPickerPanel> ();
		//		print ("This is when the preview window starts");
		modulePanels = new ModulePickerScript[3];		//3 is the max amount of modules a part can have
//		currentClickedOnCardWeaponMatrix = new CurrentWeaponHitBox (null, 0);
//		isCardClickedOn = false;
//		storedHitBodyParts = new BPartGenericScript[8]; 		//the max amount of body parts that could be stored
		
		gridDimensions = new Vector2(boxCountX, boxCountY);


		BpartMaker = gameObject.GetComponentInParent<BodyPartMakerScript>();
		if (BpartMaker == null) {
			Debug.Log ("couldn't find bodypartmaker on this object");
		}

		TargetSquareScript smallSquareInst;
		transformOriginal = gameObject.transform;
		framingBoxSize = new Vector3(1.0f/boxCountX, 1.0f/boxCountY, 0.0f);
		firstBoxCord = zeroCord + new Vector3 ((-0.5f + framingBoxSize.x / 2), (-0.5f + framingBoxSize.y / 2), 0.0f);
		grid = new TargetSquareScript[(int)gridDimensions.x][];		//grid of prefab ActiveSquare
//		Vector2 offSetToCenter = new Vector2(Mathf.Round(boxCountX/2)-Mathf.Round(bodyHitBoxWidth/2),0);

		//Debug.Log (offSetToCenter);
		for (int x = 0; x < gridDimensions.x; x++){
			//gridOfStates [x] = new TargetSquareState[(int)gridDimensions.y];
			grid[x] = new TargetSquareScript[(int)gridDimensions.y];
			for (int y = 0; y < gridDimensions.y; y++)
			{
				smallSquareInst = Instantiate (smallSquare, zeroCord, transformOriginal.rotation);
				StartCoroutine( smallSquareInst.ManualStart (gameObject.GetComponent<PartPickerAreaScript>()));
				smallSquareInst.transform.SetParent (gameObject.transform);
				smallSquareInst.transform.localScale = framingBoxSize * sizeRatioOfSmallBox;
				smallSquareInst.transform.localPosition = firstBoxCord + new Vector3(framingBoxSize.x*x, framingBoxSize.y*y, 0.0f);
				smallSquareInst.SetGridCordX (x);
				smallSquareInst.SetGridCordY (y);
				grid[x][y] = smallSquareInst;
				//gridOfStates[x][y] = smallSquareInst.activeSquareState;

			}
		}
		yield return null;
	}
	public IEnumerator populateCorrectPlayAreaSquares(){		//turns the correct targetting squares on or off
		//Debug.Log (wholeBodyOfParts.listOfAllParts.Count);
		StartCoroutine(clearPlayAreaSquares());
		//print("grid x length: " +incomingSquareGrid[0].Length + " grid y length: "+incomingSquareGrid.Length);
		for (int i=0; i<wholeBodyOfParts.listOfAllParts.Length; i++){		//for every body part in the list
//			print(wholeBodyOfParts.listOfAllParts.Length);
//			print("Name of part "+wholeBodyOfParts.listOfAllParts [i].getName());
//			print("side of part "+wholeBodyOfParts.listOfAllParts [i].getSide());
			for (int x=0; x<wholeBodyOfParts.listOfAllParts [i].getDimensionsOfPart ().x; x++){				//get the x dimensions and run through the grid of Y
				for (int y=0; y<wholeBodyOfParts.listOfAllParts [i].getDimensionsOfPart ().y; y++){			//get the y dimensions and run through every colloum of parts
					if (wholeBodyOfParts.listOfAllParts [i].getGridPoint(new Vector2(x, y))&& wholeBodyOfParts.listOfAllParts [i].getActive()){				//gets the body part point and asks the grid of bodypartnodes if they are on or off at the internal dimension of the part
						//						print("getGlobalOriginPoint(): "+ wholeBodyOfParts.listOfAllParts [i].getGlobalOriginPoint());
						int outGoingXCord = ((int)wholeBodyOfParts.listOfAllParts [i].getGlobalOriginPoint().x)+x;
						int outGoingYCord = ((int)wholeBodyOfParts.listOfAllParts [i].getGlobalOriginPoint().y)+y;
//						print (outGoingXCord + " " + outGoingYCord);
						grid[outGoingXCord][outGoingYCord].OccupiedSquare(wholeBodyOfParts.listOfAllParts [i]);	//sends a reference to the body part to the square marked as 'occupied'
					}
				}
			}
		}
		yield return null;
	}
	public IEnumerator clearPlayAreaSquares(){		//turns the correct targetting squares on or off
		//Debug.Log (wholeBodyOfParts.listOfAllParts.Count);
		int tempIntx = 0;
		foreach (TargetSquareScript[] gridx in grid) {
			int tempInty = 0;
			foreach (TargetSquareScript gridy in gridx) {
				gridy.DeactivateSquare ();
				//				print (tempIntx+ " "+tempInty);
				tempInty++;
			}
			tempIntx++;
		}
		yield return null;
	}

	public Vector2 getGridDimensions(){
		return gridDimensions;
	}
	public IEnumerator refreshSquares(BPartGenericScript incomingBpart){		//updating the visuals to show the newly selected or unselected bpart
//		print(wholeBodyOfParts.bodyPartCount());
		wholeBodyOfParts.setBodyPart(incomingBpart);
//		foreach (BPartGenericScript bpart in WholeBodyOfParts.listOfAllParts) {
//			
//		}
//		print(wholeBodyOfParts.bodyPartCount());
//		print(incomingBpart.getType()+" "+ incomingBpart.getSide());
//		print ("Check "+(wholeBodyOfParts.bodyPartCheck () ));
//		print ("Check leg "+(incomingBpart.getType () != "Leg"));
//		print("Check side "+(incomingBpart.getSide () != true));
		if (wholeBodyOfParts.bodyPartCheck ()) {		//if all body parts are accounted for. And to skip this step if its the left leg, because it is the only body part that is the same on both sides
//			print("all parts counted");
			if ((incomingBpart.getType () == "Leg") && (incomingBpart.getSide () == true)) {
			}
			else{
				wholeBodyOfParts = BpartMaker.createWholeBody( wholeBodyOfParts, gridDimensions);		//sets all the body parts to the correct lovations
	//			StartCoroutine (clearPlayAreaSquares());
				StartCoroutine( populateCorrectPlayAreaSquares ());
//				print ("populateCorrect play area triggered");
			}
		}
	yield return null;
	}
		



//	public IEnumerator upwardsModuleSelected(int incomingModulePickerIDnumber, int incomingModuleSocketLabel){		//signal coming from the module picker that a certain module was chosen, sending it up to the canvas
//		//		print("incomingModulePickerIDnumber"+incomingModulePickerIDnumber);
//		int tempChosenNumber = modulePanels [incomingModulePickerIDnumber].getCurrentModuleSelectedIDnumber ();
//		StartCoroutine(bodyPartSelectionCanvasScript.upwardsModuleSelected (tempChosenNumber, incomingModuleSocketLabel));
//		yield return null;
//	}
//	public IEnumerator upwardsModuleDeselected(int incomingModulePickerIDnumber, int incomingModuleSocketLabel){		//signal coming from the module picker that a certain module was chosen, sending it up to the canvas
//		//		print("incomingModulePickerIDnumber"+incomingModulePickerIDnumber);
//		int tempChosenNumber = modulePanels [incomingModulePickerIDnumber].getCurrentModuleSelectedIDnumber ();
//		StartCoroutine(bodyPartSelectionCanvasScript.upwardsModuleDeselected (tempChosenNumber, incomingModuleSocketLabel));
//		yield return null;
//	}
//	public IEnumerator upwardsOLDModuleDeselected(int incomingModuleIDnumber, int incomingModuleSocketLabel){		//signal coming from the module picker that a certain module was chosen, sending it up to the canvas
//		StartCoroutine( bodyPartSelectionCanvasScript.upwardsModuleDeselected (incomingModuleIDnumber, incomingModuleSocketLabel));
//		yield return null;
//	}
//
//
//	//	upwardsOldModuleDeselected
	public IEnumerator downwardsModuleSelected(int incomingModuleIDnumber){		//signal coming from above from the canvas script that a module was chosen, sending it down to the module picker
		foreach(ModulePickerScript modulePicker in modulePanels){
			if (modulePicker != null) {
				StartCoroutine(modulePicker.downwardsModuleSelected (incomingModuleIDnumber));
			}
		}
		yield return null;
	}
	public IEnumerator downwardsModuleDeselected(int incomingModuleIDnumber){		//signal coming from above from the canvas script that a module was now freed up to be chosen, sending it down to the module picker
		foreach (ModulePickerScript modulePicker in modulePanels) {
			if (modulePicker != null) {
				StartCoroutine(modulePicker.downwardsModuleDeselected (incomingModuleIDnumber));
			}
		}
		yield return null;
	}

}
//	public void SetOpponentScript(PlayerScript incomingPlayerScript){
//		opponentPlayerController = incomingPlayerScript;
//	}

