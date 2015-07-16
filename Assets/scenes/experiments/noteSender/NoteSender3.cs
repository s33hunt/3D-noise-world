using UnityEngine;
using System.Collections;

public class NoteSender3 : MonoBehaviour 
{
	public MidiChannel chan;
	public KeyCode targetKey;
	public int note;


	void Update()
	{
		if (Input.GetKeyDown (targetKey)) {
			MidiOut.SendNoteOn(chan, note, 1f);
		}
		if (Input.GetKeyUp (targetKey)) {
			MidiOut.SendNoteOff(chan, note);
		}
	}
}