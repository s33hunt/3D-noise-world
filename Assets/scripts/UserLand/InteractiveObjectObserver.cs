using UnityEngine;
using System.Collections;

public class InteractiveObjectObserver : Networking.Observer 
{
	public class SyncedData : Message
	{
		public string action;
	}
	
	void Start () {syncdData = new SyncedData (); }
	
	public override void MessageHandler (string json) 
	{
		//deserialize json data
		SyncedData o = Utils.instance.Deserialize<SyncedData> (json);
		var io = GetComponent<FPVR.InteractiveObject>();

		//handle actions
		if(((SyncedData)o).action == FPVR.PS3Controller.Axes.button_cross){io.CrossButton();}
		if(((SyncedData)o).action == FPVR.PS3Controller.Axes.button_triangle){io.TriangleButton();}
		if(((SyncedData)o).action == FPVR.PS3Controller.Axes.button_square){io.SquareButton();}
		if(((SyncedData)o).action == FPVR.PS3Controller.Axes.button_circle){io.CircleButton();}
	}
}
