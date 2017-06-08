using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (gameObject); //Allows Loader to carry over into new scene 
	}
}
