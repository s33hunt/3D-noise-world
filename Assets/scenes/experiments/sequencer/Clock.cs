using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour 
{
	public static Clock instance;
	public delegate void OnBeat();
	public delegate void OnTick();
	public OnBeat onBeat;
	public OnTick onTick;
	public int 
		beatsPerMinute = 120,
		ticksPerBeat = 4;


	IEnumerator Start()
	{
		instance = this;

		while (true) {
			if(onTick != null){ onTick();}
			yield return new WaitForSeconds (60f / (float)(beatsPerMinute*ticksPerBeat));
		}
	}
}
