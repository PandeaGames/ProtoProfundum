using UnityEngine;
using System.Collections;

public class ProfundumDeathController : GameDeathController {
	public string sceneToLoad = "";
	protected override void Death()
	{
		if (sceneToLoad != "") 
		{
			Application.LoadLevel (sceneToLoad);
			return;
		}
		Application.LoadLevel (Application.loadedLevelName);
	}
}
