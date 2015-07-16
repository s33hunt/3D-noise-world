using UnityEngine;
using System.Collections;

public class NoteSender2 : MonoBehaviour 
{
	public MidiChannel chan;
	public int bpm = 120, maxnote = 100, minnote = 50, inc;
	bool rising = true;
	
	IEnumerator Start()
	{
		int i = minnote;
		while (true) {
			MidiOut.SendNoteOn(chan, i, 1f);
			MidiOut.SendNoteOn(chan, i+3, 1f);
			MidiOut.SendNoteOn(chan, i+4, 1f);
			
			yield return new WaitForSeconds(60f/(float)bpm);
			
			MidiOut.SendNoteOff(chan, i);
			MidiOut.SendNoteOff(chan, i+3);
			MidiOut.SendNoteOff(chan, i+4);

			i += (rising ? inc : -inc);
			if(i >= maxnote){rising = false;}
			if(i <= minnote){rising = true;}
		}
		
	}
}