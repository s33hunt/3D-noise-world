using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerObserver : Networking.Observer 
{
	//create message definition 
	public class GyroData : Message
	{
		public Vector3 position;
		public Quaternion cameraOrientation;
	}

	//must set syncdData in derivation so base methods have something to sync
	void Start () {syncdData = new GyroData (); }

	//this is where you control what happens on sync 
	public override void MessageHandler (string json) 
	{
		if (Application.platform != RuntimePlatform.Android) {
			GyroData o = Utils.instance.Deserialize<GyroData> (json);
			FPVR.Player.instance.camLerpTarget = o.cameraOrientation;
			FPVR.Player.instance.bodyPosition = o.position;
		}
	}

	//this is what happens onsend for interval messages
	//if it returns false the datastream will stop
	public override bool DataStreamOutHandler () 
	{
		if (Networking.Manager.online && Application.platform == RuntimePlatform.Android) { 
			((GyroData)syncdData).cameraOrientation = FPVR.Player.instance.camera.transform.rotation;
			((GyroData)syncdData).position = FPVR.Player.instance.body.position;
			Sync ();
		} else { 
			if (Network.peerType != NetworkPeerType.Disconnected) {
				Debug.Log ("stopping gryo sync");
				return false;
			}
		}

		return true;
	}
}
