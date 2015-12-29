using UnityEngine;
using System.Collections;

public class DomeStateController : MonoBehaviour {
	private RoachController _rc;
	private HeroStateMachine _hero;
	private DomeCrystal[] _crystals;
	private bool _isInsideDome = false;
	private bool _isNearCrystal = false;
	// Use this for initialization
	void Start () {
		_hero = FindObjectOfType<HeroStateMachine> ();
		_crystals = FindObjectsOfType<DomeCrystal>();
	}
	
	// Update is called once per frame
	void Update () {
		_isInsideDome = false;
		_isNearCrystal = false;
		foreach (DomeCrystal crystal in _crystals) {
			if(crystal.LightBubble)
			{
				if(crystal.LightBubble.HeroInside)
				{
					_isInsideDome = true;
				}
			}
			else
			{
				if(crystal.HeroInside)
				{
					_isNearCrystal = true;
				}
			}
		}
	}
	public bool IsInsideDome{
		get { return _isInsideDome;}
	}
	public bool IsNearCrystal{
		get { return _isNearCrystal;}
	}
}
