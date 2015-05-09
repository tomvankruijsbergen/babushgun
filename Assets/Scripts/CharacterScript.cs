using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {

	public Vector2 initialForce;
	public float springForce;
	public float damper;

	public float dragOnComets;

	private Rigidbody2D ownRigidbody;
	private SpringJoint2D cometJoint;

	public float timeInSpace { get; private set; }
	public bool isInSpace() {
		return cometJoint == null;
	}
	public Vector2 velocity() {
		return ownRigidbody.velocity;
	}

	public float cometReleaseCooldown = 0.5f;
	private bool canReleaseFromComet;

	public float distanceFromComet = 1f;

	// events for going into space and landing 
	public delegate void _OnWentIntoSpace(CharacterScript c);
	public event _OnWentIntoSpace OnWentIntoSpace;

	public delegate void _OnLandedOntoComet(CharacterScript c);
	public event _OnLandedOntoComet OnLandedOntoComet;

	void Awake () {
		timeInSpace = 0;

		ownRigidbody = this.GetComponent<Rigidbody2D> ();
		ownRigidbody.AddForce (initialForce, ForceMode2D.Impulse);
	}

	void Update() {
		if (cometJoint == null) {
			timeInSpace += Time.deltaTime;
		} else {
			if (Input.anyKeyDown && canReleaseFromComet == true) {
				cometJoint.connectedBody.GetComponent<Collider2D>().enabled = false;
				Destroy (cometJoint);
				this.ownRigidbody.drag = 0;
				timeInSpace = 0;

				if (OnWentIntoSpace != null) OnWentIntoSpace(this);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (cometJoint != null) {
			return;
		}

		Vector2 hitPoint = c.contacts[0].point;

		this.ownRigidbody.drag = dragOnComets;

		cometJoint = gameObject.AddComponent<SpringJoint2D> () as SpringJoint2D;
		cometJoint.enableCollision = true;
		cometJoint.frequency = springForce;
		cometJoint.dampingRatio = damper;
		cometJoint.anchor = gameObject.transform.InverseTransformPoint (hitPoint);
		cometJoint.connectedBody = c.rigidbody;
		cometJoint.connectedAnchor = c.gameObject.transform.InverseTransformPoint (hitPoint);
		cometJoint.distance = distanceFromComet;

		this.canReleaseFromComet = false;
		Hashtable h = new Hashtable ();
		h.Add ("from", cometReleaseCooldown);
		h.Add ("to", cometReleaseCooldown);
		h.Add ("time", cometReleaseCooldown);
		h.Add ("onupdate", "OnTweenUpdateCometReleaseCooldown");
		h.Add ("oncomplete", "OnTweenCompleteCometReleaseCooldown");
		iTween.ValueTo (gameObject, h);

		if (OnLandedOntoComet != null) OnLandedOntoComet(this);

	}

	void OnTweenUpdateCometReleaseCooldown(float f) {

	}

	void OnTweenCompleteCometReleaseCooldown() {
		canReleaseFromComet = true;
	}

}