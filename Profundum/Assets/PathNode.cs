using UnityEngine;
using System.Collections;

public class PathNode : MonoBehaviour {
	private PathNode _next;
	private PathNode _prev;
	// Use this for initialization
	void Start () {
	
		GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public PathNode next{
		set{ _next = value;}
		get{ return _next;}
	}
	public PathNode prev{
		set{ _prev = value;}
		get{ return _prev;}
    }
}
