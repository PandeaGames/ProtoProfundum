using UnityEngine;
using System.Collections;

public class ProfundumPlayerHealth : PlayerHealthController {
	public float deathTimer = 5;
	private float _t;


	// Update is called once per frame
	void Update () 
	{
		if (getHealthRatio () != 1) {
			_t += Time.deltaTime;
		}
		_death = _t > deathTimer;
		Debug.Log (_death);
	}
}
