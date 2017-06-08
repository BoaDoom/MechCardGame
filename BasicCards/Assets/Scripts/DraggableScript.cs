using UnityEngine;
using System.Collections;


public class DraggableScript : MonoBehaviour 
{

	private Vector3 screenPoint;
	private Vector3 offset;
	private bool clickedOn;
	//private bool clickToggle;
	void Start(){
		clickedOn = false;
		//clickToggle = false;
	}
	void OnMouseDown()
	{
		if (clickedOn) {
			clickedOn = false;
		}
		else if (!clickedOn) {
			clickedOn = true;
			screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		}


	}
	void Update()
	{
		if (clickedOn) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			transform.position = curPosition;
		}
	}


}