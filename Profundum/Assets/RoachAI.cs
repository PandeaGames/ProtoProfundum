using UnityEngine;
using System.Collections;

public class RoachAI : MonoBehaviour {
	public GameObject agroRangeObj;
	public float lifeTime = 5;

	private CollisionDelegate _agroRange;
	private bool lightClose;
	private SphereCollider _agroCollider;
	private Collider lightAgroCollider;
	private Vector3 _agroColliderRange;
	private float spawnTime;
	// Use this for initialization
	void Start () {
		spawnTime = Time.time;

		_agroRange = agroRangeObj.AddComponent <CollisionDelegate>();
		_agroRange.tag = "light";
		_agroRange.CollideWithPlayer += () => lightClose = true;   
		_agroRange.ExitPlayerCollision += () => lightClose = false;

		_agroCollider = agroRangeObj.GetComponent<SphereCollider> ();
		_agroColliderRange = new Vector3 (_agroCollider.radius, _agroCollider.radius, _agroCollider.radius); 
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lightClose) 
		{
			if(_agroRange.col != null && _agroRange.col.gameObject == null) return;

			lightAgroCollider = _agroRange.col;
			transform.LookAt (lightAgroCollider.gameObject.transform.position);  

			if (Vector3.Distance (transform.position, lightAgroCollider.gameObject.transform.position) < _agroColliderRange.x) {  
				Vector3 force = (transform.position - lightAgroCollider.gameObject.transform.position);  
							  
				force.x = force.x < 0 ? (force.x + _agroColliderRange.x) * -1 : _agroColliderRange.x - force.x;  
				force.y = force.y < 0 ? (force.y + _agroColliderRange.y) * -1 : _agroColliderRange.y - force.y;  
				force.z = force.z < 0 ? (force.z + _agroColliderRange.z) * -1 : _agroColliderRange.z - force.z;  
				force /= 20;  
				force.y = force.y / 3;  
				  
				if (force.y < 0) {  
					force.y = force.y * -1;
				}  
				GetComponent<Rigidbody> ().AddForce (force, ForceMode.Impulse);
			}
		}
		if (Time.time - spawnTime > lifeTime) {
			Destroy (gameObject);
		}
	}
}