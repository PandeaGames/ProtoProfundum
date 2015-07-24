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
		Searching
	}

	public GameObject agroRangeObj;
	public GameObject closeRangeObj;

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
		if (lightAgro) 
		{
			lightAgroCol = lightAgroRange.col;
			transform.LookAt(lightAgroRange.col.gameObject.transform.position);
			//transform.transform.Translate(Vector3.back * _speed);
			//float dist =  Vector3.Distance(transform.position, lightAgroCol.gameObject.transform.position);
			rb.AddForce((transform.position -  lightAgroCol.gameObject.transform.position) / 10, ForceMode.Impulse);
		}
	}
	void Searching_Update()
	{
	}
}
