#pragma strict
private var x_move:float;
private var y_move:float;
public var cam:GameObject;
private var stride:boolean = false;
private var speed:float = 0;
public var maxRunSpeed:float = 5;
public var maxWalkSpeed:float = 2.5;
public var friction:float = 1.20;
public var stopFriction:float = 1.05;
private var bodyAngle:Vector3 = new Vector3(0, 0, 0);
private var angleVector:Vector3 = new Vector3(0, 0, 0);
private var flipping:boolean = false;
private var prev_control_vector:Vector3 = new Vector3();
private var control_vector_delta:float;
private var prev_control_vector_delta:float;
private var control_angle_delta:float;
public var stateMachine:ZeldaStateMachine;
private var anim:Animator;
function Start () {
	anim = gameObject.GetComponent(Animator);
}

function Update () {

	//

	if(stateMachine.currentState != States.Idle && stateMachine.currentState != States.Walking)
	{
		return;
	}
	
	x_move = Input.GetAxis("JoystickLeftHorizontal"); 
	y_move = Input.GetAxis("JoystickLeftVertical"); 
	
	var cam_delta_x:float = cam.transform.position.x - GetComponent.<Rigidbody>().transform.position.x;
	var cam_delta_z:float = cam.transform.position.z - GetComponent.<Rigidbody>().transform.position.z;
	
	var cam_angle:float = Mathf.Atan2(cam_delta_x, cam_delta_z);

	var control_angle:float = Mathf.Atan2(y_move, x_move) + 1.5;
	var control_vector:float = Mathf.Sqrt(x_move * x_move + y_move * y_move);
	var control_angle_vector:Vector3 = new Vector3(x_move, 0, y_move);
	if(prev_control_vector !=null)
	{
		control_angle_delta = Vector3.Angle(control_angle_vector, prev_control_vector);
	}
	control_vector_delta = Mathf.Abs(prev_control_vector_delta - control_vector);
	prev_control_vector_delta = control_vector;
	prev_control_vector = control_angle_vector;
	
	var angle:float = cam_angle+control_angle;
	angleVector.Set(Mathf.Cos(angle), 0, Mathf.Sin(angle));
	
	var runRange:float = 1;
	var walkRange:float = 0.25;
	var turnRange:float = 0.03;
	
	var running:boolean = control_vector >= runRange ? true:false;
	var walking:boolean = !running && control_vector >walkRange;
	var turning:boolean = !walking && !running && control_vector > turnRange;
	

	var slerpSpeed:float = 0.0;
	stride = false;
	var vel:float = 0;
	if(running)
	{
		//vel = control_vector/20;
		if(speed>maxRunSpeed / friction) 
		{
			speed = maxRunSpeed;
			stride = true;
		}
		slerpSpeed = 0.1;
	}
	else if(walking)
	{
		//vel = control_vector/50;
		if(speed>maxWalkSpeed) 
		{
			
			stride = true;
		}
		slerpSpeed = 0.05;
		//speed = 0.5;
	}
	else if(turning)
	{	
		slerpSpeed = 0.01;
	 	//speed = 0;
	}
	else
	{
		//speed = 0;
	}
	if(control_vector >=1)
	{
		stride = true;
	}
	else
	{
		stride = false;
	}
	if(!turning) vel = (control_vector - walkRange) / 20;
	var targetDir:Vector3 = bodyAngle - angleVector;
    var forward:Vector3 = transform.forward;
    var a:float = Vector3.Angle(bodyAngle, angleVector);
    
    if (a < 10F || stride)
    {
    	//speed += vel;
    	speed = vel*40;
    }
    
    if (a < 5F)
    {
    	flipping = false;
    }
    
    
    /*if(a>150 && !stride)
    {
    	flipping = true;
    }*/
  	//if(control_vector_delta > 0.5 
    
    if(flipping)
    {
    	slerpSpeed = 0.4;
    	//speed=0;
    	//bodyAngle = angleVector;
    }
            
	if(control_vector < turnRange)
	{
		speed= speed / stopFriction;
	}
	
	speed = (speed) / friction;
	bodyAngle = Vector3.Slerp(bodyAngle, angleVector, slerpSpeed);
	
	
	
	var deltaAngle = Vector3.AngleBetween(bodyAngle, angleVector);
	
	//Debug.Log(control_angle_delta);
	
	
	
	var strength:float = Mathf.Sqrt(x_move * x_move + y_move * y_move) /10;
	//rigidbody.AddForce(Vector3.up * 20, ForceMode.VelocityChange);
	//rigidbody.transform.Translate(new Vector3(-Mathf.Sin(angle)*strength, 0, -Mathf.Cos(angle)*strength));	
	//rigidbody.transform.Translate(new Vector3(0, 0, speed / 10));
	//rigidbody.AddRelativeForce(transform.forward * speed, ForceMode.Impulse);
	//var  q:Quaternion = Quaternion.LookRotation(new Vector3(rigidbody.transform.rotation.x, Mathf.Atan2(bodyAngle.y, bodyAngle.x), rigidbody.transform.rotation.z));
	//rigidbody.transform.rotation = q;
	anim.SetFloat("speed", speed);
	var newVel:Vector3 = (transform.forward * (speed * 5));//override Z and X movement
	newVel.y = GetComponent.<Rigidbody>().velocity.y;//retain gravity values
	GetComponent.<Rigidbody>().velocity =  newVel;
	GetComponent.<Rigidbody>().transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(bodyAngle.z, bodyAngle.x)/(Mathf.PI/180)+180, Vector3.up);
	
	if(running || walking)
	{
		stateMachine.ChangeState(States.Walking);
	}
	else
	{
		stateMachine.ChangeState(States.Idle);
	}
}
public function Jump(strength:float)
{
	stateMachine.ChangeState(States.Jumping);
	transform.position = transform.position + Vector3.up * 0.5;
	GetComponent.<Rigidbody>().velocity = Vector3.zero;
	GetComponent.<Rigidbody>().AddForce(Vector3.up * strength + transform.forward * strength, ForceMode.VelocityChange);
}
function OnCollisionEnter(collision:Collision)
{
	if(stateMachine.currentState == States.Jumping && (collision.other.tag == "Ground" || collision.other.tag == "Bridge"))
	{
		stateMachine.ChangeState(States.Idle);
	}
}
function UpdateState(state:States)
{
	if(state == States.Jumping)
	{
		GetComponent.<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
	}
	else
	{
		GetComponent.<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}
	//rigidbody.freezeRotation = state == States.Jumping;
}