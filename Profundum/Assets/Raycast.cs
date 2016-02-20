using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour {
    public GameObject target;
    public LayerMask mask;
    public Ray ray = new Ray();
    private bool _isObstructed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!target) return;

        RaycastHit hit = new RaycastHit();

        ray = new Ray(transform.position, target.transform.position - transform.position);
        float radius = Vector3.Distance(target.transform.position, transform.position);
        if (Physics.Raycast(ray, out hit, radius, mask))
        {
            Debug.DrawRay(ray.origin, ray.direction * radius, Color.red);
            _isObstructed = true;
        }
        else
        {
            _isObstructed = false;
            Debug.DrawRay(ray.origin, ray.direction * radius, Color.green);
        }
    }
    public bool IsObstructed
    {
        get { return _isObstructed; }
    }
}
