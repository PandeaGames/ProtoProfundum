using UnityEngine;
using System.Collections;

public class ContinueWhenGroupIsDone : MonoBehaviour {
	private PathGroup _group;
	public string sceneToLoad = "GameStart";
	// Use this for initialization
	void Start () {
		_group = GetComponent<PathGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_group.GetPlayerHead () > 0.9f) 
		{
			Application.LoadLevel(sceneToLoad);
		}
	}
}
