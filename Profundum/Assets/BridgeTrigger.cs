using UnityEngine;
using System.Collections;

public class BridgeTrigger : MonoBehaviour {
	private SectionBridge _sectionBridge;
	// Use this for initialization
	public void SetSectionBridge(SectionBridge sectionBridge)
	{
		_sectionBridge = sectionBridge;
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			_sectionBridge.OnTrigger(this);
		}
	}
}
