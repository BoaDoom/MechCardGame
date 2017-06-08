using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndrawnDeckScript : MonoBehaviour {

	//private SpriteRenderer spriteRenderer;
	private GameObject deckBehaviourObject;
	private DeckScript deckScript;

	public void ManualStart() {
		deckScript = gameObject.transform.parent.GetComponent<DeckScript> ();
//		deckScript = incomingDeckScript;
//		print ("done "+ incomingDeckScript.getControllerParentIdTag());
//		GameObject deckBehaviourObject = GameObject.FindWithTag("DeckController");				//whole block is for grabbing the Deck object so it can deal a card when clicked
//		if(deckBehaviourObject != null){
//			deckScript = deckBehaviourObject.GetComponent<DeckScript>();
//		}
//		if(deckBehaviourObject == null){
//			Debug.Log ("Cannot find 'DeckController'object");
//		}
		//spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void OnMouseDown(){																			//if the deck pile is clicked on, another card is dealt
		deckScript.DealCard ();
	}
		
}
