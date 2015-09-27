using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    private const float SPLIT_CONE_ANGLE = 90; // (degrees)

    public float CurveSpeed;
    public float SplitTime;     // The time when this bullet will split.
    public int SplitsLeft;      // Number of times this bullet will split into more bullets.
    public int NumSplitBullets; // The number of bullets this will split into.


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

        if (SplitsLeft > 1 && NumSplitBullets > 1 && Time.time >= SplitTime) {
            float angle = -SPLIT_CONE_ANGLE / 2;
            float stepAngle = SPLIT_CONE_ANGLE / (NumSplitBullets - 1);

            for (int i = 0; i < NumSplitBullets; i++) {
                Bullet b = Instantiate(this);
                b.SplitsLeft = SplitsLeft - 1;
                b.SplitTime = Time.time + 0.5f;

                Rigidbody2D otherRb = b.GetComponent<Rigidbody2D>();
                float bulletAngle = Mathf.Atan2(v.y, v.x);
                float newAngle = bulletAngle - angle;
                otherRb.velocity = new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle)) * rb.velocity.magnitude;

                angle += stepAngle;
            }
            
            // Destroy the current bullet.
            Destroy(gameObject);
        }
    }
}
