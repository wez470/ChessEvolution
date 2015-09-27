using UnityEngine;
using System.Collections;

public class BulletParticles : MonoBehaviour {
	
	public ParticleSystem shieldExplosion;
	public ParticleSystem shipExplosion;
	public ParticleSystem wallExplosion;
	
	private Color color;
	
	public float COLLIDER_RADIUS = 0.09f;
	
	void OnTriggerEnter2D( Collider2D other ){
		if (other.gameObject.tag == "Shield" ){
			
			createAndDestroyShieldExplosion(other.transform.position);
			this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
		}
		else if (other.gameObject.tag == "Walls" || other.gameObject.tag == "Drop"){
			shieldExplosion.transform.position = gameObject.transform.position;
			shieldExplosion.startColor = color;
			Instantiate ( shieldExplosion );
			Destroy (this.gameObject);
		}
		else if (	other.gameObject.tag == "Player1" 
		         || other.gameObject.tag == "Player2"
		         || other.gameObject.tag == "Player3"
		         || other.gameObject.tag == "Player4"){
			shipExplosion.transform.position = gameObject.transform.position;
			shipExplosion.startColor = other.gameObject.GetComponent<Player>().Color;
			Instantiate (shipExplosion);
			Destroy (this.gameObject);
		}
	}
	
	void createAndDestroyShieldExplosion(Vector3 otherPosition){
		shieldExplosion.transform.position = Vector3.MoveTowards(otherPosition, this.transform.position, COLLIDER_RADIUS);
		shieldExplosion.startColor = color;
		Instantiate ( shieldExplosion );
		Destroy ( this.gameObject );
	}
	
	public void SetBulletColor (Color color){
		this.color = color;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
