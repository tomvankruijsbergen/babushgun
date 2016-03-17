using UnityEngine;
using System.Collections;

public class CometSpawnScript : MonoBehaviour {

	public Transform[] cometPrefabs;

	public float cometOriginAngleRangeSize = 0f;
	public float levelRadiusFactorForSavingComets = 0;

	public float cometSpawnTimerMin = 2f;
	public float cometSpawnTimerMax = 2.5f;

	void Start () {
		cometOriginAngleRangeSize *= Mathf.Deg2Rad;
		GameScript.game.character.OnWentIntoSpace += OnWentIntoSpace;
		GameScript.game.OnReset += OnReset;
	}

	void OnReset() {
		CancelInvoke ();
		SpawnRandomComet ();
	}

	void OnDestroy() {
		GameScript.game.character.OnWentIntoSpace -= OnWentIntoSpace;
	}

	void SpawnRandomComet() {
		Transform cometModel = cometPrefabs[(int)Mathf.Floor(Random.value * cometPrefabs.Length)];

		float startArea = GameScript.game.levelRadius * 3;

		float speed = Random.value * 36 + 48;
		float direction = Random.value * Mathf.PI * 2;
		Vector3 cometVelocity = new Vector3 (Mathf.Cos(direction) * speed, Mathf.Sin(direction) * speed, 0);

		Vector3 position = new Vector3 ();
		position.x = startArea * Mathf.Cos (direction + Mathf.PI);
		position.y = startArea * Mathf.Sin (direction + Mathf.PI);

		float angleNormal = direction + Mathf.PI * 0.5f;
		float offset = GameScript.game.levelRadius * 1.5f;
		position.x += Mathf.Cos (angleNormal) * (Random.value * 2 * offset - offset);
		position.y += Mathf.Sin (angleNormal) * (Random.value * 2 * offset - offset);

		float cometRotationSpeed = Random.value * 20 + 190;
		if (Random.value <= 0.5) cometRotationSpeed *= -1;

		SpawnComet (cometModel, position, cometVelocity, cometRotationSpeed, true);

		float time = cometSpawnTimerMin + Random.value * (cometSpawnTimerMax - cometSpawnTimerMin);
		Invoke ("SpawnRandomComet", time);
	}

	void OnWentIntoSpace(CharacterScript c) {
		if (cometPrefabs.Length == 0) {
			return;
		}
		
		float timeUntilNextComet = Random.value * 1.5f + 4f;

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
		float direction = Random.value * Mathf.PI * 2;

		// Adjust direction if the character is on the edges of a level. In that case, comets should lead into the level.
		float levelRadius = GameScript.game.levelRadius * levelRadiusFactorForSavingComets;
		float angle = Mathf.Atan2 (targetPosition.y, targetPosition.x);
		float distance = targetPosition.magnitude;
		if (distance >= levelRadius) {
			float directionLeftBound = angle + Mathf.PI - 0.5f * cometOriginAngleRangeSize;
			direction = Random.value * cometOriginAngleRangeSize + directionLeftBound;
		}

		// Decide what the comet's velocity and rotation speed will be.
		float speed = Random.value * 36 + 48;
		Vector3 cometVelocity = new Vector3 (Mathf.Cos(direction) * speed, Mathf.Sin(direction) * speed, 0);

		// Use this to offset the comet so that it will hit that set target after the time.
		targetPosition.x -= cometVelocity.x * timeUntilNextComet;
		targetPosition.y -= cometVelocity.y * timeUntilNextComet;
		targetPosition.z -= cometVelocity.z * timeUntilNextComet;

		float cometRotationSpeed = Random.value * 20 + 190;
		if (Random.value <= 0.5) cometRotationSpeed *= -1;

		// Now instantiate the final comet with the final values.
		Transform cometModel = cometPrefabs[(int)Mathf.Floor(Random.value * cometPrefabs.Length)];
		SpawnComet (cometModel, targetPosition, cometVelocity, cometRotationSpeed, true);

	}

	void SpawnComet(Transform cometModel, Vector3 position, Vector3 velocity, float rotationSpeed, bool shouldRemove = true) {
		Transform t = Instantiate (cometModel, position, Quaternion.identity) as Transform;
		
		CometScript comet = t.GetComponent<CometScript> ();
		comet.rotationSpeed = rotationSpeed;
		comet.velocity = velocity;
		if (shouldRemove == true) comet.MarkForEventualDestruction ();
	}

}
