using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	private float lastActivationTime;

	private const float DEFAULT_ENABLED_TIME = 3.0f;
	private const float DEFAULT_ACTIVATION_DELAY = 6.0f;
	
    // Use this for initialization
    void Start () {
		lastActivationTime = Time.time - DEFAULT_ACTIVATION_DELAY;
    }
    
    // Update is called once per frame
    void Update () {
    
    }

	private bool canEnable(){
		if (lastActivationTime + DEFAULT_ENABLED_TIME >= Time.time) {
			return true;
		} else if (lastActivationTime + DEFAULT_ACTIVATION_DELAY > Time.time) {
			return false;
		} else {
			return true;
		}
	}

	public bool enabled(bool isShieldButtonDown){
		if(isShieldButtonDown && lastActivationTime + DEFAULT_ACTIVATION_DELAY < Time.time ){
			lastActivationTime = Time.time;
		}

		if (isShieldButtonDown && canEnable()) {
			gameObject.SetActive (true);
			return true;
		} else {
			gameObject.SetActive (false);
			return false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
	
	}
}
