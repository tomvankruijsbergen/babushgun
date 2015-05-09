using UnityEngine;
using System.Collections;

public class CometScript : MonoBehaviour {

	public float rotationSpeed;
	public Vector3 velocity;

	private Rigidbody ownRigidbody;

	void Start () {
		ownRigidbody = this.GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		Vector3 rotation = transform.eulerAngles;
		rotation.y += rotationSpeed * Time.fixedDeltaTime;
		ownRigidbody.MoveRotation (Quaternion.Euler (rotation));

		Vector3 position = transform.position;
		position.x += velocity.x * Time.fixedDeltaTime;
		position.y += velocity.y * Time.fixedDeltaTime;
		position.z += velocity.z * Time.fixedDeltaTime;
		//ownRigidbody.MovePosition (position);
		transform.position = position;
	}
}
