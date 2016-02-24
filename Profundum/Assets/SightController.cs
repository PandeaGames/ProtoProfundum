using UnityEngine;
using System.Collections;

public class SightController : MonoBehaviour {
	public float totalSight = 100;
	public float speed = 0.05f;

	private float _sight;
	private SightEye[] _eyes;
	private float velocity = 0;

	private float _sightValue = 0;


	// Use this for initialization
	void Start () {
		_eyes = FindObjectsOfType<SightEye> ();
		_sightValue = totalSight;
	}
	
	// Update is called once per frame
	void Update () {
		velocity -= speed;
		foreach(SightEye eye in _eyes)
		{
			if (eye.GetCanSee ()) {
				velocity += eye.power;
				if (eye.fullAwareness) {
					_sightValue = 0;
					break;
				}
			} 
		}

		_sightValue = _sightValue - velocity;

		if (_sightValue > totalSight) {
			velocity = 0;
			_sightValue = totalSight;
		} else if (_sightValue < 0) {
			_sightValue = 0;
			velocity = 0;
		}
	}

	public float GetSight()
	{
		return _sightValue / totalSight;
	}
	public bool SightActive()
	{
		return _sightValue == 0;
	}
	public bool IsInSight()
	{
		foreach(SightEye eye in _eyes)
		{
			if(eye.GetCanSee())
			{
				return true;
			}
		}
		return false;
	}
	void ResetSceneData()
	{
		_eyes = FindObjectsOfType<SightEye> ();
	}
}
