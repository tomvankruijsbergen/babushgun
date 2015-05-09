using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public float followPercentage = 0.8f;

	void FixedUpdate() {
		if (GameScript.game.character == null)
			return;

		Vector3 targetPosition = GameScript.game.character.transform.position;
		Vector3 newPosition = transform.position;
		newPosition.x += followPercentage * (targetPosition.x - transform.position.x);
		newPosition.y += followPercentage * (targetPosition.y - transform.position.y);
		newPosition.z += followPercentage * (targetPosition.z - transform.position.z);

		transform.position = newPosition;
		transform.Translate(new Vector3(0, 0, -10), Space.Self);
	}
}
