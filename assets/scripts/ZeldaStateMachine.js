#pragma strict

enum States{Idle, Walking, Falling, Jumping, Climbing, Grabbing, Pushing};
public var currentState:States;
public var oldState:States;
function Start () 
{
	currentState = States.Idle;
}

function Update () {

}
function ChangeState(state:States)
{
	if(state == currentState)
	{
		return;
	}
	oldState = currentState;
	currentState = state;
	SendMessage("UpdateState", state);
}
function UpdateState(state:States)
{
}