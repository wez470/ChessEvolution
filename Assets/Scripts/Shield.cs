using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	private bool isEnabled;
	private float lastActivationTime;

	private const float DEFAULT_ENABLED_TIME = 3.0f;
	private const float DEFAULT_ACTIVATION_DELAY = 5.0f;
	
    // Use this for initialization
    void Start () {
		isEnabled = false;
		lastActivationTime = Time.time - DEFAULT_ACTIVATION_DELAY;
    }
    
    // Update is called once per frame
    void Update () {
    
    }

	private bool canEnable(){
		if (lastActivationTime + DEFAULT_ENABLED_TIME >= Time.time) {
			return true;
		} else if (lastActivationTime + DEFAULT_ACTIVATION_DELAY >= Time.time) {
			return false;
		} else {
			return true;
		}
	}

	public void enabled(bool isShieldButtonDown){
		if (!isEnabled && isShieldButtonDown && canEnable ()) {
			lastActivationTime = Time.time;
			isEnabled = isShieldButtonDown;
		} else if (isEnabled && isShieldButtonDown && canEnable ()) {
			isEnabled = isShieldButtonDown;
		} else {
			isEnabled = false;
		}

		gameObject.SetActive (isEnabled);
	}
}
