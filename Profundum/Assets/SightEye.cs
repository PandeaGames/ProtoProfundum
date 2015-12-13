using UnityEngine;
using System.Collections;

public class SightEye : MonoBehaviour {
	private bool _canSee;
	public float power = 1;
	private bool _fullAwareness = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SetCanSee(bool value)
	{
		_canSee = value;
	}
	public bool GetCanSee()
	{
		return _canSee;
	}
	public bool fullAwareness{
		get {return _fullAwareness;} 
		set {_fullAwareness = value;}
	}
}
