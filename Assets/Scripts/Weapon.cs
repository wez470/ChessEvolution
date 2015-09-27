using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Weapon {
    public const float DEFAULT_FIRE_DELAY = 0.75f;
    public const float DEFAULT_SPEED = 8;

    public float Speed;     // Speed of the spawned bullets.
    public float FireDelay; // Minimum time (in seconds) between successive firings.
    
    // Time in seconds since the last time this was fired.
    private float lastFiredTime;
    
    private const float MORPH_STRENGTH_INCREASE = 1.0f;
    private const float CHANCE_TO_KEEP_WEAPON = 0.90f; // chance that you own bullet spawner is included in new weapon
    private const float CHANCE_TO_GAIN_WEAPON = 0.50f; // chance that other's bullet spawner is included in new weapon
    
    private List<BulletSpawner> bulletSpawners;

    public Weapon() {
        Speed = DEFAULT_SPEED;
        FireDelay = DEFAULT_FIRE_DELAY;
        bulletSpawners = new List<BulletSpawner>();
        lastFiredTime = 0;
    }
    
    public static Weapon Default() {
        Weapon w = new Weapon();
        w.bulletSpawners = new List<BulletSpawner>{new BulletSpawner()};
        return w;
    }

    public static Weapon Random(float targetStrength) {
        Weapon w = new Weapon();

        float strength = 0;
        w.bulletSpawners = new List<BulletSpawner>();
        
        w.Speed = RandomDistributions.RandNormal(DEFAULT_SPEED, 1);
        w.FireDelay = RandomDistributions.RandNormal(DEFAULT_FIRE_DELAY, 0.05f);
        
        while (strength < targetStrength) {
            BulletSpawner bulletSpawner = BulletSpawner.Random();
            strength += bulletSpawner.GetStrength(w);
            w.bulletSpawners.Add(bulletSpawner);
        }

        return w;
    }

    public void Fire(Player owner) {
        // Don't fire if we fired within fireDelay seconds ago.
        if ((Time.time - lastFiredTime) < FireDelay) {
            return;
        }
        
        lastFiredTime = Time.time;

        foreach (BulletSpawner bulletSpawner in bulletSpawners) {
            bulletSpawner.Fire(owner, this);
        }
    }
    
    public static Weapon Morph(Weapon mine, Weapon other) {
        Weapon combined = new Weapon();

        float targetStrength = Mathf.Max(mine.GetStrength(), other.GetStrength()) + MORPH_STRENGTH_INCREASE;
        float strength = 0;
        
        foreach (BulletSpawner bs in mine.bulletSpawners) {
            if (UnityEngine.Random.value < CHANCE_TO_KEEP_WEAPON) {
                strength += bs.GetStrength(combined);
                combined.bulletSpawners.Add(bs);
            }
        }
        
        foreach (BulletSpawner bs in other.bulletSpawners) {
            if (UnityEngine.Random.value < CHANCE_TO_GAIN_WEAPON) {
                strength += bs.GetStrength(combined);
                combined.bulletSpawners.Add(bs);
            }
        }

        while (strength < targetStrength) {
            BulletSpawner bulletSpawner = BulletSpawner.Random();
            strength += bulletSpawner.GetStrength(combined);
            combined.bulletSpawners.Add(bulletSpawner);
        }

        return combined;
    }

    public float GetStrength() {
        return bulletSpawners.Select(bs => bs.GetStrength(this)).Sum();
    }
}
