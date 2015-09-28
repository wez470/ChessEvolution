using UnityEngine;
using UnityEngine.UI;
using System.Collections; 
using System;

public class Countdown : MonoBehaviour {
    public int countdownStart = 120;
    public ScoreController scoreController;
	
    private int timeLeft;

    void Start() {
        timeLeft = countdownStart;
    }

	public bool gameOver(){
		return (int)Time.timeScale == 0;
	}

    void Update () {
        if (countdownStart - (int)Time.timeSinceLevelLoad >= 0) {
            timeLeft = countdownStart - (int)Time.timeSinceLevelLoad;
    		int minutesLeft = timeLeft / 60;
    		int secondsLeft = timeLeft % 60;
    		string timeToDisplay = minutesLeft.ToString ("00") + ":" + secondsLeft.ToString ("00");
    		GetComponent<Text>().text = timeToDisplay;
        }
        else {
            scoreController.DisplayWinner();
            Invoke("ReloadLevel", 10f);
        }

        if(timeLeft == 5) {
            Vector3 pos = GetComponent<RectTransform>().position;
            pos.y = 0;
            GetComponent<RectTransform>().position = pos;
            GetComponent<Text>().fontSize = 300;
        }
	}

    void ReloadLevel() {
        Application.LoadLevel(Application.loadedLevelName);
    }

    public float GetPercentMatchDone() {
        return 100.0f - ((float)timeLeft / countdownStart * 100.0f);
    }
}
