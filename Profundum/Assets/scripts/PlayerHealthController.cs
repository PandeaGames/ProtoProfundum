using UnityEngine;
using System.Collections;

public class PlayerHealthController : MonoBehaviour {
	public float rebound = 0.25f;
	public float health = 100;
	protected float _health;
	protected bool _death;
	// Use this for initialization
	void Start () {
		_health = health;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (_health < health) {
			if(!_death)
			{
				_health += rebound;
			}

		} else if (_health > health) {
			_health = health;
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
	public bool death{
		get { return _death;}
	}
}
