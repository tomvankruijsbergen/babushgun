using UnityEngine;
using System.Collections;

public class CometScript : MonoBehaviour {

	public float rotationSpeed;
	public Vector3 velocity;

	public float freezingFactor;
	public float timeRequiredForPoint;
	public int scoreAwarded;

	private bool markedForDestruction;
	[HideInInspector] public bool isTargetedAtPlayer;

	private Rigidbody2D ownRigidbody;

	void Awake() {
		isTargetedAtPlayer = false;
		markedForDestruction = false;
		ownRigidbody = this.GetComponent<Rigidbody2D> ();
	}

	void Start () {
		// Events here
	}

	public void MarkForEventualDestruction() {
		if (markedForDestruction == true)
			return;
		markedForDestruction = true;

		float destructionTime = 30f;
		Hashtable h = new Hashtable ();
		h.Add ("from", 0);
		h.Add ("to", destructionTime);
		h.Add ("time", destructionTime);
		h.Add ("onupdate", "OnTweenUpdateDestruction");
		h.Add ("oncomplete", "OnTweenCompleteDestruction");
		iTween.ValueTo (gameObject, h);
	}

	void OnTweenUpdateDestruction() {
		// Do nothing;
	}

	void OnTweenCompleteDestruction() {
		Destroy (gameObject);
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

	public void disableCollisionBriefly() {
		this.GetComponent<Collider2D> ().enabled = false;
		float timeDisabled = 0.5f;
		Hashtable h = new Hashtable ();
		h.Add ("from", 0);
		h.Add ("to", timeDisabled);
		h.Add ("time", timeDisabled);
		h.Add ("onupdate", "OnTweenUpdateCometCollision");
		h.Add ("oncomplete", "OnTweenCompleteCometCollision");
		iTween.ValueTo (gameObject, h);
	}

	void OnTweenUpdateCometCollision(float f) {
		// Do nothing
	}
	void OnTweenCompleteCometCollision() {
		this.GetComponent<Collider2D> ().enabled = true;
	}
}
