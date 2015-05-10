using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScript : MonoBehaviour {

	[HideInInspector] public static GameScript game  { get; private set; }

	public CharacterScript character;

	public Text frostText;
	public Text scoreText;

	public float levelRadius;

	void Awake() {
		GameScript.game = this;

		frostText.text = "0";
		scoreText.text = "0";

		this.character.OnFrozenFactorChanged += OnFrozenFactorChanged;
		this.character.OnCharacterScoreChanged += OnCharacterScoreChanged;
	}

	void OnFrozenFactorChanged(CharacterScript c) {
		int frozenAmount = (int)((1 - c.frozenFactor) * 100);
		frostText.text = "" + frozenAmount;
	}
	void OnCharacterScoreChanged(CharacterScript c) {
		scoreText.text = "" + c.score;
	}


}
