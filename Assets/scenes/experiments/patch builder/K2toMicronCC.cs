using UnityEngine;
using System.Collections;
using MIDI;

namespace MIDI
{
	public class K2toMicronCC : MonoBehaviour 
	{
		float[] lastVals = new float[8];
		int[] ccs = new int[]{1,2,3,4,7,8,9,10};


		void trysend(int index, float value)
		{
			if (lastVals [index] != value) {
				MidiOut.SendControlChange (MidiChannel.Ch2, ccs[index], value);
				lastVals[index] = value;
				Debug.Log ("mod:"+(index+1)+" cc#:"+ccs[index]+" value:"+value);
			}
		}

		void Update()
		{
			trysend(0, CCInputManager.instance.k2Sliders [0]);
			trysend(1, CCInputManager.instance.k2Sliders [1]);
			trysend(2, CCInputManager.instance.k2Sliders [2]);
			trysend(3, CCInputManager.instance.k2Sliders [3]);
			trysend(4, CCInputManager.instance.k2Knobs [2]);

			trysend(5, CCInputManager.instance.k2Knobs [0]);
			trysend(6, CCInputManager.instance.k2Knobs [1]);
			trysend(7, CCInputManager.instance.k2Knobs [3]);




			/*for (int i=1; i<1+ lastVals.Length; i++) {
				if (lastVals[i-1] != CCInputManager.instance.k2Knobs [i-1]) {
					MidiOut.SendControlChange (CCInputManager.instance.micronChannel, i, CCInputManager.instance.k2Knobs [i-1]);
					lastVals[i-1] = CCInputManager.instance.k2Knobs [i-1];
				}
			}*/
		}
	}
}