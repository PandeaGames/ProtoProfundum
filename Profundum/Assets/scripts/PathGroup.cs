using UnityEngine;
using System.Collections;

public class PathGroup : MonoBehaviour {
	public float totalTime = 0;
	public bool pause = false;
	public float currentTime = 0;

	private float _baseTime;
	private PathContainer[] paths;

	[Range(0.0f, 1.0f)]
	public float head = 0;
	// Use this for initialization
	void Awake () {
		paths = GetComponentsInChildren<PathContainer> ();

		foreach (PathContainer pathContainer in paths)
		{
			pathContainer.pathGroup = this;
		}
		SetPlace (head);
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdatePaths ();
	}
	public void Reset()
	{
		SetPlace (0);
	}
	public void SetPlace(float place, bool updatePaths = true)
	{
		_baseTime = Time.time - totalTime * place;

		if (updatePaths) 
		{
			UpdatePaths ();
		}

		Game game = FindObjectOfType<Game> ();
		if (game) {
			game.BroadcastMessage ("ResetPathGroup", this);
		}
	}
	void ResetPathGroup(PathGroup pathGroup)
	{
	}
	private void UpdatePaths()
	{
		if (pause) {
			SetPlace(head, false);
		} else {
			head = ((Time.time - _baseTime) % totalTime) / totalTime;
		}
		for (int i =0; i<paths.Length; i++) 
		{
			paths[i].Seek(((Time.time - _baseTime) % totalTime) / totalTime, (Time.time - _baseTime) % totalTime );
		}
		currentTime = (Time.time - _baseTime) % totalTime;
	}
	public float GetPlayerHead()
	{
		return ((Time.time - _baseTime) % totalTime) / totalTime;
	}
}
