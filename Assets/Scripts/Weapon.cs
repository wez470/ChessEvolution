using System.Collections.Generic;

public class Weapon {
    private Player owner;
    private List<BulletSpawner> bulletSpawners;

    public Weapon(Player owner) {
        bulletSpawners = new List<BulletSpawner>{new BulletSpawner(owner)};
    }

    public void Fire() {
        foreach (BulletSpawner bulletSpawner in bulletSpawners) {
            bulletSpawner.Fire();
        }
    }
    
    public void Morph(Weapon w) {
    
    }

    public float GetStrength() {
        return 0.0f;
    }
}
