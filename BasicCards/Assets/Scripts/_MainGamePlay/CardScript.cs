using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour {

	int cardSpriteNum;
	string nameOfCard; 
	int rankOfCard; 
	float attackDamageOfCard; 
	string typeOfAttack;

	private DeckScript deckScript;

	private bool active;
	private bool clickedOn;
	private bool discardLocation;
	private Sprite storedSprite;
	private SpriteRenderer spriteRenderer;
	public SpriteRenderer disabledCardSprite;
	private int hitSquareOverflow;

	private string controllerParentIDtag; //will either be EnemyController or PlayerController
	private PlayerScript selfPlayerController;
	private PlayerScript opponentPlayerController;
	private PlayerScript tempPlayerController;
	private LWCardInfo lWCardInfo;

	private XMLWeaponHitData hitBoxDataForCard;
	private BPartGenericScript BPartReference;

	//private GameControllerScript gameController;

	public void ManualStart(DeckScript incomingDeckReference) {
		deckScript = incomingDeckReference;
		//ActiveSquareBehaviour[] hitSquares;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
//		disabledCardSprite = gameObject.GetComponentInChildren<SpriteRenderer> ();
//		print (disabledCardSprite);
		disabledCardSprite.enabled = false;
		active = true;
		discardLocation = false;
		//cardInPlayArea = false;
		clickedOn = false;
		hitSquareOverflow = 0;

	}
	public void setFace(Sprite cardFaceGraphic){
		gameObject.GetComponent<SpriteRenderer>().sprite = cardFaceGraphic;
	}
	public void setWeaponHitBox(XMLWeaponHitData hitBoxDataForCardImport){
		hitBoxDataForCard = hitBoxDataForCardImport;
	}
	public void setBPartReference(BPartGenericScript incomingBPartReference){
		BPartReference = incomingBPartReference;
	}


	public int CardNumber{
		get{return cardSpriteNum;}
	}
	public float AttackValue{
		get{return attackDamageOfCard;}
	}
	public string TypeOfAttack{
		get{ return typeOfAttack; }
	}

	public XMLData CardAttributes{
		set{
			cardSpriteNum = value.cardSpriteNum;
			//nameOfCard = value.nameOfCard; /////////////////////////////////////Keep
			//rankOfCard = value.rankOfCard; /////////////////////////////////////Keep
			attackDamageOfCard = value.attackDamageOfCard; 
			typeOfAttack = value.typeOfAttack;
		}
	}
	public void setlWCardInfo(LWCardInfo incomingLWCardInfo){
		lWCardInfo = incomingLWCardInfo;
	}
	public LWCardInfo getlWCardInfo(){
		return lWCardInfo;
	}



	public void moveCard(Vector3 newPosition){
		gameObject.transform.position = newPosition;
	}

	private void OnMouseDown(){
		if (!clickedOn) {	//allows a single click down and up to activate and attach card to mouse pointer
			opponentPlayerController.sendingAttackCard (hitBoxDataForCard, attackDamageOfCard);		//sends the info about attack attached to the card to the gamecontroller
			clickedOn = true;
			//Debug.Log (gameObject.GetComponent<CardScript> ().AttackValue);
			deckScript.setCurrentCard (gameObject.GetComponent<CardScript>());	//sets the clicked on card in the deck object to let it know which card to delete/shuffle
		} else {
			clickedOn = false;
//			deckScript.emptyCurrentCard ();
		}
	}
	private void OnMouseUp(){
		if (!clickedOn) {	//if the card isn't currently clicked on it signals that it is no longer about to be played
			opponentPlayerController.cardClickedOff ();

		}
		if (discardLocation) {
			deckScript.turnOffCurrentCard ();
		}

	}
	void OnTriggerEnter2D(Collider2D other){
		//print ("");
		if (other.CompareTag("TargetSquare")){		//does not trigger anything if its colliding with anything else
//			print ("other tag : "+other.GetComponent<TargetSquareScript>().getPlayerID());
//			print ("Self tag : "+ getPlayerID());
			if (active && (hitSquareOverflow<=0) && (other.GetComponent<TargetSquareScript>().getPlayerID() != getPlayerID())){	//checks to compare if the card is being played in the enemy play area or its own
				hideCard ();		//if its being played in the enemy area, it 'hides' so the shooting pattern can be shown
//				print ("card hidden");
//				cardInPlayArea = true;
			}
			hitSquareOverflow++;			//the sum of all the small squares the card has entered. If number is 0, its left play area and can becom active again
		}
		if (other.CompareTag ("DiscardLocation")) {
			discardLocation = true;
		}
	}
	void OnTriggerExit2D(Collider2D other){		//a running count of all the squares the card has passed over, so it knows when to show back up if it is taken off the play area
		if (other.CompareTag("TargetSquare")){
			hitSquareOverflow--;
			if (!active && (hitSquareOverflow<=0)){
				showCard ();
//				cardInPlayArea = false;
			}
		}
		if (other.CompareTag ("DiscardLocation")) {
			discardLocation = false;
		}
	}

	public void hideCard(){
		active = false;
		storedSprite = spriteRenderer.sprite;
		spriteRenderer.sprite = null;
	}
	public void showCard(){
		active = true;
		spriteRenderer.sprite = storedSprite;
		//storedSprite = null;
	}

	public void deactivate(){		//once a card is played, it is deactivated to signal that it is no longer in use and will be destroyed on the next UpdateCards() actions called by the deck
		gameObject.SetActive (false);
	}
	public void SetPlayerAs(string incomingPlayerControllerIDTag){		//on contruction of card from the deck, it assigns the playeridtag

		GameObject playerGameObjectTemp = GameObject.FindWithTag("PlayerController");
		if(playerGameObjectTemp != null){
			selfPlayerController = playerGameObjectTemp.GetComponent<PlayerScript>();
		}
		if(playerGameObjectTemp == null){
			Debug.Log ("Cannot find 'playerGameObjectTemp'object");}
		
		GameObject enemyGameObjectTemp = GameObject.FindWithTag("EnemyController");
		if(enemyGameObjectTemp != null){
			opponentPlayerController = enemyGameObjectTemp.GetComponent<PlayerScript>();
		}
		if(enemyGameObjectTemp == null){
			Debug.Log ("Cannot find 'enemyGameObjectTemp'object");}

		if (incomingPlayerControllerIDTag == "EnemyController") {		//if the assigned incomingplayeridtag is enemy, swap the default assignment and assign self/owner script as the enemy
			tempPlayerController = selfPlayerController;
			selfPlayerController = opponentPlayerController;
			opponentPlayerController = tempPlayerController;
		}

		controllerParentIDtag = incomingPlayerControllerIDTag;
	}
	public IEnumerator checkIfBPartIsActive(){
		if (BPartReference.destroyedCheck ()) {
			//"do something to signal its destroyed"
			print(disabledCardSprite.enabled+ " this card is dead, don't use it plz");
			disabledCardSprite.enabled = true;
			print(disabledCardSprite.enabled+ " after test");

		}
		yield return null;
	}
	public string getPlayerID(){
		return controllerParentIDtag;
	}



}
