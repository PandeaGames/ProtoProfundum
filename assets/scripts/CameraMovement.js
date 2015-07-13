#pragma strict
public var target:GameObject;
public var speed:float = 1f;
public var damp:float = 10;
function Start () {
	transform.position = target.transform.position;
	transform.rotation = target.transform.rotation;
}

function Update () {
	//var step:float = speed * Time.deltaTime;
	var step:float = Vector3.Distance(transform.position, target.transform.position) / damp;
	transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
	
	step = Quaternion.Angle(transform.rotation, target.transform.rotation) / damp;
	transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
}
function ChangeTarget(newTarget:GameObject):void

{
	target = newTarget;
}