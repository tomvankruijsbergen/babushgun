using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {

	public Vector2 initialForce;
	public float springForce;
	public float damper;

	private Rigidbody2D ownRigidbody;
	private SpringJoint2D cometJoint;
	private CometScript attachedComet;

	public float timeInSpace { get; private set; }
	public bool isInSpace() {
		return cometJoint == null;
	}

	public Vector2 velocity() {
		return ownRigidbody.velocity;
	}

	public float cometReleaseCooldown = 0.5f;
	private bool canReleaseFromComet;
	public float dragOnComets;
	public float distanceFromComet = 1f;

	// events for going into space and landing 
	public delegate void _OnWentIntoSpace(CharacterScript c);
	public event _OnWentIntoSpace OnWentIntoSpace;

	public delegate void _OnLandedOntoComet(CharacterScript c);
	public event _OnLandedOntoComet OnLandedOntoComet;

	// freezing and unfreezing
	public float frozenFactor { get; private set; }
	public float unfreezeSpeed;
	public float velocityNeededForUnfreeze;

	public float timeOnComet { get; private set; }
	private bool scoredOnCurrentComet;

	public delegate void _OnFrozenFactorChanged(CharacterScript c);
	public event _OnFrozenFactorChanged OnFrozenFactorChanged;

	// Game flow related events
	public float score { get; private set; }

	public delegate void _OnCharacterScoreChanged(CharacterScript c);
	public event _OnCharacterScoreChanged OnCharacterScoreChanged;

	public delegate void _OnCharacterDeath(CharacterScript c);
	public event _OnCharacterDeath OnCharacterDeath;

	void Awake () {
		timeInSpace = 0;
		frozenFactor = 0;
		score = 0;

		ownRigidbody = this.GetComponent<Rigidbody2D> ();
		ownRigidbody.AddForce (initialForce, ForceMode2D.Impulse);

		OnFrozenFactorChanged += InternalOnFrozenFactorChanged;
		OnCharacterScoreChanged += InternalOnCharacterScoreChanged;
	}

	void OnDestroy() {
		OnFrozenFactorChanged -= InternalOnFrozenFactorChanged;
		OnCharacterScoreChanged -= InternalOnCharacterScoreChanged;
	}

	void Update() {
		if (this.isInSpace() == true) {
			timeInSpace += Time.deltaTime;

			float speed = this.velocity().magnitude;
			if (speed >= velocityNeededForUnfreeze) {
				frozenFactor -= unfreezeSpeed * Time.deltaTime;
				if (frozenFactor < 0) frozenFactor = 0;
				if (OnFrozenFactorChanged != null) OnFrozenFactorChanged(this);
			}

		} else {
			this.timeOnComet += Time.deltaTime;

			if (this.scoredOnCurrentComet == false && this.timeOnComet >= attachedComet.timeRequiredForPoint) {
				score++;
				this.scoredOnCurrentComet = true;
				if (OnCharacterScoreChanged != null) OnCharacterScoreChanged(this);
			}

			frozenFactor += attachedComet.freezingFactor * Time.deltaTime;
			if (OnFrozenFactorChanged != null) OnFrozenFactorChanged(this);

			if (frozenFactor >= 1) {
				LaunchIntoSpace();
				if (OnCharacterDeath != null) OnCharacterDeath(this);
			}

			if (Input.anyKeyDown && canReleaseFromComet == true) {
				LaunchIntoSpace();
			}
		}
	}

	void LaunchIntoSpace() {
		cometJoint.connectedBody.GetComponent<Collider2D>().enabled = false;
		Destroy (cometJoint);
		this.attachedComet = null;
		this.ownRigidbody.drag = 0;
		timeInSpace = 0;
		
		if (OnWentIntoSpace != null) OnWentIntoSpace(this);
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (this.isInSpace() == false) {
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

		this.timeOnComet = 0;
		this.scoredOnCurrentComet = false;
		this.attachedComet = cometJoint.connectedBody.GetComponent<CometScript> ();

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
		// Do nothing
	}

	void OnTweenCompleteCometReleaseCooldown() {
		canReleaseFromComet = true;
	}

	void InternalOnFrozenFactorChanged (CharacterScript c) {
		if (c != this)
			return;
		//print (c.frozenFactor);
	}

	void InternalOnCharacterScoreChanged (CharacterScript c) {
		if (c != this)
			return;
		print (score);
	}

}