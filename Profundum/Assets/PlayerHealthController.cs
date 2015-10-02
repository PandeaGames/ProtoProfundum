using UnityEngine;
using System.Collections;

public class PlayerHealthController : MonoBehaviour {
	public float rebound = 0.25f;
	public float health = 100;
	private float _health;
	// Use this for initialization
	void Start () {
		_health = health;
	}
	
	// Update is called once per frame
	void Update () {
		if (_health < health) {
			_health+=rebound;
		}
	}
	public void doDamage(float damage)
	{
		_health -= damage;
		if (_health < 0)
			_health = 0;
	}
	public float getHealth()
	{
		return health;
	}
	public float getHealthRatio()
	{
		return _health / health;
 	}
}
