using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSquareScript : MonoBehaviour {

	int gridCordX;
	int gridCordY;

	public Sprite occupiedUntargetedSprite;
	public Sprite occupiedTargetedSprite;
	public Sprite targetMissedSprite;
	public Sprite limbUnderThreatSprite;
	Sprite defaultSprite;
	private string playerControllerIDTag;

//	bool occupied = false;

	Sprite trueUntarget;
	Sprite trueTarget;
//	private int startingIntValue;

	public TargetSquareState activeSquareState;

	SpriteRenderer spriteRenderer;
	PlayAreaScript playArea;
//	PartPickerAreaScript partPickerAreaScript;
	BPartGenericScript bodyPartReference;
	//PlayAreaScript playAreaScript;

//	private bool bPUnderThreat = false;

	public IEnumerator ManualStart(PlayAreaScript parentPlayAreaScript){
		playArea = parentPlayAreaScript;
		StartCoroutine (ManualStartTwo ());
		yield return null;
	}
	public IEnumerator ManualStart(PartPickerAreaScript parentPartPickerAreaScript){
//		partPickerAreaScript = parentPartPickerAreaScript;
		StartCoroutine (ManualStartTwo ());
		yield return null;
	}
	public IEnumerator ManualStartTwo(){
		activeSquareState = new TargetSquareState();
//		activeSquareState.setOccupiedState (true);
//		print (activeSquareState.getOccupiedState());
		SpriteRenderer spriteRendererTemp = gameObject.GetComponent<SpriteRenderer>();
		if(spriteRendererTemp != null){
			spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
			//occupiedSprite =
			defaultSprite = spriteRenderer.sprite;
//			trueUntarget = spriteRenderer.sprite;
		}
		trueUntarget = defaultSprite;
		trueTarget = targetMissedSprite;
		if(spriteRendererTemp == null){
			Debug.Log ("Cannot find 'spriteRendererTemp'object");
		}
		hardUntargetSquare ();
		yield return null;
	}
	public void SetGridCordX(int cordx){
		gridCordX = cordx;
	}
	public void SetGridCordY(int cordy){
		gridCordY = cordy;
	}
	public int GetGridCordX(){
		return gridCordX;
	}
	public int GetGridCordY(){
		return gridCordY;
	}
//	void OnTriggerStay2D(Collider2D other){
//		if (other.CompareTag("weaponHitBox")){
//			spriteRenderer.sprite = occupiedSprite;
//		}
//	}
//	void OnTriggerExit2D(Collider2D other){
//		if (other.CompareTag("weaponHitBox")){
//			spriteRenderer.sprite = defaultSprite;
//		}
//	}
		
	void OnMouseEnter(){
		if (playArea != null) {
			playArea.squareHoveredOver (gridCordX, gridCordY);
		}
	}
	void OnMouseExit(){
		if (playArea != null) {
			playArea.squareHoveredOff ();
		}
	}
	void OnMouseDown(){
		if (playArea != null) {
			playArea.squareClickedOn (gridCordX, gridCordY);
		}
	}
		
	public void TargetSquare(){		//used by playarea to turn on and off targetting
		spriteRenderer.sprite = trueTarget;
		activeSquareState.setHardTargetedState(true);
		activeSquareState.setSoftTargetedState(true);
		if (bodyPartReference != null) {
			bodyPartReference.setBPartThreatenedOn ();
		}
//		bodyPartReference.setHardBPartTargetedOn ();
//		Debug.Log ("target triggered");

	}
	public void softUntargetSquare(){	//used by playarea to turn on and off targetting. The point of this is to turn off squares that arn't hovered over anymore, but to keep track of
										//where the last place the weapon shape was hovered over in case the user releases and 'fires' the weapon within the bounderies, the correct portions are hit
		spriteRenderer.sprite = trueUntarget;
		activeSquareState.setHardTargetedState(false);
//		Debug.Log ("soft untarget triggered");
	}
	public void hardUntargetSquare(){	//used by playarea to turn on and off targetting. Hard reset happens when Another
		spriteRenderer.sprite = trueUntarget;
		activeSquareState.setSoftTargetedState(false);
		activeSquareState.setHardTargetedState(false);		//redundent but needed
		if (bodyPartReference != null) {
			bodyPartReference.setBPartThreatenedOff ();
		}
//		Debug.Log ("hard untarget triggered");
	}

	public void setBPartUnderThreat(){
//		Vector2 tempVector2 = new Vector2 (gridCordX, gridCordY);
		if (activeSquareState.getOccupiedState () && 	//if its occupied by a bpart
			!activeSquareState.getSoftTargetedState () && 	//if its getting targetted by a weapon don't activate, so it doesn't override the weapon
			!activeSquareState.getHardTargetedState () && 	//
			bodyPartReference.getIfUnderThreat()) {			//checks to see if the body part is actually being targetted/under threat
				spriteRenderer.sprite = limbUnderThreatSprite;	//goal of the whole method, to swap to limbUnderThreat sprite
		}
//		bPUnderThreat = true;
	}
	public void setBPartNotUnderThreat(){
		spriteRenderer.sprite = trueUntarget;
//		bPUnderThreat = false;
	}
//	public void setBPartNotUnderThreatSoft(){
//		spriteRenderer.sprite = trueUntarget;
//		bodyPartReference.setHardBPartTargetedState(false);		//redundent but needed
//	}
//	public void setBPartNotUnderThreatHard(){
//		spriteRenderer.sprite = trueUntarget;
//		bodyPartReference.setSoftBPartTargetedState(false);
//		bodyPartReference.setHardBPartTargetedState(false);		//redundent but needed
//	}


//	public void takeOneSquareDamage(float incomingWeaponDamage){
//		bodyPartReference.takeDamage (incomingWeaponDamage);
//	}

	public void OccupiedSquare(BPartGenericScript incomingBodyPartReference){	//used by playerScript to turn on and off if the body part occupies the space
		bodyPartReference = incomingBodyPartReference;
		trueTarget = occupiedTargetedSprite;
		trueUntarget = occupiedUntargetedSprite;
		activeSquareState.setOccupiedState(true);
//		occupied = true;
		spriteRenderer.sprite = occupiedUntargetedSprite;
	}
	public void OccupiedSquare(){	//used by playerScript to turn on and off if the body part occupies the space. Alt method for when its just a preview window
		activeSquareState.setOccupiedState(true);
		spriteRenderer.sprite = occupiedUntargetedSprite;
	}
	public void DeactivateSquare(){		//used by playarea to turn on and off if the enemy occupies the space
		trueUntarget = defaultSprite;
		trueTarget = targetMissedSprite;
//		occupied = false;
		bodyPartReference = null;
		spriteRenderer.sprite = defaultSprite;
		activeSquareState.setOccupiedState(false);
	}

	public void SetPlayerAs(string incomingPlayerControllerIDTag){
		playerControllerIDTag = incomingPlayerControllerIDTag;
	}
	public string getPlayerID(){
		return playerControllerIDTag;
	}

//	public TargetSquareState getStateOfSquare(){
//		return activeSquareState;
//	}
//	void OnMouseExit(){
//		spriteRenderer.sprite = defaultSprite;
//	}
}

