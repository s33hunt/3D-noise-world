using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Networking
{
	public class Observer : MonoBehaviour //this is the base class for observer components they get unique id's and send data throught Networking.Manager's networkView. use this component's derivations on instead of networkViews 
	{
		//sync rate
		public bool sendAtIntervals = false;
		[Range(1,35)] public int networkDataFrameRate = 24;
		private float frameTime = 1;
		[HideInInspector] public bool streaming = false;
		//base message class. extend this class in the derivations of Observer to create json objects for syncdData
		public class Message{} 
		public int ID; 
		static int IDCounter = 0;
		public static Dictionary<int, Observer> instances = new Dictionary<int, Observer>();
		//data class to be shared over network
		public Message syncdData; 


		void GenerateID(){ID = IDCounter++; instances.Add (ID, this);}

		void Awake()
		{
			GenerateID ();
			frameTime = 1f / networkDataFrameRate;
			if(sendAtIntervals){StartDataStream();}
		}

		public void Sync(Message data){_Sync (data);}
		public void Sync(){_Sync (syncdData);}
		void _Sync(object data)
		{
			Networking.Manager.SendObserverMessage (ID, data);
		}

		public virtual void MessageHandler(string json)//handle incoming update messages
		{
			//override this method to decode JSON to correct message format (see JSONObjectDefinitions) and then handle the incoming data
		}

		public virtual bool DataStreamOutHandler()//handle incoming update messages
		{
			return true;
		}

		public void StartDataStream(){streaming = true; StartCoroutine("DataStreamUpdate");}
		public void StopDataStream(){streaming = false; StopCoroutine("DataStreamUpdate");}

		private IEnumerator DataStreamUpdate()
		{
			while (true) {
				if (!DataStreamOutHandler ()){streaming = false; break;}
				yield return new WaitForSeconds(frameTime);
			}
		}
	}
}