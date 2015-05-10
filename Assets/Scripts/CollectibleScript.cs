using UnityEngine;
using System.Collections;

public class CollectibleScript : MonoBehaviour {

	public float pointsAwarded;
	public float rotationSpeed;

	public void Start() {
		GameScript.game.character.OnCharacterPickupCollectible += OnCharacterPickupCollectible;
		GameScript.game.character.OnCharacterDeath += OnCharacterDeath;

		transform.Rotate (new Vector3 (0, 0, Random.value * 360));

		if (Random.value <= 0.5)
			rotationSpeed = -rotationSpeed;
	}

	public void Update() {
		transform.Rotate (new Vector3 (0, 0, rotationSpeed * Time.deltaTime));
	}

	public void OnDestroy() {
		GameScript.game.character.OnCharacterPickupCollectible -= OnCharacterPickupCollectible;
		GameScript.game.character.OnCharacterDeath -= OnCharacterDeath;
	}

	void OnCharacterPickupCollectible(CharacterScript character, CollectibleScript collectible) {
		if (collectible != this)
			return;
		Destroy (gameObject);
	}

	void OnCharacterDeath(CharacterScript c) {
		Destroy (gameObject);
	}

}
