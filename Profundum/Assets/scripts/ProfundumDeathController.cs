using UnityEngine;
using System.Collections;

public class ProfundumDeathController : GameDeathController {
	
	protected override void Death()
	{
		Application.LoadLevel (Application.loadedLevelName);
	}
}
