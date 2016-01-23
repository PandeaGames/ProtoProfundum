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
			AkSoundEngine.SetState("Environment_InDarkness", "InDarkness_False");
		}
		if (!_inDarkness && !_rc.isLit(_hero.transform.position)) {
			_inDarkness = true;
			AkSoundEngine.SetState("Environment_InDarkness", "InDarkness_True");
		}

		if (_isInsideBubble && !_dsc.IsInsideDome) {
			_isInsideBubble = false;
			AkSoundEngine.SetState("Environment_IsInsideBubble", "IsInsideBubble_False");
		}
		if (!_isInsideBubble && _dsc.IsInsideDome) {
			_isInsideBubble = true;
			AkSoundEngine.SetState("Environment_IsInsideBubble", "IsInsideBubble_True");
		}
		if (_isNearCrystal && !_dsc.IsNearCrystal) {
			_isNearCrystal = false;
			AkSoundEngine.SetState("Environment_IsNearCrystal", "IsNearCrystal_False");
		}
		if (!_isNearCrystal && _dsc.IsNearCrystal) {
			_isNearCrystal = true;
			AkSoundEngine.SetState("Environment_IsNearCrystal", "IsNearCrystal_True");
		}
	}
}
