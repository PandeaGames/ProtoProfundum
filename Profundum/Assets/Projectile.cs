using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public float speed = 1.0f;
	public float lifeTime = 3;

	private float _startTime;
	// Use this for initialization
	void Start () {
		_startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward * speed);
		//transform.position = transform.position. + (Vector3.forward * speed);
		if (Time.time - _startTime > lifeTime) {
			Destroy (gameObject);
		}
	}
}
