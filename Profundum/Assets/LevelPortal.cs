using UnityEngine;
using System.Collections;

public class LevelPortal : MonoBehaviour 
{
	public string sceneToLoad = "";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			if(sceneToLoad!= "")
			{
				Application.LoadLevel (sceneToLoad);
			}
		}
	}
}
