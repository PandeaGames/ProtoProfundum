using UnityEngine;
using System.Collections;

public class MainCameraMovement : MonoBehaviour {
	public static string MESSAGE_CHANGE_TARGET = "ChangeTarget";

	public GameObject target;
	public float speed = 1f;
	public float damp = 10;
	// Use this for initialization
	void Awake () {
	
	}

	void Start () {
		if (target) {
			transform.position = target.transform.position;
			transform.rotation = target.transform.rotation;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (target) {
			float step = Vector3.Distance(transform.position, target.transform.position) / damp;
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
			
			step = Quaternion.Angle(transform.rotation, target.transform.rotation) / damp;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
		}
	}
	void ChangeTarget(GameObject newTarget)
	{
		target = newTarget;
	}
}
