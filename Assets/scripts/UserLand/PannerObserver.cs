using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PannerObserver : Networking.Observer 
{
	public class PannerData : Message
	{
		public bool left;
	}

	List<Panner> panners = new List<Panner>();


	void Start () {
		if(Application.platform == RuntimePlatform.Android){sendAtIntervals = false;}
		syncdData = new PannerData (); 
		foreach (Panner p in GetComponentsInChildren<Panner>()) {panners.Add(p);}
	}
	
	public override void MessageHandler (string json) //handle incoming
	{
		GetComponent<Panner> ().SetPanTarget ();
	}
}