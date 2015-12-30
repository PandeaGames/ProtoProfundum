using UnityEngine;
using System.Collections;

public class BodyKick : MonoBehaviour {
	public GameObject collideWith;
	public GameObject kick;
	public Vector3 kickPower =  new Vector3(0, 1, 0);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider col)
	{

		if (col.tag == "light") 
		{
			//kick.GetComponent<Rigidbody>().AddRelativeForce(kickPower);
			kick.GetComponent<Rigidbody>().AddForce(kickPower);
		}
	}
}
