using UnityEngine;
using System.Collections;

public class AKCustom_ActorDistanceController : MonoBehaviour {
    private GameObject _player;
    public LayerMask mask;

    private AKCustom_ActorAudioEmitter[] _actors;
    // Use this for initialization
    void Start () {
        _actors = FindObjectsOfType<AKCustom_ActorAudioEmitter>();
        _player = FindObjectOfType<HeroStateMachine>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
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
                Debug.DrawRay(ray.origin, ray.direction * radius, Color.green);
            }
            Debug.Log("Actor_Distance"+radius);
            AkSoundEngine.SetRTPCValue("Actor_Distance", radius, actor.gameObject);
        }
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
