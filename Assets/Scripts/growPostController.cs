using UnityEngine;
using System.Collections;

public class growPostController : MonoBehaviour {

	private bool growing;
	private bool imploding;
	private float growStartTimer;
	private const float GROW_TIME = 10.0f;

	private Vector3 growthVector = new Vector3(0.005f, 0.005f, 1);
	private Vector3 implodeVector = new Vector3(0.1f, 0.1f, 1);
	// Use this for initialization
	void Start () {
		growStartTimer = Time.time;
		growing = false;
		imploding = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (growing) {
			if(growStartTimer + GROW_TIME > Time.time){
				transform.localScale += growthVector;
			}
			else{
				imploding = true;
				growing = false;
			}
		} else if (imploding) {
			transform.localScale -= implodeVector;
		}
		if (transform.localScale.x < 0.0f) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D( Collision2D other ){
		if (other.gameObject.tag.Contains("Player") && !imploding && !growing) {
			growing = true;
			growStartTimer = Time.time;
		}
	}
}
