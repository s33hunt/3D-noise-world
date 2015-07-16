using UnityEngine;
using System.Collections;

public class rename2 : MonoBehaviour 
{
	void Update () 
	{
		int a = Mathf.RoundToInt (6f * CCInputManager.instance.k2Sliders [0]);
		rename1.desiredGenerations = a;
	}
}
