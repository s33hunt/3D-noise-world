using UnityEngine;
using System.Collections;

public class Cubinator : MonoBehaviour 
{
	Renderer renderer;


	void Start()
	{
		renderer = gameObject.GetComponent<Renderer> ();
	}

	void Update()
	{
		renderer.material.color = new Color (
			CCInputManager.instance.k2Sliders[4],
			CCInputManager.instance.k2Sliders [5], 
			CCInputManager.instance.k2Sliders [6], 
			CCInputManager.instance.k2Sliders [7]
		);
	}
}