using UnityEngine;
using System.Collections;

public class DropSpawnerController : MonoBehaviour {

	public GameObject growPost;

	private const float DEFAULT_DROP_TIMER = 5.0f;
	private const int DEFAULT_MAX_DROPS = 4;
	private const float BOUNDARY_WIDTH = 20.0f;
	private const float BOUNDARY_HEIGHT = 12.0f;
	
	private float lastDropTime;
	private GameObject[] drop_options;
	private GameObject[] curr_drops;

	// Use this for initialization
	void Start () {
		lastDropTime = Time.time;
		drop_options = new GameObject[]{growPost};
	}
	
	// Update is called once per frame
	void Update () {
		spawnNewDrop();
	}

	private void spawnNewDrop(){
		curr_drops = GameObject.FindGameObjectsWithTag("Drop");
		if(curr_drops.Length < DEFAULT_MAX_DROPS && Time.time - lastDropTime >= DEFAULT_DROP_TIMER){
			spawnRandomDrop();
		}
	}

	private void spawnRandomDrop(){
		lastDropTime = Time.time;
		float randX = Random.value*BOUNDARY_WIDTH - BOUNDARY_WIDTH/2.0f;
		float randY = Random.value*BOUNDARY_HEIGHT - BOUNDARY_HEIGHT/2.0f;
		int randomDropIndex = (int)Random.Range (0, drop_options.Length - 1);
		Instantiate (drop_options [randomDropIndex], new Vector3 (randX, randY, 0.0f), Quaternion.identity);
	}
}
