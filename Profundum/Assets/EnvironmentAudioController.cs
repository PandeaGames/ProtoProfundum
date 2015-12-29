using UnityEngine;
using System.Collections;

public class EnvironmentAudioController : MonoBehaviour {
	private RoachController _rc;
	private HeroStateMachine _hero;
	private DomeCrystal[] _crystals;
	private DomeStateController _dsc;

	private bool _isInsideBubble = false;
	private bool _isNearCrystal = false;
	private bool _inDarkness = false;
	// Use this for initialization
	void Start () {
		_rc = FindObjectOfType<RoachController> ();
		_hero = FindObjectOfType<HeroStateMachine> ();
		_crystals = FindObjectsOfType<DomeCrystal>();
		_dsc = FindObjectOfType<DomeStateController> ();
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

		if (_isInsideBubble && !_dsc.IsInsideDome) {
			_isInsideBubble = false;
			AkSoundEngine.PostEvent("Environment_IsInsideBubble_False", _hero.gameObject);
		}
		if (!_isInsideBubble && _dsc.IsInsideDome) {
			_isInsideBubble = true;
			AkSoundEngine.PostEvent("Environment_IsInsideBubble_True", _hero.gameObject);
		}
		if (_isNearCrystal && !_dsc.IsNearCrystal) {
			_isNearCrystal = false;
			AkSoundEngine.PostEvent("Environment_IsNearCrystal_False", _hero.gameObject);
		}
		if (!_isNearCrystal && _dsc.IsNearCrystal) {
			_isNearCrystal = true;
			AkSoundEngine.PostEvent("Environment_IsNearCrystal_True", _hero.gameObject);
		}
	}
}
