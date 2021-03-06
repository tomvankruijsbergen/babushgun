﻿using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public float followPercentage = 0.8f;
	public float onCometCameraSize = 15;
	public float onSpaceCameraSize = 20;

	private Camera ownCamera;

	private const string TWEENNAME = "CameraScript.orthographicSize";

	void Start() {
		ownCamera = this.GetComponent<Camera> ();
		GameScript.game.character.OnWentIntoSpace += OnWentIntoSpace;
		GameScript.game.character.OnLandedOntoComet += OnLandedOntoComet;
		GameScript.game.OnReset += OnReset;
	}

	void OnDestroy() {
		GameScript.game.character.OnWentIntoSpace -= OnWentIntoSpace;
		GameScript.game.character.OnLandedOntoComet -= OnLandedOntoComet;
		GameScript.game.OnReset -= OnReset;
	}

	void OnReset() {
		iTween.StopByName (TWEENNAME);
		transform.position = new Vector3 ();
		ownCamera.orthographicSize = onCometCameraSize;
	}

	void Update() {
		if (GameScript.game.character == null)
			return;

		Vector3 targetPosition = GameScript.game.character.transform.position;
		Vector3 newPosition = transform.position;
		newPosition.x += followPercentage * (targetPosition.x - transform.position.x);
		newPosition.y += followPercentage * (targetPosition.y - transform.position.y);
		newPosition.z += followPercentage * (targetPosition.z - transform.position.z);

		//ownCamera.orthographicSize = 10;

		transform.position = newPosition;
		transform.Translate(new Vector3(0, 0, -10), Space.Self);
	}

	void OnWentIntoSpace(CharacterScript c) {
		iTween.StopByName (TWEENNAME);

		Hashtable h = new Hashtable ();
		h.Add ("from", ownCamera.orthographicSize);
		h.Add ("to", onSpaceCameraSize);
		h.Add ("time", 3f);
		h.Add ("onupdate", "OnTweenUpdateOrthographicSize");
		h.Add ("easetype", "easeInOutQuad");
		h.Add ("name", TWEENNAME);
		iTween.ValueTo (gameObject, h);
	}

	void OnLandedOntoComet(CharacterScript c) {
		iTween.StopByName (TWEENNAME);

		Hashtable h = new Hashtable ();
		h.Add ("from", ownCamera.orthographicSize);
		h.Add ("to", onCometCameraSize);
		h.Add ("time", 1.5f);
		h.Add ("easetype", "easeOutCubic");
		h.Add ("onupdate", "OnTweenUpdateOrthographicSize");
		h.Add ("name", TWEENNAME);
		iTween.ValueTo (gameObject, h);
	}

	void OnTweenUpdateOrthographicSize(float newValue) {
		ownCamera.orthographicSize = newValue;
	}
	
}
