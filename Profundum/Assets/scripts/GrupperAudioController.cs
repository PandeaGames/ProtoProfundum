using UnityEngine;
using System.Collections;

public class GrupperAudioController : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void Audio_DoAttack()
	{
		//AkSoundEngine.Set
		AkSoundEngine.PostEvent("Play_Attack", gameObject);
	}
	void Audio_AgroEnter()
	{
		AkSoundEngine.PostEvent("Play_Spot", gameObject);
	}
	void Audio_AttackTelegraphEnter()
	{
		AkSoundEngine.PostEvent("Play_Spot", gameObject);
	}
	void Audio_SearchingEnter()
	{
		AkSoundEngine.PostEvent("Grupper_Idle_Start", gameObject);
	}
}
