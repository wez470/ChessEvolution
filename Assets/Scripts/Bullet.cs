using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    private const float SPLIT_CONE_ANGLE = 45; // (degrees)

    public Player Owner;
    public float CurveSpeed;
    public float LastSplitTime; // Time when the last bullet split happened.
    public float SplitDelay;    // Amount of time between bullet splits.
    public int SplitsLeft;      // Number of times this bullet will split into more bullets.
    public int NumSplitBullets; // The number of bullets this will split into.

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate() {
        // Perform bullet curving.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 v = rb.velocity.normalized;
        Vector2 left = new Vector2(-v.y, v.x);
        rb.AddForce(left * CurveSpeed);

        // Perform bullet splitting.
        if (SplitsLeft > 0 && NumSplitBullets > 1 && (Time.time - LastSplitTime) >= SplitDelay) {
            float angle = -SPLIT_CONE_ANGLE / 2;
            float stepAngle = SPLIT_CONE_ANGLE / (NumSplitBullets - 1);

            for (int i = 0; i < NumSplitBullets; i++) {
                GameObject bulletObject = Instantiate(gameObject);
                Bullet b = bulletObject.GetComponent<Bullet>();
                b.SplitsLeft = SplitsLeft - 1;
                b.LastSplitTime = Time.time;

                Rigidbody2D otherRb = bulletObject.GetComponent<Rigidbody2D>();
                float bulletAngle = Mathf.Atan2(v.y, v.x);
                float newAngle = bulletAngle + angle * Mathf.Deg2Rad;
                otherRb.velocity = new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle)) * rb.velocity.magnitude;

                angle += stepAngle;
            }
            
            // Destroy the current bullet.
            Destroy(gameObject);
        }
    }
}
