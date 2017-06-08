using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
	private BodyPartMakerScript BpartMaker;

	//public BPartGenericScript bodyPartObject;
	//	public PlayAreaScript playAreaScript;

	float healthMax = 0;
	float remainingHealth;
	public Text enemyHealthDisplayNumber;

	public Transform healthBarGraphic;
	private Vector3 healthBarStartingScale;
	private Vector3 healthBarStartingPosition;
	private bool isEnemyPlayer;

	public Transform playerTickerTimer;
	private Vector3 playerTickerStartTransformScale;
	private Vector3 playerTickerStartTransformPosition;
//	private float defaultTickerTime = 5.0f;
//	private float tickerTimer;


	private WholeBodyOfParts wholeBodyOfParts = new WholeBodyOfParts();
	private CurrentWeaponHitBox incomingWeaponhitBox;

	private Vector2 playAreaDimensions;
	private int flagForBrokenParts;
	private PlayAreaScript playAreaScript;
	private DeckScript activeDeck;
	private GameControllerScript gameController;

	private PlayerScript opponentPlayerController;
	private CurrentWeaponHitBox storedWeaponHitBoxForDelayedHit;
	//private BPartGenericScript[] listOfBPartsUnderThreatForDelayedHit;

	private AllPickedBodyParts allPickedBodyParts;
	private bool bodyPartsPopulated = false;
	//	bool bodypartIsDone = false;
	//	bool playAreaIsDone = false;
	//float zeroTime;


	public IEnumerator ManualStart (AllPickedBodyParts incomingBodyPartPicks) {
//		print (incomingBodyPartPicks.pickedHead.moduleIDnum [0]);
		playerTickerStartTransformScale = new Vector3(playerTickerTimer.transform.localScale.x, playerTickerTimer.transform.localScale.y, playerTickerTimer.transform.localScale.y);
		playerTickerStartTransformPosition = new Vector3(playerTickerTimer.transform.localPosition.x,playerTickerTimer.transform.localPosition.y, playerTickerTimer.transform.localPosition.z);
//		tickerTimer = defaultTickerTime;

		allPickedBodyParts = incomingBodyPartPicks;  //choices of body parts picked from previous menu selections
		gameController = gameObject.GetComponentInParent<GameControllerScript> ();
		playAreaScript = gameObject.GetComponentInChildren<PlayAreaScript> ();
		activeDeck = gameObject.GetComponentInChildren<DeckScript> ();
		StartCoroutine( playAreaScript.ManualStart ());
		SetOpponentScript ();		//finds and stores the playerscript of the opponent
		playAreaScript.SetOpponentScript (opponentPlayerController);	//gives the opponent control script to the playarea

		BpartMaker = gameObject.GetComponent<BodyPartMakerScript> ();

		//		remainingHealth = healthMax;
		healthBarStartingScale = healthBarGraphic.localScale;
		healthBarStartingPosition = healthBarGraphic.localPosition;
		//		updateHealthDisplay ();

		StartCoroutine(BpartMaker.ManualStart());
		populateBody ();
		playAreaScript.populateEnemyPlayAreaSquares ();

		if (gameObject.tag == "PlayerController") {
			isEnemyPlayer = false;
		} else {
			isEnemyPlayer = true;
		}

//		StartCoroutine(startTicker());
		yield return null;
	}

