using UnityEngine;
using System.Collections;

public class AKCustomInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	void Awake()
	{
		//AkSoundEngine.
		//DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void ClearSceneData()
	{
		Debug.Log ("ClearSceneData!!!!!");
		SendMessage ("Terminate");
		//AkSoundEngine.UnregisterAllGameObj ();
	}
}
