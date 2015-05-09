using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {

	public Vector3 initialForce;
	public float springForce;
	public float damper;

	public float dragOnComets;

	private Rigidbody ownRigidbody;
	private SpringJoint cometJoint;

	public bool isFloating { get; private set; }

	void Start () {
		ownRigidbody = this.GetComponent<Rigidbody> ();
		ownRigidbody.AddForce (initialForce, ForceMode.VelocityChange);
	}

	void Update() {
		if (Input.anyKey) {
			if (cometJoint != null) {
				Destroy(cometJoint);
				this.ownRigidbody.drag = 0;
				print (ownRigidbody.velocity);
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
