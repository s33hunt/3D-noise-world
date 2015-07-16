using UnityEngine;
using System.Collections;

public class GAME : MonoBehaviour 
{
	public static GAME instance;

	public enum CurrentMenu { None, Main }
	[HideInInspector] public CurrentMenu menu = CurrentMenu.Main;

	public GUISkin mainSkin, errorSkin;
	public GameObject[] killIfAndroid;

	public struct Layers {
		public static int 
			Player = 8,
			Interactive = 9;
	}


	void Awake()
	{
		instance = this;

		if (Application.platform == RuntimePlatform.Android) {
			for (int i=0; i<killIfAndroid.Length; i++) {
				Destroy (killIfAndroid [0]);
			}
		}
	}

	void OnGUI()
	{

		Networking.Menu.instance.RenderNetworkMenu ();
		/*if(Application.platform != RuntimePlatform.Android){
			MIDI.MicronController.instance.RenderVoicesLoaded ();
			if(GUILayout.Button("Save wave files for active voice")){
				MIDI.MicronController.instance.activeVoice.SaveVoiceToFiles();
			}
		}*/


	}
}
