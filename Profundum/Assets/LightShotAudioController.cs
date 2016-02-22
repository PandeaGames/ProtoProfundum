using UnityEngine;
using System.Collections;

public class LightShotAudioController : MonoBehaviour {
    private float _time;
    private Vector3 _startPos;
	// Use this for initialization
	void Start () {
        _startPos = this.transform.position;
		AkSoundEngine.PostEvent ("Play_LightHumAttack", gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        AkSoundEngine.SetRTPCValue("LightShot_Distance", Vector3.Distance(_startPos, transform.position), gameObject);
	}
}
