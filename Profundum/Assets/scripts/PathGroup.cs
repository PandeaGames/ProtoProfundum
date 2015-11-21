using UnityEngine;
using System.Collections;

public class PathGroup : MonoBehaviour {
	public float totalTime;

	private float _baseTime;
	private PathContainer[] paths;
	// Use this for initialization
	void Start () {
		_baseTime = Time.time;
		paths = GetComponentsInChildren<PathContainer> ();

		foreach (PathContainer pathContainer in paths)
		{
			pathContainer.pathGroup = this;
		}
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
	public void SetPlace(float place)
	{
		_baseTime = Time.time - totalTime * place;
		UpdatePaths ();
		BroadcastMessage ("ResetPathGroup", this);
	}
	private void UpdatePaths()
	{
		for (int i =0; i<paths.Length; i++) 
		{
			paths[i].Seek(((Time.time - _baseTime) % totalTime) / totalTime, (Time.time - _baseTime) % totalTime );
		}
	}
}
