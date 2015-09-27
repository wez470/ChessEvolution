using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Weapon {
    public const float DEFAULT_FIRE_DELAY = 0.75f;
    public const float DEFAULT_SPEED = 8;

    public Player Owner;
    public float Speed;     // Speed of the spawned bullets.
    public float FireDelay; // Minimum time (in seconds) between successive firings.
    
    // Time in seconds since the last time this was fired.
    private float lastFiredTime;
    
    private const float MORPH_STRENGTH_INCREASE = 1.0f;
    private const float CHANCE_TO_KEEP_WEAPON = 0.90f; // chance that you own bullet spawner is included in new weapon
    private const float CHANCE_TO_GAIN_WEAPON = 0.50f; // chance that other's bullet spawner is included in new weapon
    
    private List<BulletSpawner> bulletSpawners;

    public Weapon(Player owner) {
        Owner = owner;
        Speed = DEFAULT_SPEED;
        FireDelay = DEFAULT_FIRE_DELAY;
        bulletSpawners = new List<BulletSpawner>();
        lastFiredTime = 0;
    }
    
    public static Weapon Default(Player owner) {
        Weapon w = new Weapon(owner);
        w.bulletSpawners = new List<BulletSpawner>{new BulletSpawner(w)};
        return w;
    }

    public static Weapon Random(Player owner, float targetStrength) {
        Weapon w = new Weapon(owner);

        float strength = 0;
        w.bulletSpawners = new List<BulletSpawner>();
        
        w.Speed = RandomDistributions.RandNormal(DEFAULT_SPEED, 1);
        w.FireDelay = RandomDistributions.RandNormal(DEFAULT_FIRE_DELAY, 0.05f);
        
        while (strength < targetStrength) {
            BulletSpawner bulletSpawner = BulletSpawner.Random(w);
            strength += bulletSpawner.GetStrength();
            w.bulletSpawners.Add(bulletSpawner);
        }

        return w;
    }

    public void Fire() {
        // Don't fire if we fired within fireDelay seconds ago.
        if ((Time.time - lastFiredTime) < FireDelay) {
            return;
        }
        
        lastFiredTime = Time.time;

        foreach (BulletSpawner bulletSpawner in bulletSpawners) {
            bulletSpawner.Fire();
        }
    }
    
    public static Weapon Morph(Weapon mine, Weapon other) {
        Weapon combined = new Weapon(mine.Owner);

        float targetStrength = Mathf.Max(mine.GetStrength(), other.GetStrength()) + MORPH_STRENGTH_INCREASE;
        float strength = 0;
        
        foreach (BulletSpawner bs in mine.bulletSpawners) {
            if (UnityEngine.Random.value < CHANCE_TO_KEEP_WEAPON) {
                strength += bs.GetStrength();
                combined.bulletSpawners.Add(bs);
            }
        }
        
        foreach (BulletSpawner bs in other.bulletSpawners) {
            if (UnityEngine.Random.value < CHANCE_TO_GAIN_WEAPON) {
                bs.SetOwningWeapon(combined);
                strength += bs.GetStrength();
                combined.bulletSpawners.Add(bs);
            }
        }

        while (strength < targetStrength) {
            BulletSpawner bulletSpawner = BulletSpawner.Random(mine);
            strength += bulletSpawner.GetStrength();
            combined.bulletSpawners.Add(bulletSpawner);
        }

        return combined;
    }

    public float GetStrength() {
        return bulletSpawners.Select(bs => bs.GetStrength()).Sum();
    }
}
