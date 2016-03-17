using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScript : MonoBehaviour {

	[HideInInspector] public static GameScript game  { get; private set; }

	public CharacterScript character;
	public CanvasGroup frostCanvasGroup;
	public CanvasGroup gameOverCanvasGroup;
	public CanvasGroup titleCanvasGroup;

	public Text frostText;
	public Text scoreText;

	public float levelRadius;

	public delegate void _OnReset();
	public event _OnReset OnReset;

	private bool canPressButtonToReset = false;
	private bool canPressButtonToStart = true;

	void Awake() {
		GameScript.game = this;

		this.character.OnFrozenFactorChanged += OnFrozenFactorChanged;
		this.character.OnCharacterScoreChanged += OnCharacterScoreChanged;
		this.character.OnCharacterDeath += OnCharacterDeath;

		this.OnReset += InternalOnReset;

		titleCanvasGroup.alpha = 0;
	}

	void Start() {
		//Invoke ("OnResetCaller", 0.01f);
	}

	void OnResetCaller() {
		if (this.OnReset != null)
			this.OnReset ();
	}

	void InternalOnReset() {
		canPressButtonToReset = false;
	}

	void OnFrozenFactorChanged(CharacterScript c) {
		int frozenAmount = (int)((1 - c.frozenFactor) * 100);
		frostText.text = "" + frozenAmount;
		frostCanvasGroup.alpha = c.frozenFactor;
	}
	void OnCharacterScoreChanged(CharacterScript c) {
		scoreText.text = "" + Mathf.Floor(c.score);
	}

	void OnCharacterDeath(CharacterScript c) {
		Hashtable h = new Hashtable ();
		h.Add ("from", 0);
		h.Add ("to", 1);
		h.Add ("delay", 0.5f);
		h.Add ("time", 1);
		h.Add ("onupdate", "OnCharacterDeathUpdate");
		h.Add ("oncomplete", "OnCharacterDeathComplete");
		iTween.ValueTo (gameObject, h);
	}

	void OnCharacterDeathUpdate(float value) {
		gameOverCanvasGroup.alpha = value;
	}

	void OnCharacterDeathComplete() {
		canPressButtonToReset = true;
	}

	void Update() {
		if (Input.anyKeyDown && canPressButtonToStart == true) {
			canPressButtonToStart = false;
			titleCanvasGroup.alpha = 0;
			this.OnReset();
		}
		if (Input.anyKeyDown && canPressButtonToReset == true) {
			gameOverCanvasGroup.alpha = 0;
			this.OnReset();
		}
	}

}
