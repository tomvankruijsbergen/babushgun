using UnityEngine;
using System.Collections;

public class CollectibleSpawnScript : MonoBehaviour {

	public Transform collectible;
	public int concurrentCollectibles;
	public float spawnDeadZone = 0.33f;
	public float spawnRangeLevelRadiusFactor = 1.5f;

	void Start () {
		GameScript.game.character.OnCharacterPickupCollectible += OnCharacterPickupCollectible;

		for (int i = 0; i < concurrentCollectibles; i++) {
			SpawnCollectible();
		}
	}

	void OnDestroy() {
		GameScript.game.character.OnCharacterPickupCollectible -= OnCharacterPickupCollectible;
	}

	void OnCharacterPickupCollectible(CharacterScript character, CollectibleScript collectible) {
		SpawnCollectible ();
	}

	void SpawnCollectible() {
		float angle = Random.value * Mathf.PI * 2;
		float radius = GameScript.game.levelRadius * spawnRangeLevelRadiusFactor;
		float distance = spawnDeadZone * radius + Random.value * (1 - spawnDeadZone) * radius;

		Vector3 position = new Vector3 (Mathf.Cos (angle) * distance, Mathf.Sin (angle) * distance, 0);
		Instantiate (collectible, position, Quaternion.identity);

	}

}
