using System.Collections.Generic;

public class Weapon {
	List<BulletSpawner> bulletSpawners;

	public Weapon() {
		bulletSpawners = new List<BulletSpawner>{new BulletSpawner()};
	}

	public void Fire(Player player){

	}
	
	public void Morph(Weapon w){
	
	}

	public float GetStrength() {
		return 0.0f;
	}
}
