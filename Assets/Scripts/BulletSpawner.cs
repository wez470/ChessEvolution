using UnityEngine;
using System;

public class BulletSpawner {
    private Player owner;

    // In degrees, clockwise from player direction.
    private float angle;

    // TODO(scott): What are the units here?
    private float speed;
    
    // TODO(scott): What are the units here?
    private float bulletSize;

    public BulletSpawner(Player owner) {
        this.owner = owner;
        angle = 0;
        speed = 40;      // TODO(scott): Find a proper default...
        bulletSize = 10; // TODO(scott): Find a proper default...
    }
    
    public void Fire(){
        GameObject bullet = MonoBehaviour.Instantiate(owner.Bullet);
        bullet.transform.position = owner.transform.position;
    }
}