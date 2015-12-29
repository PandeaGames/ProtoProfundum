using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SightUI : MonoBehaviour {
	public GameObject sightIcon;
	public GameObject sightBar;

	private SightController _sc;
	// Use this for initialization
	void Start () {
		_sc = FindObjectsOfType<SightController> ()[0];
	}
	
	// Update is called once per frame
	void Update () {
		sightBar.transform.localScale = new Vector3 (1-_sc.GetSight(), 1, 1);

		//sightIcon.GetComponent<SpriteRenderer>().enabled = _sc.IsInSight ();

		Color c = sightIcon.GetComponent<Image>().color;
		c.a = _sc.IsInSight () ? 1:0.3f;
		sightIcon.GetComponent<Image>().color = c;
	}
}
