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
				AkSoundEngine.SetState( "Sight_IsInSight", "IsInSight_True");
			}
			else
			{
				AkSoundEngine.SetState( "Sight_IsInSight", "IsInSight_False");
			}
		}
		if (_sightActive != _sc.SightActive ()) 
		{
			//there has been a change in SightActive. Post event. 
			_sightActive = _sc.SightActive ();
			if(_sightActive)
			{
				AkSoundEngine.SetState( "Sight_SightActive", "SightActive_True");
			}
			else
			{
				AkSoundEngine.SetState( "Sight_SightActive", "SightActive_False");
			}
		}
		if (_sightFull && _sc.GetSight () != 1) {
			_sightFull = false;
			AkSoundEngine.SetState( "Sight_SightFull", "SightFull_False");
		}

		if (!_sightFull && _sc.GetSight () == 1) {
			_sightFull = true;
			AkSoundEngine.SetState( "Sight_SightFull", "SightFull_True");
		}
	}
}
