using UnityEngine;
using System.Collections;

public class ShootTargetting : MonoBehaviour {
	Camera cam;
	public LayerMask mask;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit = new RaycastHit ();
		
		Ray ray = new Ray (cam.transform.position, cam.transform.forward );
		float radius = 100;
		if (Physics.Raycast (ray, out hit, radius, mask))
		{
			Gizmos.color = Color.green;
			transform.position = hit.point;
		}
		else
		{
			Gizmos.color = Color.red;
			transform.position = ray.GetPoint (50);
		}
		Gizmos.DrawSphere (transform.position, 0.3f);
		Gizmos.DrawLine (cam.transform.position, transform.position);
	}
}
