using UnityEngine;
using System.Collections;

public class SightAudioController : MonoBehaviour {
	private SightController _sc;
	private bool _isInSight = false;
	private bool _sightActive = false;
	private bool _sightFull = false;
    private AkAudioListener _listener;
	// Use this for initialization
	void Start () 
	{
		_sc = GetComponent<SightController> ();
        _listener =  FindObjectOfType<AkAudioListener>();

    }
	
	// Update is called once per frame
	void Update () {
        AkSoundEngine.SetRTPCValue("Tension_Vision", _sc.GetSight()*100, _listener.gameObject);

		if (_isInSight != _sc.IsInSight ()) 
		{
			//there has been a change in sight value. Post event. 
			_isInSight = _sc.IsInSight();
			if(_isInSight)
			{
				AkSoundEngine.SetState( "Sight_IsInSight", "IsInSight_True");
                AkSoundEngine.SetState(0, 1);
            }
			else
			{
				AkSoundEngine.SetState( "Sight_IsInSight", "IsInSight_False");
                AkSoundEngine.SetState(0, 0);
            }
		}
		if (_sightActive != _sc.SightActive ()) 
		{
			//there has been a change in SightActive. Post event. 
			_sightActive = _sc.SightActive ();
			if(_sightActive)
			{
				AkSoundEngine.SetState( "Sight_SightActive", "SightActive_True");
                AkSoundEngine.SetState(1, 1);
            }
			else
			{
				AkSoundEngine.SetState( "Sight_SightActive", "SightActive_False");
                AkSoundEngine.SetState(1, 0);
            }
		}
		if (_sightFull && _sc.GetSight () != 1) {
			_sightFull = false;
            AkSoundEngine.SetState(2, 0);
            AkSoundEngine.SetState( "Sight_SightFull", "SightFull_False");
		}

		if (!_sightFull && _sc.GetSight () == 1) {
			_sightFull = true;
            AkSoundEngine.SetState(2, 1);
            AkSoundEngine.SetState( "Sight_SightFull", "SightFull_True");
		}
	}
}
