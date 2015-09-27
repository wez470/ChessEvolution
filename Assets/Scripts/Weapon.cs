using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Weapon {
    private const float MORPH_STRENGTH_INCREASE = 1.0f;
    private const float CHANCE_TO_KEEP_WEAPON = 0.90f; // chance that you own bullet spawner is included in new weapon
    private const float CHANCE_TO_GAIN_WEAPON = 0.50f; // chance that other's bullet spawner is included in new weapon

    private Player owner;
    private List<BulletSpawner> bulletSpawners;

    public Weapon(Player owner) {
        this.owner = owner;
        this.bulletSpawners = new List<BulletSpawner>();
    }
    
    public static Weapon Default(Player owner, float targetStrength) {
        Weapon w = new Weapon(owner);
        w.bulletSpawners = new List<BulletSpawner>{new BulletSpawner(owner)};
        return w;
    }

    public static Weapon Random(Player owner, float targetStrength) {
        Weapon w = new Weapon(owner);

        float strength = 0;
        w.bulletSpawners = new List<BulletSpawner>();

        while (strength < targetStrength) {
            BulletSpawner bulletSpawner = BulletSpawner.Random(owner);
            strength += bulletSpawner.GetStrength();
            w.bulletSpawners.Add(bulletSpawner);
        }

        return w;
    }

    public void Fire() {
        foreach (BulletSpawner bulletSpawner in bulletSpawners) {
            bulletSpawner.Fire();
        }
    }
    
    public static Weapon Morph(Weapon mine, Weapon other) {
        Weapon combined = new Weapon(mine.owner);

        float targetStrength = Mathf.Max(mine.GetStrength(), other.GetStrength()) + MORPH_STRENGTH_INCREASE;
        float strength = 0;
        
        foreach (BulletSpawner bs in mine.bulletSpawners) {
            if (UnityEngine.Random.value > CHANCE_TO_KEEP_WEAPON) {
                strength += bs.GetStrength();
                combined.bulletSpawners.Add(bs);
            }
        }
        
        foreach (BulletSpawner bs in other.bulletSpawners) {
            if (UnityEngine.Random.value > CHANCE_TO_GAIN_WEAPON) {
                strength += bs.GetStrength();
                combined.bulletSpawners.Add(bs);
            }
        }

        while (strength < targetStrength) {
            BulletSpawner bulletSpawner = BulletSpawner.Random(mine.owner);
            strength += bulletSpawner.GetStrength();
            combined.bulletSpawners.Add(bulletSpawner);
        }

        return combined;
    }

    public float GetStrength() {
        return bulletSpawners.Select(bs => bs.GetStrength()).Sum();
    }
}
