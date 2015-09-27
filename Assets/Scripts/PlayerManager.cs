using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {
    public GameObject Player;

    private int numPlayers = 0;
    private List<GameObject> players = new List<GameObject>();

    private Color[] colors = {Color.green, Color.cyan, Color.magenta, Color.red};
    
    
    void Awake () {
        foreach(String joystick in Input.GetJoystickNames()) {
            // make sure device is a controller
            if(joystick.Contains("Controller")) {
                numPlayers++;
            }
        }
        SetupPlayers();
    }
    
    //Create a player instance foreach controller detected
    private void SetupPlayers() {
        for(int i = 1; i <= numPlayers; i++) {
            CreatePlayer(i);
        }
    }
    
    // Assigns a controller and tag value foreach player
    private void CreatePlayer(int playerNum) {
        GameObject newPlayer = Instantiate(Player) as GameObject;
		newPlayer.transform.position = getPlayerPosition(playerNum);
        Player player = newPlayer.gameObject.GetComponent<Player>();
       	player.SetColor ( colors[playerNum - 1] );
        SetupNewPlayerController(playerNum, newPlayer);
        newPlayer.tag = "Player" + playerNum;
        players.Add(newPlayer);
    }
    
	private Vector3 getPlayerPosition(int playerNum){
		switch (playerNum) {
			case (1):{return new Vector3 (-12.0f, 5.0f, 1.0f);}
			case (2):{return new Vector3 (12.0f, -5.0f, 1.0f);}
			case (3): {return new Vector3(12.0f, 5.0f, 1.0f);}
			case (4): {return new Vector3(-12.0f, -5.0f, 1.0f);}
			default: {return new Vector3(0.0f, 0.0f, 1.0f);}
		}
	}

    private void SetupNewPlayerController(int playerNum, GameObject player) 
    {
        player.GetComponent<Player>().SetInput(new PlayerXboxController(playerNum));
    }
    
    public int GetNumPlayers() {
        return numPlayers;
    }
    
    public List<GameObject> GetPlayers() {
        return players;
    }
}
