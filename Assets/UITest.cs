using UnityEngine;
using System.Collections;

public class UITest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GUI.Label(new Rect(10, 10, 100, 20), "Hello World!");
	}

}
