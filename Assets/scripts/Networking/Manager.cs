using UnityEngine;
using System.Collections;

namespace Networking
{
	public class Manager : MonoBehaviour
	{
		public static NetworkView networkView;
		public static bool online  {get {return Network.peerType != NetworkPeerType.Disconnected;}}
		public static string serverIP;
		private static int port = 6666;


		void Start()
		{
			serverIP = Network.player.ipAddress;
			networkView = GetComponent<NetworkView> ();
		}

		public static void StartServer(){Network.InitializeServer (10, port, false);}
		public static void Connect(){Network.Connect (serverIP, port);}
		public static void Disconnect(){Network.Connect (serverIP, port);}

		public static void SendObserverMessage(int id, object data)
		{
			if (online) {
				string json = Utils.instance.Serialize (data);
				networkView.RPC ("RecieveMessage", RPCMode.Others, id, json);
			} 
		}

		[RPC] void RecieveMessage(int id, string json)
		{
			if(Observer.instances.ContainsKey(id)){
				Observer.instances[id].MessageHandler(json);
			}
		}
	}
}