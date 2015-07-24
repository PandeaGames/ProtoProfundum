using UnityEngine;
using System.Collections;

public class GrupperStateTests : MonoBehaviour 
{
	public GrupperStateMachine sm;
	public GameObject agroRangeObj;

	private GameObject _player;
	private bool agro = false;

	// Use this for initialization
	void Start () 
	{
		_player = GameObject.FindWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (agro) {
			transform.LookAt(_player.transform.position);
		}
	}
	void OnCollisionEnter(Collision col)
	{

	}
	void Awake () {
		CollisionDelegate agroRange = agroRangeObj.AddComponent <CollisionDelegate>();
		agroRange.CollideWithPlayer += () => agro = true;   
		agroRange.ExitPlayerCollision += () => agro = false;   
	}
}
