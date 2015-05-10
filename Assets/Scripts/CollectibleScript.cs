using UnityEngine;
using System.Collections;

public class CollectibleScript : MonoBehaviour {

	public float scoreAwarded;

	public void Start() {
		GameScript.game.character.OnCharacterPickupCollectible += OnCharacterPickupCollectible;
	}

	public void OnDestroy() {
		GameScript.game.character.OnCharacterPickupCollectible -= OnCharacterPickupCollectible;
	}

	void OnCharacterPickupCollectible(CharacterScript character, CollectibleScript collectible) {
		if (collectible != this)
			return;
		Destroy (gameObject);
	}

}
