using UnityEngine;
using System.Collections;

public class ResetPathTrigger : MonoBehaviour {
	public GameObject pathGroupObj;
	public float place = 0;//0-1;
	public string collisionTag;
	public bool repeat = false;

	private bool _hasTriggered = false;
	private PathGroup _pathGroup;
	// Use this for initialization
	void Start () {
		_pathGroup = pathGroupObj.GetComponent<PathGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == collisionTag && !(repeat && _hasTriggered) ) 
		{
			_hasTriggered = true;
			_pathGroup.SetPlace(place);
		}
	}
}
