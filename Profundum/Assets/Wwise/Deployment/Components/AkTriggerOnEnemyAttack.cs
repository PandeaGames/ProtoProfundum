using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class TriggerOnEnemyAttack : AkTriggerBase
{
	void Attacking()
	{
		if(triggerDelegate != null)
		{
			triggerDelegate(null);
		}
	}
}