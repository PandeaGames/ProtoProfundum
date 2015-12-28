using UnityEngine;
using System.Collections;

public class AKCustom_ActorAudioController : MonoBehaviour {
	public LayerMask mask;

	private AKCustom_ActorAudioEmitter[] _actors;
	private AkAudioListener _listener;
	// Use this for initialization
	void Start () 
	{
		_actors = FindObjectsOfType<AKCustom_ActorAudioEmitter>();
		_listener = FindObjectOfType<AkAudioListener> ();
	}
	
	// Update is called once per frame
	void Update () {
		//AkSoundEngine.SetObjectPosition(_listener);
		foreach (AKCustom_ActorAudioEmitter actor in _actors) 
		{
			RaycastHit hit = new RaycastHit ();

			Ray ray = new Ray (_listener.transform.position, actor.transform.position - _listener.transform.position );
			float radius = Vector3.Distance(actor.transform.position, _listener.transform.position);
			if (Physics.Raycast (ray, out hit, radius, mask))
			{
				Debug.DrawRay (ray.origin, ray.direction * radius, Color.red);
				AkSoundEngine.SetObjectObstructionAndOcclusion(actor.gameObject, 0, 1f, 0f);
			}else
			{
				AkSoundEngine.SetObjectObstructionAndOcclusion(actor.gameObject, 0, 0f, 0f);
				Debug.DrawRay (ray.origin, ray.direction * radius, Color.green);
			}

		}
	}
}
