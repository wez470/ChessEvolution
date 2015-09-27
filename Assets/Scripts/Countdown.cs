using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Countdown : MonoBehaviour {
    public int countdownStart = 122;
	
    private int timeLeft;

    void Start() {
        timeLeft = countdownStart;
    }

    void Update () {
        timeLeft = (int)(countdownStart - Time.realtimeSinceStartup);
        int minutesLeft = timeLeft / 60;
        int secondsLeft = timeLeft % 60;
        string timeToDisplay = minutesLeft.ToString("00") + ":" + secondsLeft.ToString("00");
        GetComponent<Text>().text = timeToDisplay;
	}

    public float GetPercentMatchDone() {
        return 100.0f - ((float)timeLeft / countdownStart * 100.0f);
    }
}
