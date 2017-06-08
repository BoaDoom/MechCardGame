using UnityEngine;
using System.Collections;


public class DraggableGrid : MonoBehaviour 
{

	private Vector3 screenPoint;
	private Vector3 startingV3;
	private Vector3 offset;

	private float gridWidth;
	private float gridHeight;

	//private float currentXSnap;
	//private float currentYSnap;

	private Vector3 gridStartPoint;
	void Start(){
		//currentXSnap = 0.0f;
		//currentXSnap = 0.0f;
		gridWidth = 1.2f;
		gridHeight = 1.2f;
	}
	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		startingV3 = gameObject.transform.position;
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

	}
	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		//currentXSnap = curPosition.x;
		if ((curPosition.x-startingV3.x)/gridWidth >= 1 || (curPosition.x-startingV3.x)/gridWidth <=-1 ){
			Vector3 newXposition = new Vector3(curPosition.x, startingV3.y, startingV3.z);
			transform.position = newXposition;
			startingV3 = newXposition;

			//startingV3 = curPosition;
		}
		if ((curPosition.y-startingV3.y)/gridHeight >= 1 || (curPosition.y-startingV3.y)/gridHeight <=-1 ){
			Vector3 newYposition = new Vector3(startingV3.x, curPosition.y, startingV3.z);
			transform.position = newYposition;
			startingV3 = newYposition;
		}
	}
}