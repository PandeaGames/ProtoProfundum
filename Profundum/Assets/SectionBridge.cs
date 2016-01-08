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
	public string respawnLevel = "";

	private GameSection _currentGameSection;
	private GameObject _sectionToDestroy;
	private BridgeTrigger _bridgeTriggerA;
	private BridgeTrigger _bridgeTriggerB;

	private bool _deletionComplete, _doDeletion, _hasLoadedLevel;
	private Game _game;
	
	// Use this for initialization
	void Start () {
		_bridgeTriggerA = triggerA.GetComponent<BridgeTrigger> ();
		_bridgeTriggerB = triggerB.GetComponent<BridgeTrigger> ();

		_bridgeTriggerA.SetSectionBridge (this);
		_bridgeTriggerB.SetSectionBridge (this);

		if (currentSection) {
			_currentGameSection = currentSection.GetComponent<GameSection>();
		}

		_game = FindObjectOfType<Game> ();
	}
	
	void LateUpdate()
	{
		if (_deletionComplete) 
		{
			FindObjectOfType<Game>().BroadcastMessage("ResetSceneData");
			_deletionComplete = false;
		}
		if (_doDeletion) {
			Destroy (_sectionToDestroy);
			
			Vector3 levelOffset = (GetState() ? anchorA.transform.position: anchorB.transform.position) - (GetState() ? _currentGameSection.anchorB.transform.position: _currentGameSection.anchorA.transform.position);

			currentSection.transform.position = currentSection.transform.position + levelOffset;
			
			//Tell's all controllers to reset thier lists and static data to account for new level loaded. 
			_deletionComplete = true;

			activeTrigger = GetState() ? triggerB : triggerA;

			_hasLoadedLevel = false;
			_doDeletion = false;

			if(respawnLevel!="")
				_game.respawnScene = respawnLevel;
		}
		if (_hasLoadedLevel) {
			_doDeletion = true;
		}
	}
	public void OnTrigger(BridgeTrigger trigger)
	{
		if (trigger.gameObject != activeTrigger ) {
			return;
		}
		_hasLoadedLevel = true;

		_sectionToDestroy = currentSection;
		currentSection = Instantiate (activeTrigger == triggerA ? sectionA : sectionB);
		currentSection.transform.parent = _game.transform;

		_currentGameSection = currentSection.GetComponent<GameSection>();
		
		_game.BroadcastMessage("ClearSceneData");


	}
	private bool GetState()
	{
		return activeTrigger == triggerA;
	}
}
