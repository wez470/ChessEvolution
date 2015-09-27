using System.Collections.Generic;
using System.Linq;

public class Weapon {
    private Player owner;
    private List<BulletSpawner> bulletSpawners;

    public Weapon(Player owner) {
        this.owner = owner;
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
    
    public void Morph(Weapon w) {
    
    }

    public float GetStrength() {
        return bulletSpawners.Select(bs => bs.GetStrength()).Sum();
    }
}
