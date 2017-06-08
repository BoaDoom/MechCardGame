using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualOnlyBPartGenericScript : MonoBehaviour {

	private BodyPartDataHolder bodyPartData;

	//completely static, non dependent variables
	private string bPartType;				//arm,head,legs,shoulder, or torso
	private string bPartName;
	private BodyPartNode[][] nodesOfBP;		//given an open grid, the list of active hitable points by list of vectors
	private Vector2[][] internalGlobalCords;
	//	private Vector2 anchorPoint;			//the location in which all parts will be located and placed
	//	private List<ComplexAnchorPoints> listOfComplexAnchorPoints = new List<ComplexAnchorPoints> ();
	private float maxHealth;
	private Vector2 dimensions;		//dependent on the farthest location from the source (0,0) of the list of binaryDimensions

	public ModuleSocketCount moduleSocketCount;
	public void Start(){
	}

	public void CreateNewPart(BodyPartDataHolder incomingBodyPartData){
		//Debug.Log ("incomingbpart name:"+ incomingBodyPartData.name +" leftright " +leftOrRight);
//		print(incomingBodyPartData);
		bPartType = incomingBodyPartData.typeOfpart;				//arm,head,legs,shoulder, or torso
		bPartName = incomingBodyPartData.name;

		maxHealth = incomingBodyPartData.maxHealth;
		moduleSocketCount = incomingBodyPartData.moduleSocketCount;

		nodesOfBP = new BodyPartNode[incomingBodyPartData.bodyPartGrid.Length][];
		for(int i=0; i < incomingBodyPartData.bodyPartGrid.Length; i++){	//transfering the int[][] grid
//			int g = incomingBodyPartData.bodyPartGrid.Length - i-1;
			nodesOfBP [i] = new BodyPartNode[incomingBodyPartData.bodyPartGrid[0].Length];
			for(int j=0; j < incomingBodyPartData.bodyPartGrid[0].Length; j++){
				BodyPartNode bodyPartNode = new BodyPartNode ();
				if (incomingBodyPartData.bodyPartGrid [i] [j] == 1) {
					bodyPartNode.turnOn ();
				}
				nodesOfBP [i] [j] = bodyPartNode;
			}
		}
		dimensions = new Vector2(nodesOfBP.Length, nodesOfBP[0].Length);		//dependent on the farthest location from the source (0,0) of the list of binaryDimensions
		//Debug.Log("complex list for "+bPartName+ " : "+ listOfComplexAnchorPoints.Count);

	}
	public string getName(){
		return bPartName;
	}
	public string getType(){
		return bPartType;
	}
	public Vector2 getDimensionsOfPart(){
		return dimensions;
	}
	public float getHealth(){
		return maxHealth;
	}
	public ModuleSocketCount getModuleSocketCount(){
		return moduleSocketCount;
	}

	public bool getGridPoint(Vector2 incomingPoint){
		return nodesOfBP [(int)incomingPoint.x] [(int)incomingPoint.y].getState ();
	}
}