using UnityEngine;
using System.Collections;

public class SightAudioController : MonoBehaviour {
	private SightController _sc;
	private bool _isInSight = false;
	private bool _sightActive = false;
	private bool _sightFull = false;
	// Use this for initialization
	void Start () 
	{
		_sc = GetComponent<SightController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_isInSight != _sc.IsInSight ()) 
		{
			//there has been a change in sight value. Post event. 
			_isInSight = _sc.IsInSight();
			if(_isInSight)
			{
				AkSoundEngine.PostEvent( "Sight_IsInSight_True", gameObject);
			}
			else
			{
				AkSoundEngine.PostEvent("Sight_IsInSight_False", gameObject);
			}
		}
		if (_sightActive != _sc.SightActive ()) 
		{
			//there has been a change in SightActive. Post event. 
			_sightActive = _sc.SightActive ();
			if(_sightActive)
			{
				AkSoundEngine.PostEvent("Sight_SightActive_True", gameObject);
			}
			else
			{
				AkSoundEngine.PostEvent("Sight_SightActive_False", gameObject);
			}
		}
		if (_sightFull && _sc.GetSight () != 1) {
			_sightFull = false;
			AkSoundEngine.PostEvent("Sight_SightFull_False", gameObject);
		}

		if (!_sightFull && _sc.GetSight () == 1) {
			_sightFull = true;
			AkSoundEngine.PostEvent("Sight_SightFull_True", gameObject);
		}
	}
}
