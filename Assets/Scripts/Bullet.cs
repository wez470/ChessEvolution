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
	void Start() {
        IgnoreCollisions();
	}
	
	// Update is called once per frame
	void Update() {
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
                MakeSplitClone(angle);
                angle += stepAngle;
            }
            
            // Destroy the current bullet.
            Destroy(gameObject);
        }
    }

    // Angle in degrees.
    public void MakeSplitClone(float angle) {
        GameObject bullet = MonoBehaviour.Instantiate(Owner.Bullet);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Owner = Owner;
        bulletScript.CurveSpeed = CurveSpeed;
        bulletScript.NumSplitBullets = NumSplitBullets;
        bulletScript.SplitsLeft = SplitsLeft - 1;
        bulletScript.SplitDelay = SplitDelay;
        bulletScript.LastSplitTime = Time.time;
        
        bullet.transform.position = transform.position;
        bullet.transform.localScale = transform.localScale;
        
        bullet.GetComponent<SpriteRenderer>().color = Owner.Color;
        bullet.GetComponent<BulletParticles>().SetBulletColor(Owner.Color);

        Vector2 oldVelocity = GetComponent<Rigidbody2D>().velocity;
        float oldAngle = Mathf.Atan2(oldVelocity.y, oldVelocity.x);

        float newAngle = oldAngle + angle * Mathf.Deg2Rad;
        Vector2 newVelocity = new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle));

        bullet.GetComponent<Rigidbody2D>().velocity = newVelocity  * oldVelocity.magnitude;
    }
    
    public void IgnoreCollisions() {
        // Ignore collisions between the bullet and the player who fired it.
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), Owner.GetComponent<Collider2D>(), true);
        Collider2D[] childColliders = Owner.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D childCollider in childColliders){
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), childCollider, true);
        }
    }
}
