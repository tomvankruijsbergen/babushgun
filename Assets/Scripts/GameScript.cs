using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	public CharacterScript character;

	void Awake() {
		Camera.main.GetComponent<CameraScript> ().game = this;
	}

}
