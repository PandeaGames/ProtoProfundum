using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class HeroStateMachine : StateBehaviour {

	//Declare which states we'd like use
	public enum HeroStates
	{
		Init,
		Running, 
		Idle, 
		Falling, 
		Jumping
	}

	// Use this for initialization
	void Awake () {

		Initialize<HeroStates>();
		
		//Change to our first state
		ChangeState(HeroStates.Init);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
