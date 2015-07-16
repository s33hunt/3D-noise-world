using UnityEngine;
using System.Collections;

namespace Networking
{
	public class Menu
	{
		private static Menu _instance = null;
		public static Menu instance {get{return _instance ?? new Menu();}}
		private Menu(){}

		public void RenderNetworkMenu()
		{
			if (Application.platform == RuntimePlatform.Android) {
				GUI.skin.button.fontSize = 50;
			}
			if(!Manager.online){
				Manager.serverIP = GUILayout.TextField(Manager.serverIP);
				if (GUILayout.Button ("connect to server")) {Manager.Connect();}
				if (GUILayout.Button ("start server")) {Manager.StartServer();}
			}else{
				if (GUILayout.Button ("disconnect")) {Manager.Disconnect();}
			}
		}


	}
}