using UnityEngine;
using System.Collections;

public class DomeStateController : MonoBehaviour {
	private RoachController _rc;
	private HeroStateMachine _hero;
	private DomeCrystal[] _crystals;
	private LightBubble[] _bubbles;
	private bool _isInsideDome = false;
	private bool _isNearCrystal = false;
	// Use this for initialization
	void Start () {
		_hero = FindObjectOfType<HeroStateMachine> ();
		_crystals = FindObjectsOfType<DomeCrystal>();
		_bubbles = FindObjectsOfType<LightBubble>();
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
		foreach (LightBubble bubble in _bubbles) {
			if(bubble.HeroInside)
			{
				_isInsideDome = true;
			}
		}
	}
	public bool IsInsideDome{
		get { return _isInsideDome;}
	}
	public bool IsNearCrystal{
		get { return _isNearCrystal;}
	}
	void ResetSceneData()
	{
		_crystals = FindObjectsOfType<DomeCrystal>();
		_bubbles = FindObjectsOfType<LightBubble>();
	}
	void ClearSceneData()
	{
		_crystals = new DomeCrystal[0];
		_bubbles = new LightBubble[0];
	}
}
