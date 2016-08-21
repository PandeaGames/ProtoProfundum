using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class RoachDarkController  : StateBehaviour  
{
	//Declare which states we'd like use
	public enum RoachDarkStates
	{
		Lit, 
		Unlit,
		Disabled
	}

	public GameObject hero;
	public GameObject roachPrefab;
	public float spawnRate = 2;//seconds
	public float spawnRadius = 2;

	private DomeStateController _dsc;
	private RoachController controller;
	private double nextSpawn = 0;
	private Vector3 spawnOffset = new Vector3(0, 6, 0);

	private Game _game;
	// Use this for initialization
	void Start () 
	{
		_game = FindObjectOfType<Game> ();
		controller = GetComponent<RoachController>();
		_dsc = FindObjectOfType<DomeStateController> ();
	}

	void Awake()
	{
		Initialize<RoachDarkStates>();
		
		//Change to our first state
		ChangeState(RoachDarkStates.Lit);
	}
	
	// Update is called once per frame
	void Unlit_Update () 
	{
		if (controller.isLit (hero.transform.position)) {
			ChangeState (RoachDarkStates.Lit);
		} else {
			if(Time.time > nextSpawn && !_dsc.IsInsideDome)
			{
				nextSpawn = Time.time + spawnRate;
				SpawnRoach();
			}
		}
	}
	private void SpawnRoach()
	{
		Vector3 pos = new Vector3 (
			hero.transform.position.x + (Random.Range(0, spawnRadius) - Random.Range (0, spawnRadius)) + spawnOffset.x, 
			hero.transform.position.y + (Random.Range(0, spawnRadius) - Random.Range (0, spawnRadius)) + spawnOffset.y, 
			hero.transform.position.z + (Random.Range(0, spawnRadius) - Random.Range (0, spawnRadius)) + spawnOffset.z);
		GameObject roach = (GameObject)Instantiate(roachPrefab, pos, roachPrefab.transform.rotation);
		if (_game != null) {
			roach.gameObject.transform.parent = _game.gameObject.transform;
		}

	}
	// Update is called once per frame
	void Lit_Update () 
	{
		if (!controller.isLit (hero.transform.position)) {
			ChangeState (RoachDarkStates.Unlit);
			Debug.Log ("UNLIT");
		}
	}
	void ClearSceneData()
	{

	}
}
