using UnityEngine;
using System.Collections;

public class ParallaxScript : MonoBehaviour {

	public Vector2 parallaxValue;

	private Vector3 startPosition;

	void Start() {
		startPosition = transform.position;
		parallaxValue.x = Mathf.Clamp (parallaxValue.x, -1, 1);
		parallaxValue.y = Mathf.Clamp (parallaxValue.y, -1, 1);
	}

	void Update () {
		Vector3 characterPosition = GameScript.game.character.transform.position;
		Vector3 newPosition = new Vector3 ();
		newPosition.x = startPosition.x + characterPosition.x * (1 - parallaxValue.x);
		newPosition.y = startPosition.y + characterPosition.y * (1 - parallaxValue.y);

		transform.position = newPosition;
	}
}