//	public IEnumerator setTimer(float timerLength){
////		print ("Timer start "+ timerLength);
//		//float timerCounter = 0f;
//		float endTime = gameController.getTime () + timerLength;
//		while (gameController.getTime () < endTime) {
//			//print ("Timer going " +(endTime -gameController.getTime()));
//			yield return null;
//		}
////		print ("Timer done" +timerLength);
//		yield return null;
//	}
	public IEnumerator startTicker(float incomingTime, BPartGenericScript[] incomingListOfBPartsUnderThreat){	//this is stored to use on the opponents controller after players timer has counted down
		//storedWeaponHitBoxForDelayedHit = incomingWeaponHitBox;

		//print("stored weapon damage "+opponentPlayerController.storedWeaponHitBoxForDelayedHit.weaponDamage);
		//listOfBPartsUnderThreatForDelayedHit = incomingListOfBPartsUnderThreat;
//		print ("start");
		float startingIncomingTime = incomingTime;
		int enemy;
		if (isEnemyPlayer) {
			enemy = -1;
		} else {
			enemy = 1;
		}
		while (incomingTime > 0){
//			print ("loop");
			incomingTime = incomingTime - Time.deltaTime;
			playerTickerTimer.localScale = new Vector3((playerTickerStartTransformScale.x*(incomingTime / startingIncomingTime)), playerTickerStartTransformScale.y, playerTickerStartTransformScale.z);
			playerTickerTimer.localPosition = new Vector3 ((playerTickerStartTransformPosition.x - ((playerTickerStartTransformScale.x/2)-((playerTickerStartTransformScale.x/2)*(incomingTime / startingIncomingTime))) * enemy),
				playerTickerStartTransformPosition.y, playerTickerStartTransformPosition.z);
			yield return null;
		}
		//incomingTime = defaultTickerTime;
		playerTickerTimer.localScale = playerTickerStartTransformScale;
		playerTickerTimer.localPosition = playerTickerStartTransformPosition;
		//gameController.transferOfCardDamage ();
		//print ("One rotation done");
//		print ("end");
		for (int i=0; i<incomingListOfBPartsUnderThreat.Length; i++) {	//deals damage to stored body parts from card. These are the enemies body parts
			if (incomingListOfBPartsUnderThreat[i] != null) {
				incomingListOfBPartsUnderThreat[i].takeDamage (opponentPlayerController.storedWeaponHitBoxForDelayedHit);
			}
		}
		opponentPlayerController.updateHealthDisplay ();
		yield return null;
	}

	public void setPlayAreaDimensions(Vector2 incomingDimensions){
		//print ("inc dim "+incomingDimensions);
		playAreaDimensions = incomingDimensions;
		remainingHealth = healthMax;
	}

	public void ResetHealthBar(){
		healthBarGraphic.localScale = healthBarStartingScale;

	}
	public void updateHealthDisplay(){
		int tempEnemyIntCheck;
		if (isEnemyPlayer) {
			tempEnemyIntCheck = -1;
		} else {
			tempEnemyIntCheck = 1;
		}
		Vector3 tempHealth = healthBarStartingScale;
		Vector3 tempPositionForHealth = healthBarStartingPosition;
		float newHealth = 0;
		//Debug.Log ("Number of body parts: " + wholeBodyOfParts.listOfAllParts.Count);
		for (int i=0; i<wholeBodyOfParts.listOfAllParts.Length; i++){
			float currentHealth = wholeBodyOfParts.listOfAllParts [i].getCurrentHealth ();
			newHealth += wholeBodyOfParts.listOfAllParts [i].getCurrentHealth ();
			//Debug.Log (i+ " health: "+wholeBodyOfParts.listOfAllParts [i].getCurrentHealth ());
			if (currentHealth <= 0 && wholeBodyOfParts.listOfAllParts [i].getActive()) {
				wholeBodyOfParts.listOfAllParts [i].setAsInactive ();
				Debug.Log ("deactivated triggered");
			}
		}
		remainingHealth = newHealth;
		tempHealth.x = healthBarStartingScale.x * (remainingHealth / healthMax);
		tempPositionForHealth.x = (healthBarStartingPosition.x - ((healthBarStartingScale.x - tempHealth.x)/2)*tempEnemyIntCheck);
		healthBarGraphic.localScale = tempHealth;
		healthBarGraphic.localPosition = tempPositionForHealth;
		enemyHealthDisplayNumber.text = remainingHealth.ToString() + "/" + healthMax.ToString();
	}
	public void populateBody(){				//currently by the manualstart of this same script, which is started by the game controller
		healthMax = 0;
		//StartCoroutine (waitForBpartMakerScript ());
		wholeBodyOfParts.resetBodyToZero ();

		//		int rand = Random.Range(1,5);
//		print("Body part picking test: " + allPickedBodyParts.pickedTorso);
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ((allPickedBodyParts.pickedTorso), 0));

		//		rand = Random.Range(1,5);
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ((allPickedBodyParts.pickedLeg), 1));
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ((allPickedBodyParts.pickedLeg), 2));

		//		rand = Random.Range(1,5);				//random body part between one and four
		//print(rand);
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ((allPickedBodyParts.pickedLeftArm), 1));
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ((allPickedBodyParts.pickedRightArm), 2));
		//		rand = Random.Range(1,5);
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ((allPickedBodyParts.pickedHead), 0));

		//		rand = Random.Range(1,5);
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ((allPickedBodyParts.pickedLeftShoulder), 1));
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ((allPickedBodyParts.pickedRightShoulder), 2));

