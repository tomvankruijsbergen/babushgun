using UnityEngine;
using System.Collections;

public class CometScript : MonoBehaviour {

	public float rotationSpeed;
	public Vector3 velocity;

	public float freezingFactor;
	public float timeRequiredForPoint;
	public int scoreAwarded;

	private Rigidbody2D ownRigidbody;

	void Start () {
		ownRigidbody = this.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate() {
		Vector3 rotation = transform.eulerAngles;
		rotation.z += rotationSpeed * Time.fixedDeltaTime;
		ownRigidbody.MoveRotation (rotation.z);

		Vector3 position = transform.position;
		position.x += velocity.x * Time.fixedDeltaTime;
		position.y += velocity.y * Time.fixedDeltaTime;
		position.z += velocity.z * Time.fixedDeltaTime;
		transform.position = position;
	}
}
