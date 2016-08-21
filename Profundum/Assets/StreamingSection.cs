using UnityEngine;
using System.Collections;

public class StreamingSection : MonoBehaviour 
{
	public GameObject bridgeA;
	public GameObject bridgeB;
	public GameObject section;
    public string respawnScene;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public GameObject CreateSection()
	{
		return Instantiate (section);
	}
}
