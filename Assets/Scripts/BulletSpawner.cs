using UnityEngine;
using System;

public class BulletSpawner {
    private const float DEFAULT_BULLET_SIZE   = 2.0f;
    private const float DEFAULT_CURVE_SPEED   = 0.0f;
    private const int   DEFAULT_SPLIT_BULLETS = 2;
    private const int   DEFAULT_SPLITS        = 0;
    private const float DEFAULT_SPLIT_DELAY   = 0.5f;

    private float angle;      // In degrees, clockwise from player direction.
    private float bulletSize; // Scale multiplier of the spawned bullets.
    private float curveSpeed;
    private int numSplitBullets;
    private int numSplits;
    private float splitDelay;

    public BulletSpawner() {
        angle = 0;
        bulletSize = DEFAULT_BULLET_SIZE;
        numSplitBullets = DEFAULT_SPLIT_BULLETS;
        numSplits = DEFAULT_SPLITS;
        splitDelay = DEFAULT_SPLIT_DELAY;
    }

    public static BulletSpawner Random() {
        BulletSpawner bs = new BulletSpawner();

        bs.angle = RandomDistributions.RandNormal(0, 45);
        bs.bulletSize = RandomDistributions.RandNormal(DEFAULT_BULLET_SIZE, 0.4f);

        bs.curveSpeed = RandomDistributions.RandNormal(DEFAULT_CURVE_SPEED, 10.0f);
        if (Mathf.Abs(bs.curveSpeed) < 15.0f) {
            bs.curveSpeed = 0;
        }

        if (UnityEngine.Random.value > 0.85) {
            bs.numSplits = Mathf.RoundToInt(RandomDistributions.RandNormal(2, 0.55f));
            bs.numSplitBullets = Mathf.RoundToInt(RandomDistributions.RandNormal(2, 0.55f));
            bs.splitDelay = RandomDistributions.RandNormal(DEFAULT_SPLIT_DELAY, 0.2f);
            
            if (bs.numSplits < 1 || bs.numSplitBullets < 2 || bs.splitDelay <= 0.0f) {
                bs.numSplitBullets = DEFAULT_SPLIT_BULLETS;
                bs.numSplits = DEFAULT_SPLITS;
                bs.splitDelay = DEFAULT_SPLIT_DELAY;
            }
        }

        return bs;
    }
    
    public void Fire(Player owner, Weapon weapon) {
        GameObject bullet = MonoBehaviour.Instantiate(owner.Bullet);
        
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Owner = owner;
        bulletScript.CurveSpeed = curveSpeed;
        bulletScript.NumSplitBullets = numSplitBullets;
        bulletScript.SplitsLeft = numSplits;
        bulletScript.SplitDelay = splitDelay;
        bulletScript.LastSplitTime = Time.time;
        
        bulletScript.IgnoreCollisions();

        bullet.transform.position = owner.transform.position;
        bullet.transform.localScale *= bulletSize;

        float angle = (owner.transform.eulerAngles.z + this.angle + 90.0f) * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * weapon.Speed;
        bullet.GetComponent<Rigidbody2D>().velocity = velocity;

        bullet.GetComponent<SpriteRenderer>().color = owner.Color;
        bullet.GetComponent<BulletParticles>().SetBulletColor(owner.Color);
    }
    
    public float GetStrength(Weapon weapon) {
        float numBullets = Mathf.Pow(numSplitBullets, numSplits);
        if (numBullets < 1) {
            numBullets = 1;
        }

        float strength =
            (weapon.Speed     - Weapon.DEFAULT_SPEED)      / Weapon.DEFAULT_SPEED      * 1.0f +
            (weapon.FireDelay - Weapon.DEFAULT_FIRE_DELAY) / Weapon.DEFAULT_FIRE_DELAY * -1.0f +
            (bulletSize       - DEFAULT_BULLET_SIZE)       / DEFAULT_BULLET_SIZE       * 1.0f;

        if (numSplits > 1) {
            strength += (splitDelay - DEFAULT_SPLIT_DELAY) / DEFAULT_SPLIT_DELAY * -1.0f;
        }

        return Mathf.Max(numBullets * strength, 1.0f);
    }
}