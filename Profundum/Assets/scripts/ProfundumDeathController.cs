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
		hero.Death();
		if (sceneToLoad != "") 
		{


			//Application.LoadLevel (sceneToLoad);
			return;
		}
		Application.LoadLevel (Application.loadedLevelName);
	}
	public override void Update()
	{
		base.Update ();
	

	}
}
