using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BloodUI : MonoBehaviour {
	private Image image;
	private PlayerHealthController playerHealthController;
	// Use this for initialization
	void Start () {
		playerHealthController = FindObjectOfType<PlayerHealthController> ();
		image = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update () 
	{
		Color c = image.color;
		c.a = 1-playerHealthController.getHealthRatio();
		image.color = c;

	}
}
