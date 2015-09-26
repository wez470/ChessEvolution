using UnityEngine;
using System;

public class BulletSpawner {
    private Player owner;

    // In degrees, clockwise from player direction.
    private float angle;

    private float fireDelay;
    private float speed;    
    private float bulletSize;

    public BulletSpawner(Player owner) {
        this.owner = owner;
        angle = 0;
        fireDelay = 1;
        speed = 8;
        bulletSize = 1;
    }
    
    public void Fire() {
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