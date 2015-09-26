using System;

public class BulletSpawner {
	// In degrees, clockwise from player direction.
	float angle;

	// TODO(scott): What are the units here?
	float speed;
	
	// TODO(scott): What are the units here?
	float bulletSize;

	public BulletSpawner() {
		angle = 0;
		speed = 40; 	 // TODO(scott): Find a proper default...
		bulletSize = 10; // TODO(scott): Find a proper default...
	}
	
	public void Fire(Player player){
		
	}
}