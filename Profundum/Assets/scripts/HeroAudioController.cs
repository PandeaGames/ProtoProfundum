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
		AkSoundEngine.PostEvent("Hero_ClimbUpStart", gameObject);
	}
	void ClimbComplete()
	{
		AkSoundEngine.PostEvent("Hero_ClimbComplete", gameObject);
	}
	void ClimbDownStart()
	{
		AkSoundEngine.PostEvent("Hero_ClimbDownStart", gameObject);
	}
	void RunStep()
	{
		AkSoundEngine.PostEvent("Play_Footstep", gameObject);
	}
	void Dead()
	{
	}
    void Jump()
    {
        AkSoundEngine.PostEvent("Play_Jump", gameObject);
    }
    void HandPlan()
    {
        AkSoundEngine.PostEvent("Play_HandPlant", gameObject);
    }
    void KneePlant()
    {
        AkSoundEngine.PostEvent("Play_KneePlant", gameObject);
    }
    void StepUp()
    {
        AkSoundEngine.PostEvent("Play_StepUp", gameObject);
    }
}
