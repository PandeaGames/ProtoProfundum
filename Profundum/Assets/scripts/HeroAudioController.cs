using UnityEngine;
using System.Collections;

public class HeroAudioController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ClimbUpStart()
	{
	}
	void ClimbComplete()
	{
	}
	void ClimbDownStart()
	{
	}
	void RunStep()
	{
		AkSoundEngine.PostEvent("Footstep", gameObject);
	}
}
