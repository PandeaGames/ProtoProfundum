using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class GrupperV2StateMachine : StateBehaviour {
	//Declare which states we'd like use
	public enum GrupperStates
	{
		Init,
		Attacking, 
		Agro, 
		Feeding, 
		Idle, 
		Scared,
		Searching, 
		AttackRecovering,
		AttackTelegraphing
	}

	public GameObject agroRangeObj;
	public GameObject closeRangeObj;
	public GameObject attackAreaObj;
	public Vector3 searchRotation = new Vector3 (0, 1, 0);
	public LayerMask mask;
	public float attackRecoverTime = 3;
	public GameObject path;
	public GameObject pathHead;

	private SightController _sc;
	private PathNode pathNode;
	private PathContainer pathContainer;
	private SphereCollider _agroCollider;
	private Vector3 _agroColliderRange;
	private Collider lightAgroCol;
	private GameObject _player;
	private bool agro = false;
	private bool close = false;
	private bool lightAgro = false;
	private bool lightClose = false;
	private bool canAttack = false;
	private float _acc = 0.01f;
	private float _speed = 0f;
	private Rigidbody rb;
	private Vector3 _impulseSpeed = new Vector3 (0.1f, 0.03f, 0.1f);
	private Vector3 _riseSpeed = new Vector3 (0.1f, 0.4f, 0.1f);
	private Vector3 _searchRotation = new Vector3 (0, 1, 0);
	private Vector3 _agroDeltaDamp = new Vector3 (-0.1f, -0.1f, -0.1f);
	private Vector3 _attackDeltaDamp = new Vector3 (-0.5f, -0.5f, -0.5f);
	private Vector3 _attackPosition;
	private float _recoverStartTime;
	private CollisionDelegate lightAgroRange, lightCloseRange, closeRange, agroRange, attackRange;
	private int pathNodeIndex = 0;
	private float attackTelegraphEnd;
	private float attackTelegraphTime = 1;
	private PlayerHealthController playerHealthController;
	private Vector3 _oldPosition = new Vector3();
	private Vector3 _deltaPosition = new Vector3();
	private SightEye _eye;
	private bool _killAttack = false;

	void Start()
	{
		playerHealthController = FindObjectOfType<PlayerHealthController> ();

		pathContainer = path.GetComponent<PathContainer> ();
		pathNode = pathContainer.GetClosestPath (transform.position);

		_sc = FindObjectOfType<SightController> ();
	}
	// Use this for initialization

	void Awake () {
		_eye = GetComponent<SightEye> ();

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
		lightCloseRange.tag = "Player";
		lightCloseRange.CollideWithPlayer += () => lightClose = true;   
		lightCloseRange.ExitPlayerCollision += () => lightClose = false; 

		lightAgroRange = agroRangeObj.AddComponent <CollisionDelegate>();
		lightAgroRange.tag = "Player";
		lightAgroRange.CollideWithPlayer += () => lightAgro = true;   
		lightAgroRange.ExitPlayerCollision += () => lightAgro = false; 

		attackRange = attackAreaObj.AddComponent <CollisionDelegate>();
		attackRange.tag = "Player";
		attackRange.CollideWithPlayer += () => canAttack = true;   
		attackRange.ExitPlayerCollision += () => canAttack = false; 

		rb = GetComponent<Rigidbody>();

		_player = FindObjectOfType<HeroStateMachine> ().gameObject;

	}
	void Update()
	{
	}



	void DoAttack()
	{
	}

	void AttackRecovering_Update()
	{
		if (Time.time - _recoverStartTime > attackRecoverTime) 
		{
			ChangeState(GrupperStates.Searching);
		}
	}

	void AttackRecovering_Exit()
	{
		_eye.SetCanSee (false);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 0, transform.eulerAngles.z);
	}

	void AttackRecovering_Enter()
	{
		_recoverStartTime = Time.time;
	}

	void Attacking_Update()
	{
		Vector3 delta = transform.position - _attackPosition;
		rb.AddForce (Vector3.Scale (delta, _attackDeltaDamp), ForceMode.Impulse);

		if (Vector3.Distance (transform.position, _attackPosition) < 0.1) 
		{
			ChangeState(GrupperStates.AttackRecovering);
		}
	}

	void AttackTelegraphing_Enter(){
		SendMessage ("Audio_AttackTelegraphEnter");
		transform.LookAt(agroRange.col.gameObject.transform.position);
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		_attackPosition = _player.transform.position;
		attackTelegraphEnd = Time.time + attackTelegraphTime;
	}

	void AttackTelegraphing_Update(){
		if (Time.time > attackTelegraphEnd) {
			ChangeState (GrupperStates.Attacking);
		} else if (_killAttack) {
			ChangeState (GrupperStates.Attacking);
		}
	}

	void Attacking_Enter()
	{
		SendMessage ("Audio_DoAttack");
	}

	void Agro_Enter()
	{
		_eye.SetCanSee (true);
		SendMessage ("Audio_AgroEnter");
		//transform.LookAt(agroRange.col.gameObject.transform.position);
	}
	void Agro_Exit()
	{
		//_eye.SetCanSee (false);
	}
	void Agro_Update()
	{
		//transform.LookAt(_player.transform.position);
		//transform.rotation.Set (0, transform.rotation.y, 0, 0);

		RotateByMovement (0.5f);

		float force = 4f;
		
		//GetComponent<Rigidbody> ().AddForce (force, ForceMode.Impulse);

		GetComponent<Rigidbody> ().AddForce ((_player.transform.position - transform.position).normalized * force * Time.smoothDeltaTime, ForceMode.Impulse);

		if (canAttack) 
		{
			ChangeState(GrupperStates.AttackTelegraphing);
		}
		if (lightAgro == false)
		{
			ChangeState(GrupperStates.Searching);
		}
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
	void Searching_Enter()
	{
		_eye.SetCanSee (false);
		SendMessage ("Audio_SearchingEnter");
	}
	void Searching_Update()
	{
		_eye.fullAwareness = false;
		_eye.SetCanSee (false);
		_killAttack = false;
		if (lightAgro ==  true || lightClose == true) 
		{
			float dy = Mathf.Abs(transform.position.y - _player.transform.position.y);

			Vector3 dirFromAtoB = (transform.position - _player.transform.position).normalized;
			float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);

			if(lightClose)
			{
				_killAttack = true;
				_eye.fullAwareness = true;
			}


			if(dy<0.35f && dotProd < -0.8 && lightAgro || dy<0.35f && lightClose){
				_eye.SetCanSee (true);
				if(_sc.SightActive())
				{
					ChangeState(GrupperStates.Agro);
				}
			}
		}

		transform.position = Vector3.MoveTowards(transform.position, pathHead.transform.position, 0.05f);

		RotateByMovement ();

		Vector3 force = transform.forward * 0.1f;
	}

	void Feeding_Update()
	{
	}
	void RotateByMovement(float speed = 0.05f)
	{
		if (_oldPosition != transform.position) {
			Vector3 deltaPosition =  transform.position - _oldPosition;
			transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation (deltaPosition), speed);
			_oldPosition = transform.position;
		}
	}
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player" && (GrupperStates)GetState () == GrupperStates.Attacking) 
		{
			playerHealthController.doDamage(100);
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
			ChangeState(GrupperStates.AttackRecovering);
		}
	}
	void ResetPathGroup(PathGroup pathGroup)
	{
		if (pathContainer && pathGroup == pathContainer.pathGroup) {
			transform.position = pathHead.transform.position;
		}
	}

}
