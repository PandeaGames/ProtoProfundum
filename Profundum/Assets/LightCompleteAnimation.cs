using UnityEngine;
using System.Collections;

public class LightCompleteAnimation : MonoBehaviour {
	public GameObject kickObject;
	private bool hasKicked = false;
	public GameObject cinematics;
	public GameObject mainLight;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (hasKicked) 
		{
			mainLight.GetComponent<Light>().intensity -= 0.5f;
		}
	}
	void OnTriggerEnter()
	{
		if (!hasKicked) 
		{
			cinematics.SendMessage("FadeOut");
			hasKicked = true;
			kickObject.GetComponent<Rigidbody>().AddForce(new Vector3(100, 1000, 100), ForceMode.Force);
			//kickObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(100, 100, 100));
		}

	}
}