//		print ("allPickedBodyParts.pickedHead "+allPickedBodyParts.pickedHead.moduleIDnum [0]);
//		print ("processed whole body "+wholeBodyOfParts.getBodyPart ("head").moduleIDnum [0]);
		wholeBodyOfParts = BpartMaker.createWholeBody (wholeBodyOfParts, playAreaDimensions);		//setting internal location positions of each of the body parts in relation to eachother
		for (int i=0; i<wholeBodyOfParts.listOfAllParts.Length; i++){
			healthMax += wholeBodyOfParts.listOfAllParts [i].getCurrentHealth ();		//makes health pool
		}
		foreach (BPartGenericScript bPart in wholeBodyOfParts.listOfAllParts) {
			bPart.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
		}
		remainingHealth = healthMax;
		updateHealthDisplay ();
		bodyPartsPopulated = true;
	}
	public void outgoingBrokenPartNodes(Vector2[][] incomingSet){		//new replacement for turning off the targeting nodes when a bodypart is destroyed
		foreach(Vector2[] partCordsRow in incomingSet){
			foreach (Vector2 cord in partCordsRow) {
				playAreaScript.getSmallSquare ((int)cord.x, (int)cord.y).DeactivateSquare ();
//				print ("square deactivated");
				playAreaScript.hardResetSmallSquares ();		//in place to refresh the visability of all the small squares
			}
		}
		updateHealthDisplay ();
		activeDeck.updateCards ();		//in place to refresh any cards that have been disabled because of their body parts being destroyed
	}
	public PlayAreaScript getPlayAreaOfPlayer(){
		return playAreaScript;
	}

	public WholeBodyOfParts getWholeBodyOfParts(){
		return wholeBodyOfParts;
	}


	public TargetSquareScript[][] populateCorrectPlayAreaSquares(TargetSquareScript[][] incomingSquareGrid){
		//Debug.Log (wholeBodyOfParts.listOfAllParts.Count);
		//print("grid x length: " +incomingSquareGrid[0].Length + " grid y length: "+incomingSquareGrid.Length);
		for (int i=0; i<wholeBodyOfParts.listOfAllParts.Length; i++){		//for every body part in the list
			for (int x=0; x<wholeBodyOfParts.listOfAllParts [i].getDimensionsOfPart ().x; x++){				//get the x dimensions and run through the grid of Y
				for (int y=0; y<wholeBodyOfParts.listOfAllParts [i].getDimensionsOfPart ().y; y++){			//get the y dimensions and run through every colloum of parts
					if (wholeBodyOfParts.listOfAllParts [i].getGridPoint(new Vector2(x, y))&& wholeBodyOfParts.listOfAllParts [i].getActive()){				//gets the body part point and asks the grid of bodypartnodes if they are on or off at the internal dimension of the part
						//						print("getGlobalOriginPoint(): "+ wholeBodyOfParts.listOfAllParts [i].getGlobalOriginPoint());
						int outGoingXCord = ((int)wholeBodyOfParts.listOfAllParts [i].getGlobalOriginPoint().x)+x;
						int outGoingYCord = ((int)wholeBodyOfParts.listOfAllParts [i].getGlobalOriginPoint().y)+y;
						//						print ("outgoing x cord: " + outGoingXCord + " outgoing y cord: " + outGoingYCord);
						//						print("Type: "+wholeBodyOfParts.listOfAllParts[i].getName ());
						incomingSquareGrid[outGoingXCord][outGoingYCord].OccupiedSquare(wholeBodyOfParts.listOfAllParts [i]);	//sends a reference to the body part to the square marked as 'occupied'

						//if grid point is on, it finds the relative relation of the body part node and turns it on as an Occupiedsquare in the play area. it finds the relative location on the grid because each
						//body part knows its own global origin point, the 0,0 location is the lower left field off the square of the body part. No redundency yet for overlapping body parts.
					}
				}
			}
		}
		return incomingSquareGrid;
	}
	public void SetOpponentScript(){		//on contruction of play area, it identifys the owner of the play area and assigns the opposite as the opponent script
		if (tag == "EnemyController") {
			GameObject playerGameObjectTemp = GameObject.FindWithTag ("PlayerController");
			if (playerGameObjectTemp != null) {
				opponentPlayerController = playerGameObjectTemp.GetComponent<PlayerScript> ();
			}
			if (playerGameObjectTemp == null) {
				Debug.Log ("Cannot find 'playerGameObjectTemp'object");
			}
		}
		if (tag == "PlayerController") {
			GameObject playerGameObjectTemp = GameObject.FindWithTag ("EnemyController");
			if (playerGameObjectTemp != null) {
				opponentPlayerController = playerGameObjectTemp.GetComponent<PlayerScript> ();
			}
			if (playerGameObjectTemp == null) {
				Debug.Log ("Cannot find 'playerGameObjectTemp'object");
			}
		}


	}

	public void sendingAttackCard(XMLWeaponHitData weaponHitMatrix, float weaponDamage){		//info from the card in play by opponent's currently clicked on card, sending it down to playarea of this player
//		print("weapon damage "+ weaponDamage + tag);
		storedWeaponHitBoxForDelayedHit =  new CurrentWeaponHitBox(weaponHitMatrix, weaponDamage);
//		print ("stored damage " +storedWeaponHitBoxForDelayedHit.weaponDamage);
		playAreaScript.cardClickedOn (storedWeaponHitBoxForDelayedHit);	//sends the data to the play area
	}
//	public void storedWaitingAttackInfo( BPartGenericScript[] incomingListOfBPartsUnderThreat){	//this is stored to use on the opponents controller after players timer has counted down
//		//storedWeaponHitBoxForDelayedHit = incomingWeaponHitBox;
//		listOfBPartsUnderThreatForDelayedHit = incomingListOfBPartsUnderThreat;
//	}


	public void cardClickedOff(){ //signal from the opponent card that it has been unclicked
		playAreaScript.cardClickedOff();
	}

	public DeckScript getActiveDeck(){
		return activeDeck;
	}
	public GameControllerScript getGameController(){
		return gameController;
	}
	public string getWhichPlayer(){
		return gameObject.tag;
	}
	public string intToStringNumber(int incomingNumber){
		switch (incomingNumber) {
		case(1):
			return "one";
		case(2):
			return "two";
		case(3):
			return "three";
		case(4):
			return "four";
		}
		return null;
	}
	public bool getIfBodyPartsPopulated(){
		return bodyPartsPopulated;
	}
//	public AllPickedBodyParts getAllPickedBodyParts

}

