using UnityEngine;
using System.Collections;

public class PathObject : MonoBehaviour {
	public float speed = 0.3f;//speed in units/second
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SetPosition(Vector3 pos)
	{
		transform.position = pos;
	}
}
