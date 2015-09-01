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

	void OnDestroy()
	{
		MidiChannel[] channels = new MidiChannel[16]{
			MidiChannel.Ch1,
			MidiChannel.Ch2,
			MidiChannel.Ch3,
			MidiChannel.Ch4,
			MidiChannel.Ch5,
			MidiChannel.Ch6,
			MidiChannel.Ch7,
			MidiChannel.Ch8,
			MidiChannel.Ch9,
			MidiChannel.Ch10,
			MidiChannel.Ch11,
			MidiChannel.Ch12,
			MidiChannel.Ch13,
			MidiChannel.Ch14,
			MidiChannel.Ch15,
			MidiChannel.Ch16
		};

		for (int i=0; i<120; i++) {
			foreach(MidiChannel c in channels){
				MidiOut.SendNoteOff(c,i);
			}
		}
	}
}