using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreController : MonoBehaviour {
    public PlayerManager playerManager;

    private List<Score> scores = new List<Score>();

    void Start() {
        foreach (Score score in GetComponentsInChildren<Score>()) {
            scores.Add(score);
        }
        for (int i = 3; i >= playerManager.GetNumPlayers(); i--) {
            scores[i].gameObject.SetActive(false);
        }
    }

    public void SetColor(int playerNum, Color color) {
        scores[playerNum - 1].SetColor(color);
    }

    public void IncreaseScore(int playerNum) {
        scores[playerNum - 1].IncreaseScore();
    }
}
