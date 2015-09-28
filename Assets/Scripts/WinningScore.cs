using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinningScore : MonoBehaviour {
	void Start () {
        GetComponent<Text>().text = "";
	}

    public void SetColor(Color color) {
        GetComponent<Text>().color = color;
    }

    public void SetScore(int score) {
        GetComponent<Text>().text = score.ToString();
    }
}
