using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class LightTargetDisplay : StateBehaviour {
	//Declare which states we'd like use
	public enum LightStates
	{
		ShootReady, 
		ShootRecovery, 
		Leaving, 
		Recovering
	}
	public AnimationCurve recoveryCurve;
	public GameObject sphere;
	public float maxIntensity = 5;
	public float maxRadius = 0.05f;
	public float maxRange = 3;
	public float leavingIntensity = 10;
	public float leavingRadius = 2;
	public float leavingRange = 8;

	private float _flareBrightness;
	private HeroShooter shooter;
	public GameObject target;
	private Light _light;
	private LensFlare _flare;
	private ProfundumPlayerHealth playerHealth;

	// Use this for initialization

	void Awake()
	{
		Initialize<LightStates>();
		
		//Change to our first state
		ChangeState(LightStates.ShootRecovery);
	}
	void Start () {
		_flare = GetComponent<LensFlare> ();
		_light = GetComponentInChildren<Light> ();
		shooter = FindObjectOfType<HeroShooter> ();

		_flareBrightness = _flare.brightness;
		playerHealth = FindObjectOfType<ProfundumPlayerHealth> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (playerHealth.death) {
			ChangeState(LightStates.Leaving);
		}
	}
	void Leaving_Enter()
	{
		transform.Rotate (new Vector3 (0, 90, 0));
		FindObjectOfType<RoachController> ().LightLaunched (this.gameObject);
		_light.range = leavingRange;
		_light.intensity = leavingIntensity;
		sphere.transform.localScale = new Vector3 (leavingRadius, leavingRadius, leavingRadius);
	}
	void Leaving_Update()
	{
		GetComponent<Rigidbody> ().AddForce (new Vector3 (0, 0.03f), ForceMode.Impulse);
		//transform.position = new Vector3 (transform.position.x, transform.position.y + 0.05f, transform.position.z);
	}
	void ShootReady_Update()
	{
		transform.position = target.transform.position;
		if (!shooter.CanShoot ()) {
			ChangeState(LightStates.ShootRecovery);
		}
	}
	void ShootRecovery_Update()
	{
		transform.position = target.transform.position;
		_flare.brightness = _flareBrightness * recoveryCurve.Evaluate (shooter.GetRecoveringProgress ());
		_light.range = maxRange * recoveryCurve.Evaluate (shooter.GetRecoveringProgress ());
		_light.intensity = maxIntensity * recoveryCurve.Evaluate (shooter.GetRecoveringProgress ());
		sphere.transform.localScale = new Vector3(maxRadius * recoveryCurve.Evaluate (shooter.GetRecoveringProgress ()), 
		maxRadius * recoveryCurve.Evaluate (shooter.GetRecoveringProgress ()), 
		maxRadius * recoveryCurve.Evaluate (shooter.GetRecoveringProgress ()));
		if (shooter.CanShoot ()) 
		{
			ChangeState(LightStates.ShootReady);
		}
	}
}
