using UnityEngine;
using System.Collections;

public class LightRotation : MonoBehaviour {

	public int lightSpeed = 20;

	
	// Update is called once per frame
	void Update () {
		PlayerController playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
		if (!playerScript.paused) {
			transform.RotateAround(Vector3.zero, Vector3.up, lightSpeed * Time.deltaTime);
		}
	}

	public const int DUSK_RANGE = 10;
	public static string getDark(GameObject other) {
		GameObject sun = GameObject.Find("Sun");
		Vector3 direction = (sun.transform.position - other.transform.position).normalized;
		float angle = Vector3.Angle(other.transform.up, direction);
		if (angle <= 90-DUSK_RANGE) {
			return "light";
		} else if (angle < 90 + DUSK_RANGE) {
			return "dusk";
		}
		return "dark";

	}
}
