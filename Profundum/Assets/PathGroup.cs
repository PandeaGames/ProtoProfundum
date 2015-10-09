using UnityEngine;
using System.Collections;

public class PathGroup : MonoBehaviour {
	public float totalTime;
	private PathContainer[] paths;
	// Use this for initialization
	void Start () {
		paths = GetComponentsInChildren<PathContainer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{

		for (int i =0; i<paths.Length; i++) 
		{
			paths[i].Seek((Time.time % totalTime) / totalTime, Time.time % totalTime );
		}
	}
}
