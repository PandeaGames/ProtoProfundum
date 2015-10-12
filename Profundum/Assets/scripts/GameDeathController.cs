using UnityEngine;
using System.Collections;

public class GameDeathController : MonoBehaviour 
{
	protected PlayerHealthController healthController;
	// Use this for initialization
	void Start () 
	{
		healthController = FindObjectOfType<PlayerHealthController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!healthController)
			return;
		if (healthController.death) {
			Death ();
		}
	}

	protected virtual void Death()
	{
	}
}
