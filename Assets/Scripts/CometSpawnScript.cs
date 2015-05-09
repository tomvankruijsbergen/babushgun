using UnityEngine;
using System.Collections;

public class CometSpawnScript : MonoBehaviour {

	public Transform cometPrefab;

	void Start () {
		GameScript.game.character.OnWentIntoSpace += OnWentIntoSpace;
		//OnWentIntoSpace (GameScript.game.character); //this is fired manually because there is no way this object can receive events from CharacterScript during start.
	}

	void OnWentIntoSpace(CharacterScript c) {
		if (cometPrefab == null) {
			return;
		}
		
		float timeUntilNextComet = Random.value * 3 + 2;

		// First, find the end point for the comet. This is the character's position plus speed over time.
		Vector3 targetPosition = c.transform.position;
		Vector3 targetVelocity = c.velocity ();

		targetPosition.x += targetVelocity.x * timeUntilNextComet;
		targetPosition.y += targetVelocity.y * timeUntilNextComet;
		targetPosition.z += targetVelocity.z * timeUntilNextComet;

		// Offset for the comet's size. (is this necessary? maybe not for iteration 1)
		//Vector3 cometSize = cometPrefab.GetComponent<Renderer>().bounds.size;

		// Decide what the comet's velocity and rotation speed will be.
		Vector2 direction = Random.insideUnitCircle.normalized;
		float speed = Random.value * 5 + 2;
		Vector3 cometVelocity = new Vector3 (direction.x * speed, 0, direction.y * speed);

		// Use this to offset the comet so that it will hit that set target after the time.
		targetPosition.x -= cometVelocity.x * timeUntilNextComet;
		targetPosition.y -= cometVelocity.y * timeUntilNextComet;
		targetPosition.z -= cometVelocity.z * timeUntilNextComet;

		// Now instantiate the final comet with the final values.
		Transform t = Instantiate (cometPrefab, targetPosition, Quaternion.identity) as Transform;

		float cometRotationSpeed = Random.value * 50 + 100;
		if (Random.value <= 0.5) cometRotationSpeed *= -1;

		CometScript comet = t.GetComponent<CometScript> ();
		comet.rotationSpeed = cometRotationSpeed;
		comet.velocity = cometVelocity;
	}

}
