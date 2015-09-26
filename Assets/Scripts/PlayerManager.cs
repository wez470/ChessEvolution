using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {
	public GameObject Player;

	private int numPlayers = 0;
	private List<GameObject> players = new List<GameObject>();
	
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
		newPlayer.transform.position = new Vector3(0, 0, 0);
		SetupNewPlayerController(playerNum, newPlayer);
		players.Add(newPlayer);
	}
	
	private void SetupNewPlayerController(int playerNum, GameObject player) 
	{
		//player.GetComponent<Player>().SetInput(new PlayerXboxController(playerNum));
	}
	
	public int GetNumPlayers() {
		return numPlayers;
	}
	
	public List<GameObject> GetPlayers() {
		return players;
	}
}
