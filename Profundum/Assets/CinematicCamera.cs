using UnityEngine;
using System.Collections;

public class CinematicCamera : MonoBehaviour {
	public GameObject trackTarget;
	public GameObject lookTarget;
	// Use this for initialization
	void Start () {
		transform.position = trackTarget.transform.position;
		transform.LookAt (lookTarget.transform.position);
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position -= (transform.position - trackTarget.transform.position) / 10;
		transform.LookAt (lookTarget.transform.position);
	}
}
