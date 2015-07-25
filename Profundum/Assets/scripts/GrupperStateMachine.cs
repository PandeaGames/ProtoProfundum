using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class GrupperStateMachine : StateBehaviour {
	//Declare which states we'd like use
	public enum GrupperStates
	{
		Init,
		Attacking, 
		Agro, 
		Feeding, 
		Idle, 
		Scared,
		Searching
	}

	public GameObject agroRangeObj;
	public GameObject closeRangeObj;

	private SphereCollider _agroCollider;
	private Vector3 _agroColliderRange;
	private Collider lightAgroCol;
	private GameObject _player;
	private bool agro = false;
	private bool close = false;
	private bool lightAgro = false;
	private bool lightClose = false;
	private float _acc = 0.01f;
	private float _speed = 0f;
	private Rigidbody rb;

	private CollisionDelegate lightAgroRange, lightCloseRange, closeRange, agroRange;

	// Use this for initialization
	void Awake () {
		Initialize<GrupperStates>();

		//Change to our first state
		ChangeState(GrupperStates.Searching);

		agroRange = agroRangeObj.AddComponent <CollisionDelegate>();
		agroRange.CollideWithPlayer += () => agro = true;   
		agroRange.ExitPlayerCollision += () => agro = false; 
		_agroCollider = agroRangeObj.GetComponent<SphereCollider> ();
		_agroColliderRange = new Vector3 (_agroCollider.radius, _agroCollider.radius, _agroCollider.radius);
		
		closeRange = closeRangeObj.AddComponent <CollisionDelegate>();
		closeRange.CollideWithPlayer += () => close = true;   
		closeRange.ExitPlayerCollision += () => close = false; 

		lightCloseRange = closeRangeObj.AddComponent <CollisionDelegate>();
		lightCloseRange.tag = "light";
		lightCloseRange.CollideWithPlayer += () => lightClose = true;   
		lightCloseRange.ExitPlayerCollision += () => lightClose = false; 

		lightAgroRange = agroRangeObj.AddComponent <CollisionDelegate>();
		lightAgroRange.tag = "light";
		lightAgroRange.CollideWithPlayer += () => lightAgro = true;   
		lightAgroRange.ExitPlayerCollision += () => lightAgro = false; 

		rb = GetComponent<Rigidbody>();
	}
	void Update()
	{
	}

	void Scared_Update()
	{
		lightAgroCol = lightAgroRange.col;

		if (!lightAgroCol) {
			return;
		}

		if (!lightAgro || lightAgroCol.gameObject == null) {
			ChangeState(GrupperStates.Searching);
			return;
		}

		transform.LookAt(lightAgroCol.gameObject.transform.position);

		if(Vector3.Distance(transform.position, lightAgroCol.gameObject.transform.position) < _agroColliderRange.x) {
			Vector3 force = (transform.position -  lightAgroCol.gameObject.transform.position);
			
			force.x = force.x < 0 ? (force.x + _agroColliderRange.x) * -1:_agroColliderRange.x - force.x;
			force.y = force.y < 0 ? (force.y + _agroColliderRange.y) * -1:_agroColliderRange.y - force.y;
			force.z = force.z < 0 ? (force.z + _agroColliderRange.z) * -1:_agroColliderRange.z - force.z;
			
			force /= 20;
			
			force.y = force.y/3;
			
			if(force.y<0)
			{
				force.y = force.y*-1;
			}
			
			rb.AddForce(force, ForceMode.Impulse);
		}

	
	}

	void Searching_Update()
	{
		if (lightAgro) {
			ChangeState(GrupperStates.Scared);
		}
	}

	void Feeding_Update()
	{
	}
}
