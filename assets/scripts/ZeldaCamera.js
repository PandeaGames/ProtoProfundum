#pragma strict
public var target:GameObject;
private var pitch:float = 0;
private var yaw:float = 0;
public var radius:float = 100;
public var radiusWindow:float = 0.5f;
private var r:float;
public var pitchCeiling:int = 45;
public var pitchFloor:int = -20;
public var damp:float = 20;
function Start () {
}

function Update () 
{
	var pitchDelta:float =  Mathf.DeltaAngle(pitchFloor, pitch / (Mathf.PI/180));
	var pitchWindow:float =  Mathf.DeltaAngle(pitchFloor, pitchCeiling);
	r = radius - ((radius * radiusWindow)) * (1 - pitchDelta / pitchWindow);
	
	var p:float = pitch;
	p = (-1 + ((pitchDelta / pitchWindow) * (pitchCeiling + 1))) * (Mathf.PI/180);
	
	var pos:Vector3 = new Vector3(
		target.transform.position.x + r * Mathf.Cos(p) * Mathf.Cos(yaw), 
		target.transform.position.y + .25 + r * Mathf.Sin(p),
		target.transform.position.z+ r *Mathf.Cos(p) * Mathf.Sin(yaw)
	);
	transform.position = pos;
	pitch += Input.GetAxis("JoystickRightVertical") / damp;
	yaw -= Input.GetAxis("JoystickRightHorizontal") / damp;
	
	if(pitch > pitchCeiling *(Mathf.PI/180))
	{
		pitch = pitchCeiling *(Mathf.PI/180);
	}
	else if(pitch < pitchFloor *(Mathf.PI/180))
	{
		pitch = pitchFloor *(Mathf.PI/180);
	}
}