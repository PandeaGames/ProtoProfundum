using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cinematics : MonoBehaviour {
	public GameObject darkFade;

	public bool faded = true;
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
	public void FadeIn()
	{
		Color c = darkFade.GetComponent<Image>().color;
		c.a += 1;
		darkFade.GetComponent<Image>().color = c;

		faded = false;
	}
	public void FadeOut()
	{
		Color c = darkFade.GetComponent<Image>().color;
		c.a = 0;
		darkFade.GetComponent<Image>().color = c;

		faded = true;
	}
}
