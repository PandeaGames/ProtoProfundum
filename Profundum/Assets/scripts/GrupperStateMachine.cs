using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class GrupperStateMachine : StateBehaviour {
	//Declare which states we'd like use
	public enum GrupperStates
	{
		Init,
		Attacking, 
		Agro, 
		Feeding, 
		Idle
	}
	// Use this for initialization
	void Awake () {
		Initialize<GrupperStates>();
		
		//Change to our first state
		ChangeState(GrupperStates.Init);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
