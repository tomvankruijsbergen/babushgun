using UnityEngine;
using System.Collections;

public class CometSpawnScript : MonoBehaviour {

	public Transform[] cometPrefabs;

	public float cometOriginAngleRangeSize = 120f;

	void Start () {
		cometOriginAngleRangeSize *= Mathf.Deg2Rad;
		GameScript.game.character.OnWentIntoSpace += OnWentIntoSpace;
		//OnWentIntoSpace (GameScript.game.character); //this is fired manually because there is no way this object can receive events from CharacterScript during start.
	}

	void OnWentIntoSpace(CharacterScript c) {
		if (cometPrefabs.Length == 0) {
			return;
		}
		
		float timeUntilNextComet = Random.value * 1.5f + 3.5f;

		// First, find the end point for the comet. This is the character's position plus speed over time.
		Vector3 targetPosition = c.transform.position;
		Vector3 targetVelocity = c.velocity ();

		targetPosition.x += targetVelocity.x * timeUntilNextComet;
		targetPosition.y += targetVelocity.y * timeUntilNextComet;
		targetPosition.z += targetVelocity.z * timeUntilNextComet;

		// Offset for the comet's size. (is this necessary? maybe not for iteration 1)
		//Vector3 cometSize = cometPrefab.GetComponent<Renderer>().bounds.size;

		/* Decide what the comet's direction will be. We want to exclude a few directions: comets should come from
		 * different sides, and also they should be more likely to come from the edge of the level if you are
		 * far away from the center of the level. */
		float direction = Random.value;

		// Adjust direction for level size.
		float levelRadius = GameScript.game.levelRadius;
		float angle = Mathf.Atan2 (targetPosition.z, targetPosition.x);
		float distance = targetPosition.magnitude;
		if (distance >= levelRadius) {
			float directionLeftBound = angle + Mathf.PI - 0.5f * cometOriginAngleRangeSize;
			direction = Random.value * cometOriginAngleRangeSize + directionLeftBound;
		}

		// Decide what the comet's velocity and rotation speed will be.
		float speed = Random.value * 36 + 24;
		Vector3 cometVelocity = new Vector3 (Mathf.Cos(direction) * speed, Mathf.Sin(direction) * speed, 0);

		// Use this to offset the comet so that it will hit that set target after the time.
		targetPosition.x -= cometVelocity.x * timeUntilNextComet;
		targetPosition.y -= cometVelocity.y * timeUntilNextComet;
		targetPosition.z -= cometVelocity.z * timeUntilNextComet;

		// Now instantiate the final comet with the final values.
		Transform cc = cometPrefabs[(int)Mathf.Floor(Random.value * cometPrefabs.Length)];
		Transform t = Instantiate (cc, targetPosition, Quaternion.identity) as Transform;

		float cometRotationSpeed = Random.value * 40 + 120;
		if (Random.value <= 0.5) cometRotationSpeed *= -1;

		CometScript comet = t.GetComponent<CometScript> ();
		comet.rotationSpeed = cometRotationSpeed;
		comet.velocity = cometVelocity;
	}

}
