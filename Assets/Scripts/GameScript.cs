using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	[HideInInspector] public static GameScript game  { get; private set; }

	public CharacterScript character;

	public float levelRadius;

	void Awake() {
		GameScript.game = this;
	}

	void Start() {

	}


}
