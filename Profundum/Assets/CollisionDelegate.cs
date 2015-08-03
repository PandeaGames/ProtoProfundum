using UnityEngine;
using System.Collections;

public class CollisionDelegate : MonoBehaviour {
	public string tag = "Player";
	public Collider col;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public delegate void EventHandler (); 
	public event EventHandler CollideWithPlayer;
	public event EventHandler ExitPlayerCollision;
	
	// Checking a reference to a collider is better than using strings. 
	public Collider playerCollider;
	
	void OnTriggerEnter (Collider collider) { if (collider.tag == tag) {
			col = collider;
			CollideWithPlayer ();
		} }
	void OnTriggerExit (Collider collider) { if (collider.tag == tag) {
			col = collider;
			ExitPlayerCollision ();
		}}
}
