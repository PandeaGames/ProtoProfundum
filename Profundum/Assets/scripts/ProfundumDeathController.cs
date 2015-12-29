using UnityEngine;
using System.Collections;

public class ProfundumDeathController : GameDeathController {
	public string sceneToLoad = "";
	private HeroMovementControls hero;
	public override void Start()
	{
		base.Start ();
		hero = FindObjectOfType<HeroMovementControls> ();
	}
	protected override void Death()
	{
		if (sceneToLoad != "") 
		{
			hero.Death();
			//Application.LoadLevel (sceneToLoad);
			return;
		}
		Application.LoadLevel (Application.loadedLevelName);
	}
}
