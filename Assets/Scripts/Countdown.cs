using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Countdown : MonoBehaviour {
    public int countdownStart = 122;
	
    private int timeLeft;

    void Start() {
        timeLeft = countdownStart;
    }

	public bool gameOver(){
		return (int)Time.timeScale == 0;
	}

    void Update () {
        timeLeft = (int)(countdownStart - Time.realtimeSinceStartup);
		if (timeLeft >= 0) {
			int minutesLeft = timeLeft / 60;
			int secondsLeft = timeLeft % 60;
			string timeToDisplay = minutesLeft.ToString ("00") + ":" + secondsLeft.ToString ("00");
			GetComponent<Text> ().text = timeToDisplay;
		} else {
			Time.timeScale = 0.0f;
		}
		Debug.Log(gameOver());
	}

    public float GetPercentMatchDone() {
        return 100.0f - ((float)timeLeft / countdownStart * 100.0f);
    }
}
