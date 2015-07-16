using UnityEngine;
using System.Collections;

public class SequencerButtonPlay : SequencerButton 
{
	void MouseDown()
	{
		active = true;
		sequencer.StartCoroutine ("Play");
	}
}