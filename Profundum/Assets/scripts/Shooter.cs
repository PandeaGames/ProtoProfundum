using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {
	public GameObject projectile;
	public GameObject spawn;

	private bool _hasShot;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0) && !_hasShot) {
			Shoot();
			_hasShot = true;
		} else if (!Input.GetMouseButton(0)){
			_hasShot = false;
		}
	}
	private void Shoot()
	{
		Instantiate (projectile, spawn.transform.position, getProtectileRotation());
	}
	protected virtual Quaternion getProtectileRotation()
	{
		return spawn.transform.rotation;
	}
}
