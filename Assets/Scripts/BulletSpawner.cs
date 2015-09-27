using UnityEngine;
using System;

public class BulletSpawner {
    private const float DEFAULT_BULLET_SIZE = 2.0f;
    private const float DEFAULT_CURVE_SPEED = 0.0f;

    private float angle;      // In degrees, clockwise from player direction.
    private float bulletSize; // Scale multiplier of the spawned bullets.
    private float curveSpeed;

    public BulletSpawner() {
        angle = 0;
        bulletSize = DEFAULT_BULLET_SIZE;
    }

    public static BulletSpawner Random() {
        BulletSpawner bs = new BulletSpawner();

        bs.angle = RandomDistributions.RandNormal(0, 45);
        bs.bulletSize = RandomDistributions.RandNormal(DEFAULT_BULLET_SIZE, 0.4f);

        bs.curveSpeed = RandomDistributions.RandNormal(DEFAULT_CURVE_SPEED, 10.0f);
        if (Mathf.Abs(bs.curveSpeed) < 15.0f) {
            bs.curveSpeed = 0;
        }

        return bs;
    }
    
    public void Fire(Player owner, Weapon weapon) {
        GameObject bullet = MonoBehaviour.Instantiate(owner.Bullet);
        
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Owner = owner;
        bulletScript.CurveSpeed = curveSpeed;
        bulletScript.NumSplitBullets = 2;
        bulletScript.SplitsLeft = 1;
        bulletScript.SplitDelay = 0.50f;
        bulletScript.LastSplitTime = Time.time;

        bullet.transform.position = owner.transform.position;
        bullet.transform.localScale *= bulletSize;

        float angle = (owner.transform.eulerAngles.z + this.angle + 90.0f) * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * weapon.Speed;
        bullet.GetComponent<Rigidbody2D>().velocity = velocity;

        bullet.GetComponent<SpriteRenderer>().color = owner.Color;
        bullet.GetComponent<BulletParticles>().SetBulletColor(owner.Color);
    }
    
    public float GetStrength(Weapon weapon) {
        float strength =
            (weapon.Speed     - Weapon.DEFAULT_SPEED)      / Weapon.DEFAULT_SPEED      * 1.0f +
            (weapon.FireDelay - Weapon.DEFAULT_FIRE_DELAY) / Weapon.DEFAULT_FIRE_DELAY * 1.0f +
            (bulletSize       - DEFAULT_BULLET_SIZE)       / DEFAULT_BULLET_SIZE       * 1.0f;

        return Mathf.Max(strength, 1.0f);
    }
}