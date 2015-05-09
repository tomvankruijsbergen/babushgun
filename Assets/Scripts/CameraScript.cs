using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	[HideInInspector] public GameScript game;
	//public CharacterScript followedCharacter;

	void Start () {

	}

	void LateUpdate() {
		if (game.character == null)
			return;

		transform.position = game.character.transform.position;
		transform.Translate(new Vector3(0, 0, -10), Space.Self);
	}
}
