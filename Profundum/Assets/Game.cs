using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public string respawnScene = "";
	public float deathSequenceLength = 8;
	// Use this for initialization
	private bool _deathSequence = false;

	private PlayerHealthController playerHealth;
	private float _timeStamp;
	void Start () {
		playerHealth = FindObjectOfType<PlayerHealthController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerHealth.death && !_deathSequence) 
		{
			_deathSequence = true;
			_timeStamp = Time.time +deathSequenceLength;
		}
		if (_deathSequence && Time.time>_timeStamp) 
		{
			Application.LoadLevel(respawnScene);
		}
	}
}
