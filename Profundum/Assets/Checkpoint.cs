using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	private Game _game;
	public string sceneToLoad;
	// Use this for initialization
	void Start () {
		_game = FindObjectOfType<Game> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			_game.respawnScene = sceneToLoad;
		}
	}
}
