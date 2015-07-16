using UnityEngine;
using System.Collections;

public class NoteSender : MonoBehaviour 
{
	public MidiChannel chan;
	public int bpm = 120, maxnote = 50, minnote = 0;
	bool rising = true;


	IEnumerator Start()
	{
		int i = minnote;
		while (true) {
			MidiOut.SendNoteOn(chan, i, 1f);

			yield return new WaitForSeconds(60f/(float)bpm);

			MidiOut.SendNoteOff(chan, i);
			i += (rising ? 1 : -1);
			if(i >= maxnote){rising = false;}
			if(i <= minnote){rising = true;}
		}

	}
}