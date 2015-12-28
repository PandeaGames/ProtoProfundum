using UnityEngine;
using System.Collections;

public class EnvironmentAudioController : MonoBehaviour {
	private RoachController _rc;
	private HeroStateMachine _hero;
	private DomeCrystal[] _crystals;

	private bool _isInsideBubble = false;
	private bool _isNearCrystal = false;
	private bool _inDarkness = false;
	// Use this for initialization
	void Start () {
		_rc = FindObjectOfType<RoachController> ();
		_hero = FindObjectOfType<HeroStateMachine> ();
		_crystals = FindObjectsOfType<DomeCrystal>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_inDarkness && _rc.isLit(_hero.transform.position)) {
			_inDarkness = false;
			AkSoundEngine.PostEvent("Environment_InDarkness_False", _hero.gameObject);
		}
		if (!_inDarkness && !_rc.isLit(_hero.transform.position)) {
			_inDarkness = true;
			AkSoundEngine.PostEvent("Environment_InDarkness_True", _hero.gameObject);
		}

		//Habndling the audio involved with light crystals and the Domes created when they are spawned. 
		bool isInsideBubble = false;
		bool isNearCrystal = false;
		foreach (DomeCrystal crystal in _crystals) {
			if(crystal.LightBubble)
			{
				if(crystal.LightBubble.HeroInside)
				{
					isInsideBubble = true;
				}
			}
			else
			{
				if(crystal.HeroInside)
				{
					isNearCrystal = true;
				}
			}
		}
		if (_isInsideBubble && !isInsideBubble) {
			_isInsideBubble = false;
			AkSoundEngine.PostEvent("Environment_IsInsideBubble_False", _hero.gameObject);
		}
		if (!_isInsideBubble && isInsideBubble) {
			_isInsideBubble = true;
			AkSoundEngine.PostEvent("Environment_IsInsideBubble_True", _hero.gameObject);
		}
		if (_isNearCrystal && !isNearCrystal) {
			_isNearCrystal = false;
			AkSoundEngine.PostEvent("Environment_IsInsideBubble_False", _hero.gameObject);
		}
		if (!_isNearCrystal && isNearCrystal) {
			_isNearCrystal = true;
		}
	}
}
