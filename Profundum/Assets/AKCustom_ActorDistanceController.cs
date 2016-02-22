using UnityEngine;
using System.Collections;

public class AKCustom_ActorDistanceController : MonoBehaviour {
    private GameObject _player;
    public LayerMask mask;

    private AKCustom_ActorAudioEmitter[] _actors;
    private AkAudioListener _listener;
    // Use this for initialization
    void Start () {
        _actors = FindObjectsOfType<AKCustom_ActorAudioEmitter>();
        _player = FindObjectOfType<HeroStateMachine>().gameObject;
        _listener = FindObjectOfType<AkAudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        float closestDist = float.MaxValue;
        foreach (AKCustom_ActorAudioEmitter actor in _actors)
        {
            RaycastHit hit = new RaycastHit();

            Ray ray = new Ray(_player.transform.position, actor.transform.position - _player.transform.position);
            float radius = Vector3.Distance(actor.transform.position, _player.transform.position);
            if (Physics.Raycast(ray, out hit, radius, mask))
            {
                Debug.DrawRay(ray.origin, ray.direction * radius, Color.red);
            }
            else
            {
                if (radius < closestDist) closestDist = radius;
                Debug.DrawRay(ray.origin, ray.direction * radius, Color.green);
            }
            AkSoundEngine.SetRTPCValue("Actor_Distance", radius, actor.gameObject);
        }
        AkSoundEngine.SetRTPCValue("Grupper_Distance", closestDist, _listener.gameObject);
    }
    void ResetSceneData()
    {
        _actors = FindObjectsOfType<AKCustom_ActorAudioEmitter>();
        _player = FindObjectOfType<HeroStateMachine>().gameObject;
    }
    void ClearSceneData()
    {
        _actors = new AKCustom_ActorAudioEmitter[0];
    }
}
