using UnityEngine;
using System.Collections;

public class GameDeathController : MonoBehaviour 
{
	protected PlayerHealthController healthController;
	private bool dead = false;
	// Use this for initialization
	public virtual void Start () 
	{
		healthController = FindObjectOfType<PlayerHealthController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!healthController)
			return;
		if (healthController.death && !dead) {
			Death ();
			dead = true;
		}
	}

	protected virtual void Death()
	{
	}
}
