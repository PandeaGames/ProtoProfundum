using UnityEngine;
using System.Collections;

public class DomeCrystal : MonoBehaviour {
	public GameObject dome;
	public GameObject flare;
	public GameObject sphere;
	private bool _heroInside = false;
	private LightBubble _lightBubble;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter (Collider collider)
	{
		if (collider.tag == "Player") {
			_heroInside = true;
		}
		if (collider.tag == "light") {
			collider.gameObject.SendMessage("Contact", gameObject);
			Break ();
		}
	}
	public void Break()
	{
		GameObject bubble = (GameObject)Instantiate (dome, transform.position, transform.rotation);
       // bubble.transform.parent = gameObject.transform.parent;
       // bubble.transform.position = transform.position;
        _lightBubble = bubble.GetComponent<LightBubble> ();
		_lightBubble.animTime = 3;
		_heroInside = false;
		GetComponent<Collider> ().enabled = false;

		Destroy (flare);
		Destroy (sphere);
	}
	void OnTriggerExit (Collider collider)
	{
		if (collider.tag == "Player") {
			_heroInside = false;
		}
	}
	public bool HeroInside{
		get {return _heroInside;}
	}
	public LightBubble LightBubble{
		get{ return _lightBubble;}
	}
}
