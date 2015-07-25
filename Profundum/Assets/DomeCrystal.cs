using UnityEngine;
using System.Collections;

public class DomeCrystal : MonoBehaviour {
	public GameObject dome;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter (Collider collider)
	{
		if (collider.tag == "light") {
			collider.gameObject.SendMessage("Contact", gameObject);
			Break ();
		}
	}
	public void Break()
	{
		GameObject bubble = (GameObject)Instantiate (dome, transform.position, transform.rotation);
		LightBubble bubbleComp = bubble.GetComponent<LightBubble> ();
		bubbleComp.animTime = 3;
		Destroy (gameObject);
	}
}
