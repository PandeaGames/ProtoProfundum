using UnityEngine;
using System.Collections;

public class GrupperAudioController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DoAttack()
	{
		AkSoundEngine.PostEvent("Play_Attack", gameObject);
	}
}
