using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
}
