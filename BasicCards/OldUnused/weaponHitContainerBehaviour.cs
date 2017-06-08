using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponHitContainerBehaviour : MonoBehaviour {


//	List<SpriteRenderer> hitSquares;
	CardBehaviour card;
	float heightOfAllHitSquares;
	float widthOfAllHitSquares;
	float heightOfHitSquares;
	float widthOfHitSquares;

	Vector3 startingV3;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}	
	public void takeInHitSquares(CardBehaviour incCard, float widthOfall, float heightOfall,  float widthOfHitSquaresT, float heightOfHitSquaresT){
		//hitSquares = newHitSquares;
		card = incCard;
		heightOfAllHitSquares = heightOfall;
		widthOfAllHitSquares = widthOfall;

		widthOfHitSquares = widthOfHitSquaresT;
		heightOfHitSquares = heightOfHitSquaresT;

		startingV3 = card.transform.localPosition;
		gameObject.transform.localPosition = card.transform.localPosition;

//		GameObject GameControllerTemp = GameObject.FindWithTag("GameController");
//		if (GameControllerTemp != null) {
//			//PlayArea = playAreaTemp;
//			startingV3 = GameControllerTemp.transform.localPosition;}
//		if(GameControllerTemp == null){
//			Debug.Log ("Cannot find 'GameController'object");}

//		GameObject playAreaT = GameObject.FindWithTag("PlayArea");
//		if(playAreaT != null){
//			startingV3 = playAreaT.GetComponent<GridMaker>().firstBoxCord;}
//		if(playAreaT == null){
//			Debug.Log ("Cannot find 'PlayArea'object");}

	}
	public void locationUpdate(Vector3 cardsPosition){
//		gameObject.transform.localPosition = cardsPosition;


		Vector3 curPosition = gameObject.transform.localPosition;

		if ((cardsPosition.x - startingV3.x)/widthOfHitSquares >= 1) {
			Vector3 newXposition = new Vector3((transform.position.x +widthOfHitSquares), startingV3.y, -1.0f);
			transform.position = newXposition;
			startingV3 = newXposition;
		}
		if ((cardsPosition.x - startingV3.x)/widthOfHitSquares <= -1) {
			Vector3 newXposition = new Vector3((transform.position.x -widthOfHitSquares), startingV3.y, -1.0f);
			transform.position = newXposition;
			startingV3 = newXposition;
		}

		if ((cardsPosition.y-startingV3.y)/heightOfHitSquares >= 1){
			Vector3 newYposition = new Vector3(startingV3.x, (transform.position.y +heightOfHitSquares), -1.0f);
			transform.position = newYposition;
			startingV3 = newYposition;

		}
		if ((cardsPosition.y-startingV3.y)/heightOfHitSquares <=-1 ){
			Vector3 newYposition = new Vector3(startingV3.x, transform.position.y -heightOfHitSquares, -1.0f);
			transform.position = newYposition;
			startingV3 = newYposition;

		}
	}
}
