using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public string respawnScene = "";
	public float deathSequenceLength = 8;
	// Use this for initialization
	private bool _deathSequence = false;

	private PlayerHealthController playerHealth;
	private float _timeStamp;
	public float timeUntilFade = 5;
	private float deathTime;
	private Cinematics _cinematics;
	private MainCameraMovement _camMovement;
	void Start () {
		playerHealth = FindObjectOfType<PlayerHealthController> ();
		_camMovement = FindObjectOfType<MainCameraMovement> ();
		_cinematics = FindObjectOfType<Cinematics> ();

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
            BroadcastMessage("SceneReset");
            BroadcastMessage("ClearSceneData");
			Application.LoadLevel(respawnScene);
		}
		if (_deathSequence) {
			if(!_cinematics.faded && Time.time > _timeStamp - timeUntilFade)
			{
				_cinematics.FadeOut();
			}
		}
	}
}
