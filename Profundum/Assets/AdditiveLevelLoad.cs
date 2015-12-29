using UnityEngine;
using System.Collections;

public class AdditiveLevelLoad : MonoBehaviour {

	private Game _game;

	public string sceneToLoad = "";
	public GameObject sceneToDestroy;
	private bool _hasLoadedLevel = false;
	private bool _doDeletion = false;
	private bool _deletionComplete = false;
	// Use this for initialization
	void Start () {
		_game = FindObjectOfType<Game> ();
	}
	
	// Update is called once per frame
	void Update () {

	}
	void LateUpdate()
	{
		if (_deletionComplete) 
		{
			FindObjectOfType<Game>().BroadcastMessage("ResetSceneData");
			Destroy (gameObject);
		}
		if (_doDeletion) {
			LoadLevelAnchorEnd end = FindObjectOfType<LoadLevelAnchorEnd>();
			LoadLevelAnchorStart start = FindObjectOfType<LoadLevelAnchorStart>();

			Vector3 levelOffset = start.transform.position - end.transform.position;

			Destroy (sceneToDestroy);

			LevelAdditive newLevel = FindObjectOfType<LevelAdditive>();
			newLevel.transform.position = newLevel.transform.position - levelOffset;

			newLevel.transform.parent = _game.transform;

			//Tell's all controllers to reset thier lists and static data to account for new level loaded. 
			_deletionComplete = true;
		}
		if (_hasLoadedLevel) {
			_doDeletion = true;
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			if(sceneToLoad!= "")
			{
				_hasLoadedLevel = true;
				//Application.LoadLevel (sceneToLoad);
				Application.LoadLevelAdditive(sceneToLoad);

				_game.BroadcastMessage("ClearSceneData");
			}
		}
	}
}
