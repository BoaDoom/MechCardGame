using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour {
	public Button shufflePlayerButton;
	public Button shuffleEnemyButton;
	//public DeckBehaviour deckBehav;
//	private DeckScript incomingPlayerScript.getActiveDeck();
//	private DeckScript playerDeckController;
	//public PlayAreaScript playAreaController;
	private PlayerScript enemyController;
	private PlayerScript playerController;

//	private PlayerScript storedAttackOnPlayer;
	private BPartGenericScript[] storedHitBodyParts;
	private CurrentWeaponHitBox storedWeaponHitBox;

	private SceneTransferVariablesScript sceneTransferVariablesScript;

//	public PlayerScript actingPlayer{ get; set; }
//	public PlayerScript opposingPlayer{ get; set; }

	public CurrentWeaponHitBox currentClickedOnCardWeaponMatrix{ get; set; }
	//private bool boolCardClickedOn;
//	private float publicTimer = 3f;

	void Start () {
		GameObject loaderScriptTemp = GameObject.FindWithTag("MainLoader");				//whole block is for grabbing the Deck object so it can deal a card when clicked
		if(loaderScriptTemp == null){
			SceneManager.LoadScene("XMLLoaderScene"); //Only happens if coroutine is finished
//			StartCoroutine (StartUpLoader());
			return;
		}

		GameObject sceneTransferVariablesScriptTemp = GameObject.FindWithTag("SceneTransferVariables");		//the script containing the variables input in from previous scenes to inform the rest of the game
		if (sceneTransferVariablesScriptTemp != null) {
			sceneTransferVariablesScript = sceneTransferVariablesScriptTemp.GetComponent<SceneTransferVariablesScript> ();
		} else {
			print ("Couldnt find SceneTransferVariablesScript");
		}

		GameObject tempEnemyController = GameObject.FindWithTag("EnemyController");				//whole block is for grabbing the Deck object so it can deal a card when clicked
		if (tempEnemyController != null) {
			enemyController = tempEnemyController.GetComponent<PlayerScript>();
		} else if (tempEnemyController == null) {
			Debug.Log ("couldn't find an object with 'EnemyController' tag");
		}
		GameObject tempPlayerController = GameObject.FindWithTag("PlayerController");				//whole block is for grabbing the Deck object so it can deal a card when clicked
		if (tempPlayerController != null) {
			playerController = tempPlayerController.GetComponent<PlayerScript>();;
		} else if (tempPlayerController == null) {
			Debug.Log ("couldn't find an object with 'PlayerController' tag");
		}
//		enemyController = gameObject.GetComponentInChildren<EnemyScript>();
//		playerController = gameObject.GetComponentInChildren<PlayerScript>();

//		incomingPlayerScript.getActiveDeck() = enemyController.GetComponentInChildren<DeckScript>();
//		playerDeckController = playerController.GetComponentInChildren<DeckScript>();

//		sceneTransferVariablesScript.bleh ();
		currentClickedOnCardWeaponMatrix = new CurrentWeaponHitBox( null, 0);
		shufflePlayerButton.onClick.AddListener(discardAllActivePlayerShuffle);
//		MakeSquaresButton.onClick.AddListener(makeActiveSquares);
		shuffleEnemyButton.onClick.AddListener(discardAllActiveEnemyShuffle);

		///////////commenting out after implimenting more info transfer, need to fix once I start to work on main screen play
//		if (sceneTransferVariablesScript == null) {			//this is here to default the body parts to slot 1 incase the game controller is ran before the part picker screen
//			SceneTransferVariablesScript sceneTransferVariablesScriptTempFiller = new SceneTransferVariablesScript ();
//			AllPickedBodyParts allPickedBodyPartsTempFiller = new AllPickedBodyParts ();
//			allPickedBodyPartsTempFiller.setAllPickedBodyParts ("Head One", "Arm One", "Torso One", "Shoulder One", "Leg One");
//			sceneTransferVariablesScriptTempFiller.setPartsPicked (allPickedBodyPartsTempFiller);
//			StartCoroutine (enemyController.ManualStart (sceneTransferVariablesScriptTempFiller.getAllPartsPicked ()));
//			StartCoroutine (playerController.ManualStart (sceneTransferVariablesScriptTempFiller.getAllPartsPicked ()));
//		} else {
			StartCoroutine (enemyController.ManualStart (sceneTransferVariablesScript.getAllPartsPicked ()));
			StartCoroutine (playerController.ManualStart (sceneTransferVariablesScript.getAllPartsPicked ()));
//		}
		///////////////////////////////////

	}
//	public void Update(){
//		publicTimer = publicTimer + Time.deltaTime;
//		//print ("Timer counted " +zeroTime );
//	}
//	public float getTime(){
//		return publicTimer;
//	}
//	IEnumerator StartUpLoader(){
//		SceneManager.LoadScene("XMLLoaderScene"); //Only happens if coroutine is finished
//		print("its still going");
//		yield return null;
//	}


//	public void makeBody(){
//		enemyController.populateBody ();
//		//enemyController.takeDamage ();
//	}
//	public void makeActiveSquares(){
//		playAreaController.populateEnemyPlayAreaSquares ();
//	}

								//	public void cardClickedOn(PlayerScript incomingPlayerScript, XMLWeaponHitData WeaponHitMatrix, float weaponDamage){		//command sent from the CardBehaviour script with info about the damage its doing
								//		if (incomingPlayerScript.tag == "PlayerController") {
								//			actingPlayer = playerController;
								//			opposingPlayer = enemyController;
								//		} else if (incomingPlayerScript.tag == "EnemyController") {
								//			actingPlayer = enemyController;
								//			opposingPlayer = playerController;
								//		} else {Debug.Log ("can't find appropriate player script");}
								//		currentClickedOnCardWeaponMatrix = new CurrentWeaponHitBox(incomingPlayerScript, true, WeaponHitMatrix, weaponDamage);
								//		opposingPlayer.getPlayAreaOfPlayer().hardResetSmallSquares ();
								//
								//		//boolCardClickedOn = true;
								//	}
					//	public void cardClickedOff(){				//sent from the cardbehaviour
					//		opposingPlayer.getPlayAreaOfPlayer().softResetSmallSquares ();			//resets all the targetting squares if the card is released. If not in place, used cards never 'exit'
					//		currentClickedOnCardWeaponMatrix.isCardClickedOn = false;
					//	}

	public void discardAllActivePlayerShuffle (){			//discards all active cards and cards in draw pile and then shuffles
		playerController.getActiveDeck().discardAllActiveShuffle();
	}
	public void discardAllActiveEnemyShuffle (){			//discards all active cards and cards in draw pile and then shuffles
		enemyController.getActiveDeck().discardAllActiveShuffle();
	}

//	public void startSendingDamage(){
//		StartCoroutine( actingPlayer.startTicker(2f));		//starts the timer bar with a countdown on that specific player
//
//	}
					//	public void transferOfCardDamage(){		//is sent by the play area script that the active card was just played
					////		Debug.Log("target: " +playAreaController.getActiveSquareStateSoftTarget(0,0));
					////		Debug.Log("occupied: " +playAreaController.getActiveSquareStateOccupied(0,0));
					//		//incomingPlayerScript.takeDamage (currentClickedOnCardWeaponMatrix);
					//		StartCoroutine( actingPlayer.startTicker(2f));		//starts the timer bar with a countdown on that specific player
					//		if (storedHitBodyParts != null) {
					//			for (int i = 0; storedHitBodyParts.Length >= 0; i++) {
					//				storedHitBodyParts [i] = null;
					//			}
					//		}
					//		int f = 0;
					//		foreach (BPartGenericScript bodyPartObject in opposingPlayer.getWholeBodyOfParts().listOfAllParts){
					//
					//			if (bodyPartObject.getIfUnderThreat ()) {
					//				//print ("body part run though");
					//				storedHitBodyParts[f] = bodyPartObject;	//stores the incoming data so that it can allow the rest of the game run but delete card and allow targetting squares to reset
					//				storedWeaponHitBox = currentClickedOnCardWeaponMatrix;
					//				storedAttackOnPlayer = opposingPlayer;
					////				bodyPartObject.takeDamage (currentClickedOnCardWeaponMatrix);
					////				opposingPlayer.updateHealthDisplay ();
					//				f++;
					//			}
					//		}
					//		cardClickedOff ();
					//	}
					//	public void executeDelayedCardDamage(){
					//		foreach (BPartGenericScript bodyPartObject in storedHitBodyParts) {
					//			bodyPartObject.takeDamage (currentClickedOnCardWeaponMatrix);
					//		}
					//		opposingPlayer.updateHealthDisplay ();
					//	}
					//	public DeckScript getEnemyDeckController(){
					//		return incomingPlayerScript.getActiveDeck();
					//	}

}
public class CurrentWeaponHitBox{
//	public bool isCardClickedOn{ get; set; }
	public XMLWeaponHitData weaponHitData{ get;  set; }
	public float weaponDamage{ get; private set; }
	//public PlayerScript actingPlayerScript;
	public CurrentWeaponHitBox( XMLWeaponHitData incomingWeaponHitData, float weaponDamageT){
		//actingPlayerScript = incomingPlayerScript;
//		isCardClickedOn = incomingCardClickedData;
		weaponHitData = incomingWeaponHitData;
		weaponDamage = weaponDamageT;
	}
}
