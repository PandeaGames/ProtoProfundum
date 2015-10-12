using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
public class GrupperStateTests : StateBehaviour 
{
	public GameObject agroRangeObj;
	public GameObject closeRangeObj;

	private GameObject _player;
	private bool agro = false;
	private bool close = false;

	void Awake () {
		CollisionDelegate agroRange = agroRangeObj.AddComponent <CollisionDelegate>();
		agroRange.CollideWithPlayer += () => agro = true;   
		agroRange.ExitPlayerCollision += () => agro = false;   
		
		CollisionDelegate closeRange = closeRangeObj.AddComponent <CollisionDelegate>();
		closeRange.CollideWithPlayer += () => close = true;   
		closeRange.ExitPlayerCollision += () => close = false;   
	}

	// Use this for initialization
	void Start () 
	{
		Initialize< GrupperStateMachine.GrupperStates >();
		_player = GameObject.FindWithTag ("Player");
	}
	// Update is called once per frame
	void Update () 
	{
		if (stateMachine.GetState().Equals(GrupperStateMachine.GrupperStates.Searching) && agro) {
			ChangeState(GrupperStateMachine.GrupperStates.Agro);
			//Debug.Log ("CHANGE STATE");
			//Searching_Update();
		}
	}
	void OnCollisionEnter(Collision col)
	{

	}
}
