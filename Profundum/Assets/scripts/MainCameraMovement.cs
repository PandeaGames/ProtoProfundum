using UnityEngine;
using System.Collections;

public class MainCameraMovement : MonoBehaviour {
	public static string MESSAGE_CHANGE_TARGET = "ChangeTarget";

	public GameObject target;
	public float speed = 1f;
	public float damp = 10;
	public LayerMask mask;

	public float radius = 3;

	private Vector3 pos;
	private float rayPct = 1.0f;
	
	// Use this for initialization
	void Awake () {

	}

	void Start () 
	{
		;
		if (target) 
		{
			transform.position = target.transform.position;
			transform.rotation = target.transform.rotation;
		}
		pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (target) 
		{
			float step = Vector3.Distance(pos, target.transform.position) / damp;
			pos = Vector3.MoveTowards(pos, target.transform.position, step);
			
			step = Quaternion.Angle(transform.rotation, target.transform.rotation) / damp;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);

			transform.position = checkRadius(pos);

		}
	}
	void ChangeTarget(GameObject newTarget)
	{
		target = newTarget;
	}
	private Vector3 checkRadius(Vector3 pos)
	{
		/*RaycastHit hit = new RaycastHit ();
		float deploymentHeight = 10;
		Ray ray = new Ray (transform.position + transform.forward * .55f + transform.up * 2.5f, Vector3.down);
		if (Physics.Raycast (ray, out hit, deploymentHeight, mask)) {
			float distanceToGround = hit.distance;
			if (Input.GetKey (KeyCode.Space)) {
				if(distanceToGround>1.6 && distanceToGround < 2)
				{
					//climb up
					ChangeState (HeroStates.Climbing);
				}
				if(distanceToGround>5.0 && distanceToGround < 5.4)
				{
					//climb down
					ChangeState (HeroStates.ClimbingDown);
				}
			}
		}*/
		RaycastHit hit = new RaycastHit ();
		//Ray ray = new Ray (pos, transform.forward );
		Ray ray = new Ray ((pos + transform.forward * (radius)), transform.forward * -1 );
		if (Physics.Raycast (ray, out hit, radius, mask))
		{
			Debug.DrawRay (ray.origin, ray.direction * radius, Color.red);
			rayPct = hit.distance / radius;
			return pos + (transform.forward *  (radius - hit.distance));
		}
		rayPct = 1.0f;
		Debug.DrawRay (ray.origin, ray.direction * radius, Color.green);
		return pos;
	}
	public float GetRayDepthPercent()
	{
		return rayPct;
	}
	public bool GetIsHittingGeometry()
	{
		return rayPct != 1.0f;
	}
}
