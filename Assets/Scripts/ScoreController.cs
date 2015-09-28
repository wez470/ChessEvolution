using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ScoreController : MonoBehaviour {
    public PlayerManager playerManager;

    private List<Score> scores = new List<Score>();
    private bool initialized = false;

    void Awake() {
        if (!initialized) {
            foreach (Score score in GetComponentsInChildren<Score>()) {
                scores.Add(score);
            }
            for (int i = 3; i >= playerManager.GetNumPlayers(); i--) {
                scores[i].gameObject.SetActive(false);
            }
            initialized = true;
        }
    }

    public void SetColor(int playerNum, Color color) {
        if (!initialized) {
            Awake();
        }
        scores[playerNum - 1].SetColor(color);
    }

    public void IncreaseScore(int playerNum) {
        scores[playerNum - 1].IncreaseScore();
    }

    public void DisplayWinner() {
        int maxScore = scores[0].PlayerScore;
        int maxIndex = 0;
        for (int i = 0; i < scores.Count; i++) {
            if (scores[i].PlayerScore > maxScore) {
                maxScore = scores[i].PlayerScore;
                maxIndex = i;
            }
        }

        WinningScore winningScore = GetComponentInChildren<WinningScore>();
        winningScore.SetColor(scores[maxIndex].Color);
        winningScore.SetScore(maxScore);
    }
}
