using UnityEngine;
using System.Collections;



public class CharacterScript : MonoBehaviour {

	public Vector3 initialForce;
	public float springForce;
	public float damper;

	public float dragOnComets;

	private Rigidbody ownRigidbody;
	private SpringJoint cometJoint;

	public float timeInSpace { get; private set; }
	public bool isInSpace() {
		return cometJoint == null;
	}
	public Vector3 velocity() {
		return ownRigidbody.velocity;
	}

	// event going into space
	public delegate void _OnWentIntoSpace(CharacterScript c);
	[HideInInspector] public event _OnWentIntoSpace OnWentIntoSpace;

	void Awake () {
		timeInSpace = 0;

		ownRigidbody = this.GetComponent<Rigidbody> ();
		ownRigidbody.AddForce (initialForce, ForceMode.VelocityChange);
	}

	void Update() {
		if (cometJoint == null) {
			timeInSpace += Time.deltaTime;
		} else {
			if (Input.anyKeyDown) {
				cometJoint.connectedBody.detectCollisions = false;
				Destroy (cometJoint);
				this.ownRigidbody.drag = 0;
				timeInSpace = 0;

				if (OnWentIntoSpace != null) OnWentIntoSpace(this);
			}
		}
	}

	void OnCollisionEnter(Collision c) {
		if (cometJoint != null) {
			return;
		}

		Vector3 hitPoint = c.contacts[0].point;

		this.ownRigidbody.drag = dragOnComets;

		cometJoint = gameObject.AddComponent<SpringJoint> () as SpringJoint;
		cometJoint.autoConfigureConnectedAnchor = false;
		cometJoint.enableCollision = true;
		cometJoint.spring = springForce;
		cometJoint.damper = damper;
		cometJoint.anchor = gameObject.transform.InverseTransformPoint (hitPoint);
		cometJoint.connectedBody = c.rigidbody;
		cometJoint.connectedAnchor = c.gameObject.transform.InverseTransformPoint (hitPoint);
		cometJoint.minDistance = 0.5f;
		cometJoint.maxDistance = 0.6f;
	}
}
