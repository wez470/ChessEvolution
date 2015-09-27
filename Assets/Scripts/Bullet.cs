using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public float CurveSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate() {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 v = rb.velocity.normalized;
        Vector2 left = new Vector2(-v.y, v.x);
        rb.AddForce(left * CurveSpeed);
    }
}
