using UnityEngine;
using System.Collections;

public class PS3Observer : Networking.Observer 
{
	public class PS3Input : Message {}

	void Start () {syncdData = new PS3Input (); }
	
	public override void MessageHandler (string json) {}
	
	public override bool DataStreamOutHandler () {return false;}
}
