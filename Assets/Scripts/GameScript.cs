using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	public CharacterScript character;

	void Awake() {
		// set the camera's reference to this object manually, because we can access the camera through Camera.main;
		Camera.main.GetComponent<CameraScript> ().game = this;
	}



}
