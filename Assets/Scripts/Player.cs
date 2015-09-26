using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private const float ROT_DEAD_ZONE = 0.2f;

	public float Speed;

	void Start () {
	}
	
	void Update () {
	}

	void FixedUpdate() {
		setMovement();
		setRotation();
	}
	
	private void setMovement() {
		float speedX = Input.GetAxis("L_XAxis_1") * Speed;
		float speedY = -Input.GetAxis("L_YAxis_1") * Speed;
		GetComponent<Rigidbody2D>().AddForce(new Vector2(speedX, speedY));
	}
	
	private void setRotation() {
		float rotX = Input.GetAxis("R_XAxis_1");
		float rotY = Input.GetAxis("R_YAxis_1");
		float angle = Mathf.Atan2(-rotY, rotX) * Mathf.Rad2Deg - 90f;

		if(Mathf.Abs(rotX) > ROT_DEAD_ZONE || Mathf.Abs(rotY) > ROT_DEAD_ZONE) {
			transform.rotation = Quaternion.Euler(0, 0, angle);
			GetComponent<Rigidbody2D>().angularVelocity = 0;
		}
	}
}