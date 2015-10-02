using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class RoachAI : MonoBehaviour {

	//Declare which states we'd like use
	public enum RoachStates
	{
		Idle, 
		Scared,
		Agro, 
		Attack, 
		PostAttack
	}

	public GameObject agroRangeObj;
	public float lifeTime = 5;

	private CollisionDelegate _lightRange;
	private CollisionDelegate _agroRange;
	private bool lightClose;
	private bool agroClose;
	private SphereCollider _agroCollider;
	private Collider lightAgroCollider;
	private Collider agroCollider;
	private Vector3 _agroColliderRange;
	private float spawnTime;
	private RoachStates State;
	private Vector3 agroOffset;
	private float agroOffsetRange = 1;
	private PlayerHealthController playerHealthController;
	void Awake()
	{
		playerHealthController = FindObjectOfType<PlayerHealthController> ();
		State = RoachStates.Idle;
		agroOffset = new Vector3 (
			Random.Range (0, agroOffsetRange) - Random.Range (0, agroOffsetRange), 
			Random.Range (0, agroOffsetRange) - Random.Range (0, agroOffsetRange), 
			Random.Range (0, agroOffsetRange) - Random.Range (0, agroOffsetRange));
	}

	// Use this for initialization
	void Start () 
	{
		spawnTime = Time.time;

		_lightRange = agroRangeObj.AddComponent <CollisionDelegate>();
		_lightRange.tag = "light";
		_lightRange.CollideWithPlayer += () => lightClose = true;   
		_lightRange.ExitPlayerCollision += () => lightClose = false;

		_agroRange = agroRangeObj.AddComponent <CollisionDelegate>();
		_agroRange.tag = "Player";
		_agroRange.CollideWithPlayer += () => agroClose = true;   
		_agroRange.ExitPlayerCollision += () => agroClose = false;

		_agroCollider = agroRangeObj.GetComponent<SphereCollider> ();
		_agroColliderRange = new Vector3 (_agroCollider.radius, _agroCollider.radius, _agroCollider.radius); 
	}

	void Update()
	{
		switch (State) 
		{
		case RoachStates.Idle:
			Idle_Update();
			break;
		case RoachStates.Scared:
			Scared_Update();
			break;
		case RoachStates.Agro:
			Agro_Update();
			break;
		case RoachStates.Attack:
			Attack_Update();
			break;
		case RoachStates.PostAttack:
			PostAttack_Update();
			break;
		}
	}
	void Scared_Update ()
	{
		spawnTime = Time.time;
		if (_lightRange.col != null && _lightRange.col.gameObject == null || _lightRange.col == null) {
			State = RoachStates.Idle;
			lightClose = false;
			return;
		}

		
		lightAgroCollider = _lightRange.col;
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
	void Agro_Update ()
	{
		if (_agroRange.col == null) 
		{
			State = RoachStates.Idle;
			return;
		}
		agroCollider = _agroRange.col;

		transform.LookAt (agroCollider.transform.position + agroOffset);

		//transform.rotation.SetLookRotation (agroCollider.transform.position);

		Vector3 force = transform.forward * 0.25f;

		GetComponent<Rigidbody> ().AddForce (force, ForceMode.Impulse);

		if (lightClose) 
		{
			State = RoachStates.Scared;
		}

	}
	void Attack_Update ()
	{
	}
	void PostAttack_Update ()
	{
	}
	// Update is called once per frame
	void Idle_Update ()
	{
		if (lightClose) 
		{
			State = RoachStates.Scared;
		}
		if (agroClose) {
			State = RoachStates.Agro;
		}
		if (Time.time - spawnTime > lifeTime) {
			Destroy (gameObject);
		}
	}
	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "Player")
		{
			playerHealthController.doDamage(10);
		}
	}
}