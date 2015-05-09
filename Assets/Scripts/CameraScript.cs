using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public CharacterScript followedCharacter;

	void Start () {

	}

	void LateUpdate() {
		if (followedCharacter == null)
			return;

		transform.position = followedCharacter.transform.position;
		transform.Translate(new Vector3(0, 0, -50), Space.Self);
	}
}
