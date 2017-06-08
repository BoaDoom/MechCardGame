using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSquareScript : MonoBehaviour {

	int gridCordX;
	int gridCordY;

	public Sprite occupiedUntargetedSprite;
	public Sprite occupiedTargetedSprite;
	public Sprite targetMissedSprite;
	public Sprite limbUnderThreatSprite;
	Sprite defaultSprite;
	private string playerControllerIDTag;

	Sprite trueUntarget;
//	Sprite trueTarget;
//	private int startingIntValue;

	public TargetSquareState activeSquareState;

	SpriteRenderer spriteRenderer;
//	PlayAreaScript playArea;
	BPartGenericScript bodyPartReference;
	//PlayAreaScript playAreaScript;

//	private bool bPUnderThreat = false;

	public IEnumerator ManualStart(PlayAreaScript parentPlayAreaScript){
//		playArea = parentPlayAreaScript;
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
//		trueTarget = targetMissedSprite;
		if(spriteRendererTemp == null){
			Debug.Log ("Cannot find 'spriteRendererTemp'object");
		}

//		GameObject playAreaTemp = GameObject.FindWithTag ("PlayAreaController");
//		if(playAreaTemp != null){
//			playArea = playAreaTemp.GetComponent<PlayAreaScript>();
//		}
//		if(playAreaTemp == null){
//			Debug.Log ("Cannot find 'playAreaImport'object");
//		}
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
	public void hardUntargetSquare(){	//used by playarea to turn on and off targetting. Hard reset happens when Another
		spriteRenderer.sprite = trueUntarget;
		activeSquareState.setSoftTargetedState(false);
		activeSquareState.setHardTargetedState(false);		//redundent but needed
		if (bodyPartReference != null) {
			bodyPartReference.setBPartThreatenedOff ();
		}
//		Debug.Log ("hard untarget triggered");
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


	public void OccupiedSquare(){	//used by playerScript to turn on and off if the body part occupies the space. Alt method for when its just a preview window
		activeSquareState.setOccupiedState(true);
		spriteRenderer.sprite = occupiedUntargetedSprite;
	}
	public void DeactivateSquare(){		//used by playarea to turn on and off if the enemy occupies the space
//		trueUntarget = defaultSprite;
//		trueTarget = targetMissedSprite;
		spriteRenderer.sprite = trueUntarget;
		activeSquareState.setOccupiedState(false);
	}
		
//	public TargetSquareState getStateOfSquare(){
//		return activeSquareState;
//	}
//	void OnMouseExit(){
//		spriteRenderer.sprite = defaultSprite;
//	}
}

