using UnityEngine;
using System.Collections;

public class SectionBridge : MonoBehaviour {
	public GameObject currentSection;
	public GameObject activeTrigger;
	public GameObject sectionA;
	public GameObject sectionB;
	public GameObject triggerA;
	public GameObject triggerB;
	public GameObject anchorA;
	public GameObject anchorB;

	private SectionStreamingController _ssc;
	private BridgeTrigger _bridgeTriggerA;
	private BridgeTrigger _bridgeTriggerB;

	// Use this for initialization
	void Start () {
		_ssc = FindObjectOfType<SectionStreamingController> ();

		_bridgeTriggerA = triggerA.GetComponent<BridgeTrigger> ();
		_bridgeTriggerB = triggerB.GetComponent<BridgeTrigger> ();

		_bridgeTriggerA.SetSectionBridge (this);
		_bridgeTriggerB.SetSectionBridge (this);
	}
	public void OnTrigger(BridgeTrigger trigger)
	{
		_ssc.Trigger (trigger == _bridgeTriggerA ? SectionStreamingController.TRIGGER_A : SectionStreamingController.TRIGGER_B, gameObject);
	}
	private bool GetState()
	{
		return activeTrigger == triggerA;
	}
}
