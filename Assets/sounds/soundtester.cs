using UnityEngine;
using System.Collections;

public class soundtester : MonoBehaviour {
	AudioSource source;


	void Start()
	{
		source = GetComponent<AudioSource> ();
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){source.Play ();}	
	}
}
