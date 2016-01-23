using UnityEngine;
using System.Collections;

public class SectionStreamingController : MonoBehaviour {
	public static uint TRIGGER_A = 0;
	public static uint TRIGGER_B = 1;

	public GameObject currentSection;
	public GameObject bridgeA;
	public GameObject bridgeB;

	public StreamingSection currentStreamingSection;

	private StreamingSection[] _streamingSections;
	private StreamingSection _newStreamingSection;
	private GameObject _transitioningBridge;
	private GameSection _currentGameSection;
	private Vector3 offset;
	private Vector3 bridgeOffset;

	private GameObject _newBridge;
	private GameObject _sectionToDestroy;

	private bool _deletionComplete, _doDeletion, _hasLoadedLevel;
	private Game _game;
	// Use this for initialization
	void Start () {
		_game = FindObjectOfType<Game> ();
		_streamingSections = GetComponents<StreamingSection> ();
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

			currentSection.transform.position = currentSection.transform.position + offset;
			_newBridge.transform.position = _newBridge.transform.position + bridgeOffset;
			//Tell's all controllers to reset thier lists and static data to account for new level loaded. 
			_deletionComplete = true;

			_hasLoadedLevel = false;
			_doDeletion = false;
			
			//if(respawnLevel!="")
			//	_game.respawnScene = respawnLevel;
		}
		if (_hasLoadedLevel) {
			_doDeletion = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private int IndexOf(StreamingSection section)
	{
		for(int i=0; i<_streamingSections.Length;i++)
		{
			if(_streamingSections[i] == section)
			{
				return i;
			}
		}
		return -1;
	}
	public void Trigger(uint trigger, GameObject bridge)
	{
		if (trigger == TRIGGER_A && bridge == bridgeB)
			return;
		if (trigger == TRIGGER_B && bridge == bridgeA)
			return;

		_transitioningBridge = bridge;
		_sectionToDestroy = currentSection;
		if (trigger == TRIGGER_A) 
		{
			//at the end of the game
			if(IndexOf(currentStreamingSection) == _streamingSections.Length - 1) return;
			_newStreamingSection = _streamingSections[IndexOf(currentStreamingSection)+1];
			currentSection = _newStreamingSection.CreateSection();
			_newBridge = Instantiate(_newStreamingSection.bridgeA);
			Destroy (bridgeB);
			bridgeB = bridgeA;
			bridgeA = _newBridge;
			offset = bridgeB.GetComponent<SectionBridge>().anchorA.transform.position - currentSection.GetComponent<GameSection>().anchorB.transform.position;
			bridgeOffset = offset + (currentSection.GetComponent<GameSection>().anchorA.transform.position - bridgeA.GetComponent<SectionBridge>().anchorB.transform.position);
		} 
		else if (trigger == TRIGGER_B)  
		{
			//at the beginning of the game
			if(IndexOf(currentStreamingSection) == 0) return;
			_newStreamingSection = _streamingSections[IndexOf(currentStreamingSection)-1];
			currentSection = _newStreamingSection.CreateSection();
			_newBridge = Instantiate(_newStreamingSection.bridgeB);
			Destroy (bridgeA);
			bridgeA = bridgeB;
			bridgeB = _newBridge;

			offset = bridgeA.GetComponent<SectionBridge>().anchorB.transform.position - currentSection.GetComponent<GameSection>().anchorA.transform.position;
			bridgeOffset = offset + (currentSection.GetComponent<GameSection>().anchorB.transform.position - bridgeB.GetComponent<SectionBridge>().anchorA.transform.position);
		}

		_hasLoadedLevel = true;

		currentSection.transform.parent = _game.transform;
		currentStreamingSection = _newStreamingSection;

		_currentGameSection = currentSection.GetComponent<GameSection>();
		
		_game.BroadcastMessage("ClearSceneData");
	}
}
