using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cinematics : MonoBehaviour {
	public GameObject darkFade;

	private bool faded = true;
	// Use this for initialization
	void Start () {
		FadeIn ();
	}
	
	// Update is called once per frame
	void Update () {
		Color c = darkFade.GetComponent<Image>().color;
		c.a += faded ? 0.003f:-0.01f;
		darkFade.GetComponent<Image>().color = c;
	}
	void FadeIn()
	{
		Color c = darkFade.GetComponent<Image>().color;
		c.a += 1;
		darkFade.GetComponent<Image>().color = c;

		faded = false;
	}
	void FadeOut()
	{
		Color c = darkFade.GetComponent<Image>().color;
		c.a = 0;
		darkFade.GetComponent<Image>().color = c;

		faded = true;
	}
}
