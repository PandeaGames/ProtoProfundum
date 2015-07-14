using UnityEngine;
using System.Collections;

public class CameraAnchorControl : MonoBehaviour {
	public GameObject target;
	public float radius = 100;
	public float radiusWindow = 0.5f;
	public int pitchCeiling = 45;
	public int pitchFloor = -20;
	public float damp = 20;

	private float _pitch;
	private float _yaw;
	private float _r;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null)
			return;

		float pitchDelta =  Mathf.DeltaAngle(pitchFloor, _pitch / (Mathf.PI/180));
		float pitchWindow =  Mathf.DeltaAngle(pitchFloor, pitchCeiling);
		_r = radius - ((radius * radiusWindow)) * (1 - pitchDelta / pitchWindow);
		
		float p = _pitch;
		p = (-1 + ((pitchDelta / pitchWindow) * (pitchCeiling + 1))) * (Mathf.PI/180);
		
		Vector3 pos = new Vector3(
			(float)(target.transform.position.x + _r * Mathf.Cos(p) * Mathf.Cos(_yaw)), 
			(float)(target.transform.position.y + .25 + _r * Mathf.Sin(p)),
			(float)(target.transform.position.z+ _r *Mathf.Cos(p) * Mathf.Sin(_yaw)));
		transform.position = pos;
		_pitch += Input.GetAxis("JoystickRightVertical") / damp;
		_yaw -= Input.GetAxis("JoystickRightHorizontal") / damp;
		
		if(_pitch > pitchCeiling *(Mathf.PI/180))
		{
			_pitch = pitchCeiling *(Mathf.PI/180);
		}
		else if(_pitch < pitchFloor *(Mathf.PI/180))
		{
			_pitch = pitchFloor *(Mathf.PI/180);
		}
	}
}