using UnityEngine;
using System;

public class BulletSpawner {
    private Player owner;

    // In degrees, clockwise from player direction.
    private float angle;

    // Time in seconds since the last time this was fired.
    private float lastFiredTime;

    // Minimum time (in seconds) between successive firings.
    private float fireDelay;

    // Speed of the spawned bullets.
    private float speed;    

    // Scale multiplier of the spawned bullets.
    private float bulletSize;

    public BulletSpawner(Player owner) {
        this.owner = owner;
        lastFiredTime = 0;
        angle = 0;
        fireDelay = 0.25f;
        speed = 8;
        bulletSize = 1;
    }
    
    public void Fire() {
        // Don't fire if we fired within fireDelay seconds ago.
        if (lastFiredTime + fireDelay >= Time.time) {
            return;
        }

        lastFiredTime = Time.time;
        GameObject bullet = MonoBehaviour.Instantiate(owner.Bullet);

        // Ignore collisions between the bullet and the player who fired it.
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), owner.GetComponent<Collider2D>(), true);

        bullet.transform.position = owner.transform.position;
        bullet.transform.localScale *= bulletSize;

        float angle = (owner.transform.eulerAngles.z + 90.0f) * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
        bullet.GetComponent<Rigidbody2D>().velocity = velocity;
    }
}