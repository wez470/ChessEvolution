using UnityEngine;
using System;

public class BulletSpawner {
    private const float DEFAULT_BULLET_SIZE = 2.0f;

    private Weapon weapon;

    // In degrees, clockwise from player direction.
    private float angle;

    // Scale multiplier of the spawned bullets.
    private float bulletSize;

    public BulletSpawner(Weapon weapon) {
        this.weapon = weapon;
        angle = 0;
        bulletSize = DEFAULT_BULLET_SIZE;
    }

    public static BulletSpawner Random(Weapon weapon) {
        BulletSpawner bs = new BulletSpawner(weapon);

        bs.angle = RandomDistributions.RandNormal(0, 45);
        bs.bulletSize = RandomDistributions.RandNormal(DEFAULT_BULLET_SIZE, 0.4f);

        return bs;
    }
    
    public void SetOwningWeapon(Weapon weapon) {
        this.weapon = weapon;
    }
    
    public void Fire() {
        GameObject bullet = MonoBehaviour.Instantiate(weapon.Owner.Bullet);

        // Ignore collisions between the bullet and the player who fired it.
			Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), weapon.Owner.GetComponent<Collider2D>(), true);
		Collider2D[] childColliders = weapon.Owner.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D childCollider in childColliders){
        	Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), childCollider, true);
        }

        bullet.transform.position = weapon.Owner.transform.position;
        bullet.transform.localScale *= bulletSize;

        float angle = (weapon.Owner.transform.eulerAngles.z + this.angle + 90.0f) * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * weapon.Speed;
        bullet.GetComponent<Rigidbody2D>().velocity = velocity;
    }
    
    public float GetStrength() {
        float strength =
            (weapon.Speed     - Weapon.DEFAULT_SPEED)      / Weapon.DEFAULT_SPEED      * 1.0f +
            (weapon.FireDelay - Weapon.DEFAULT_FIRE_DELAY) / Weapon.DEFAULT_FIRE_DELAY * 1.0f +
            (bulletSize       - DEFAULT_BULLET_SIZE)       / DEFAULT_BULLET_SIZE       * 1.0f;

        return Mathf.Max(strength, 1.0f);
    }
}