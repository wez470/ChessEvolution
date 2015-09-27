using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {
    private int score = 0;
    private Text uiText;

    void Start() {
        uiText = GetComponent<Text>();
    }
	
    void Update() {
        uiText.text = score.ToString();
    }

    public void SetColor(Color color) {
        uiText = GetComponent<Text>();
        uiText.color = color;
    }

    public void IncreaseScore() {
        score++;
    }
}
