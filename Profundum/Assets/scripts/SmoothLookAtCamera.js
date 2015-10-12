#pragma strict

function Start () {
	var smoothLook:SmoothLookAt;
	smoothLook = gameObject.GetComponent("SmoothLookAt");
	smoothLook.target = Camera.main.transform;
}

function Update () {

}