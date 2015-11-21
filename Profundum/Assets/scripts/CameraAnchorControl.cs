using UnityEngine;
using System.Collections;

public class CameraAnchorControl : MonoBehaviour {
	public GameObject target;
	public float radius = 100;
	public float radiusWindow = 0.5f;
	public int pitchCeiling = 45;
	public int pitchFloor = -20;
	public float damp = 20;
	public float mouseDamp = 30;
	public Vector3 offset;
	public GameObject innerOffset;
	public GameObject lookTarget;
	public LayerMask mask;

	private float _pitch;
	private float _yaw;
	private float _r;
	private MainCameraMovement camMovement;
	private Vector3 innerOffsetVector;
	// Use this for initialization
	void Start () {
		camMovement = FindObjectOfType<MainCameraMovement> ();
		innerOffsetVector = innerOffset.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null)
			return;
		float pitchDelta =  Mathf.DeltaAngle(pitchFloor, _pitch / (Mathf.PI/180));
		float pitchWindow =  Mathf.DeltaAngle(pitchFloor, pitchCeiling);
		//_r = radius - ((radius * radiusWindow)) * (1 - pitchDelta / pitchWindow);
		_r = radius;

		float p = _pitch;
		p = (-1 + ((pitchDelta / pitchWindow) * (pitchCeiling + 1))) * (Mathf.PI/180);
		
		Vector3 pos = new Vector3(
			(float)(target.transform.position.x + _r * Mathf.Cos(p) * Mathf.Cos(_yaw)), 
			(float)(target.transform.position.y + .25 + _r * Mathf.Sin(p)),
			(float)(target.transform.position.z+ _r *Mathf.Cos(p) * Mathf.Sin(_yaw)));

		pos = new Vector3(
			(float)(target.transform.position.x + _r * Mathf.Cos(p) * Mathf.Cos(_yaw)), 
			(float)(target.transform.position.y + .25 + _r * Mathf.Sin(p)),
			(float)(target.transform.position.z+ _r *Mathf.Cos(p) * Mathf.Sin(_yaw)));

		transform.position = pos;
		transform.LookAt (lookTarget.transform.position);

		pos += applyOffset (offset * camMovement.GetRayDepthPercent());

		transform.position = pos;
		_pitch += (Input.GetAxis("JoystickRightVertical") / damp) - (Input.GetAxis("MouseVertical") / mouseDamp);
		_yaw -= (Input.GetAxis("JoystickRightHorizontal") / damp) + (Input.GetAxis("MouseHorizontal") / mouseDamp);

		if(_pitch > pitchCeiling *(Mathf.PI/180))
		{
			_pitch = pitchCeiling *(Mathf.PI/180);
		}
		else if(_pitch < pitchFloor *(Mathf.PI/180))
		{
			_pitch = pitchFloor *(Mathf.PI/180);
		}
	}

	private Vector3 applyOffset(Vector3 pos)
	{
		Vector3 offset = new Vector3 (pos.x, pos.y, pos.z);

		/*//z
x' = x*cos q - y*sin q
y' = x*sin q + y*cos q 
z' = z

//x
y' = y*cos q - z*sin q
z' = y*sin q + z*cos q
x' = x


	//y
z' = z*cos q - x*sin q
x' = z*sin q + x*cos q
y' = y
*/



		Vector3 d = target.transform.position - transform.position;


/*d = new Vector3(
 target.transform.position.x - transform.position.x,
  target.transform.position.y - transform.position.y,
  target.transform.position.z - transform.position.z);

float theta_z  = Mathf.Atan2(d.y, d.x);
float theta_x = Mathf.Atan2(d.z, d.y);
float theta_y = Mathf.Atan2(d.x, d.z);

//z axis rotation
offset.x = offset.x * Mathf.Cos(theta_z) - offset.y * Mathf.Sin(theta_z);
offset.y = offset.x * Mathf.Sin(theta_z) + offset.y * Mathf.Cos(theta_z);

//x axis rotation
offset.y = offset.y * Mathf.Cos (theta_x) - offset.z * Mathf.Sin(theta_x);
offset.z = offset.y * Mathf.Sin(theta_x) + offset.z * Mathf.Cos(theta_x);

//y axis rotation
offset.z = offset.z * Mathf.Cos (theta_y) - offset.x * Mathf.Sin(theta_y);
offset.x = offset.z * Mathf.Sin(theta_y) + offset.x * Mathf.Cos(theta_y);
*/
return new Vector3();
return offset;
	}
}