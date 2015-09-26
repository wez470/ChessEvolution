using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private const float ROT_DEAD_ZONE = 0.2f;

	public float Speed;

	private InputController input;

	private Shield playerShield;

	void Start () {
		playerShield = GetComponentInChildren<Shield>();
	}
	
	void Update () {
	}

	void FixedUpdate() {
		setMovement();
		setRotation();
		checkShield();
	}
	
	public void SetInput(InputController inputController) {
		input = inputController;
	}

	private void setMovement() {
		float speedX = Input.GetAxis(input.GetXAxisMovement()) * Speed;
		float speedY = -Input.GetAxis(input.GetYAxisMovement()) * Speed;
		GetComponent<Rigidbody2D>().AddForce(new Vector2(speedX, speedY));
	}
	
	private void setRotation() {
		float rotX = Input.GetAxis(input.GetXAxisRotation());
		float rotY = Input.GetAxis(input.GetYAxisRotation());
		float angle = Mathf.Atan2(-rotY, rotX) * Mathf.Rad2Deg - 90f;

		if(Mathf.Abs(rotX) > ROT_DEAD_ZONE || Mathf.Abs(rotY) > ROT_DEAD_ZONE) {
			transform.rotation = Quaternion.Euler(0, 0, angle);
			GetComponent<Rigidbody2D>().angularVelocity = 0;
		}
	}

	private void checkShield() {
		float leftTrigger = Input.GetAxis(input.GetUseShield());
		bool isLeftTriggerDown = leftTrigger > 0.9f;
		Debug.Log (isLeftTriggerDown);

		// TODO: Disable Shield 
		playerShield.gameObject.SetActive(isLeftTriggerDown);
	}
}