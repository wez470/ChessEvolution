using UnityEngine;
using System.Collections;

public class growPostController : MonoBehaviour {
    private const float DEFAULT_GROW_TIME = 8.0f;
    private const float DEFAULT_GROW_SPEED = 0.0075f;

	private bool growing;
	private bool imploding;
	private float growStartTimer;
    private float growTime;
    private float growSpeed;

	// Use this for initialization
	void Start () {
		growStartTimer = Time.time;
		growing = false;
		imploding = false;
        growTime = RandomDistributions.RandNormal(DEFAULT_GROW_TIME, 2.0f);
        growSpeed = Mathf.Max(
            RandomDistributions.RandNormal(DEFAULT_GROW_SPEED, 0.003f),
            DEFAULT_GROW_SPEED);

        if (Random.value > 0.90) {
            growSpeed *= 3;
            growTime /= 2;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (growing) {
			if(growStartTimer + growTime > Time.time){
                transform.localScale += new Vector3(growSpeed, growSpeed, 1);
			}
			else{
				imploding = true;
				growing = false;
			}
		} else if (imploding) {
            transform.localScale -= new Vector3(growSpeed * 20, growSpeed * 20, 1);
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
