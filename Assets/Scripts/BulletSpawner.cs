using UnityEngine;
using System;

public class BulletSpawner {
    private const float DEFAULT_FIRE_DELAY = 0.5f;
    private const float DEFAULT_SPEED = 8;
    private const float DEFAULT_BULLET_SIZE = 1;

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
        fireDelay = DEFAULT_FIRE_DELAY;
        speed = DEFAULT_SPEED;
        bulletSize = DEFAULT_BULLET_SIZE;
    }

    private static float randNormal(float mean, float stdDev) {
        float u1 = UnityEngine.Random.value; //these are uniform(0,1) random doubles
        float u2 = UnityEngine.Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
            Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)
        return mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
    }

    public static BulletSpawner Random(Player owner) {
        BulletSpawner bs = new BulletSpawner(owner);

        bs.angle = randNormal(0, 65);
        bs.speed = randNormal(DEFAULT_SPEED, 1);
        bs.bulletSize = randNormal(DEFAULT_BULLET_SIZE, 0.2f);
        bs.fireDelay = randNormal(DEFAULT_FIRE_DELAY, 0.05f);

        return bs;
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

        float angle = (owner.transform.eulerAngles.z + this.angle + 90.0f) * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
        bullet.GetComponent<Rigidbody2D>().velocity = velocity;
    }
    
    public float GetStrength() {
        float strength =
            (speed - DEFAULT_SPEED) / DEFAULT_SPEED * 1.0f +
            (fireDelay - DEFAULT_FIRE_DELAY) / DEFAULT_FIRE_DELAY * 1.0f +
            (bulletSize - DEFAULT_BULLET_SIZE) / DEFAULT_BULLET_SIZE * 1.0f;

        return Mathf.Max(strength, 1.0f);
    }
}