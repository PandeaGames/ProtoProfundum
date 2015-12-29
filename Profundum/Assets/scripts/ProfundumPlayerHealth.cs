using UnityEngine;
using System.Collections;

public class ProfundumPlayerHealth : PlayerHealthController {
	public float deathTimer = 5;
	private float _t;

	// Update is called once per frame
	public override void Update () 
	{
		_death = getHealthRatio () == 0;
		base.Update ();
	}
}
