using UnityEngine;
using System.Collections;

public class CCInputManager : MonoBehaviour 
{
	public class XYPad{public float x=0, y=0;}


	//Mono Singleton
	public static CCInputManager instance;

	public MidiChannel 
		k2Channel,
		micronChannel;
	/*int 
		_k2Channel,
		_micronChannel;*/
	public enum XYZKnobRange {full, partial}
	public XYZKnobRange xyzRange; 
	public XYPad xyPad = new XYPad();
	public float 
		xyz = 0,
		m1 = 0,
		m2 = 0,
		pitchBender = 0f;
	[Range(0f,1f)] public float[] 
		k2Knobs = new float[8],
		k2Sliders = new float[8];
	public bool[]
		k2Sbtns = new bool[8],
		k2Mbtns = new bool[8],
		k2Rbtns = new bool[8];
	public bool
		k2BackTrack = false,
		k2ForwardTrack = false,
		k2Cycle = false,
		k2Set = false,
		k2BackMarker = false,
		k2ForwardMarker = false,
		k2Rewind = false,
		k2FastForward = false,
		k2Stop = false,
		k2Play = false,
		k2Record = false;

	float 
		lastXYZ6 = 0,
		lastXYZ38 = 0;


	void Start () {
		instance = this; 
		MidiInput.instance.OnCC += handler;
		//MIDI.Kontrol2.channel = k2Channel;
	}

	void handler(MidiBridge.Message m)
	{


		//k2
		if (m.channel == (int)k2Channel) {
			//Debug.Log (m.ToString ());

			//K2 sliders
			if(m.data1 >= 0 && m.data1 <= 7){k2Sliders[m.data1] = m.data2 / 127f;}
			//K2 Knobs
			if(m.data1 >= 16 && m.data1 < 24){k2Knobs[m.data1 - 16] = m.data2 / 127f;}
			//K2 S btns
			if(m.data1 >= 32 && m.data1 < 40){k2Sbtns[m.data1 - 32] = m.data2 == 0x7f;}
			//K2 M btns
			if(m.data1 >= 48 && m.data1 < 56){k2Mbtns[m.data1 - 48] = m.data2 == 0x7f;}
			//K2 R btns
			if(m.data1 >= 64 && m.data1 < 72){k2Rbtns[m.data1 - 64] = m.data2 == 0x7f;}
			//left btns
			if(m.data1 == 0x3a){k2BackTrack = m.data2 == 0x7f;}
			if(m.data1 == 0x3b){k2ForwardTrack = m.data2 == 0x7f;}
			if(m.data1 == 0x2e){k2Cycle = m.data2 == 0x7f;}
			if(m.data1 == 0x3c){k2Set = m.data2 == 0x7f;}
			if(m.data1 == 0x3d){k2BackMarker = m.data2 == 0x7f;}
			if(m.data1 == 0x3e){k2ForwardMarker = m.data2 == 0x7f;}
			if(m.data1 == 0x2b){k2Rewind = m.data2 == 0x7f;}
			if(m.data1 == 0x2c){k2FastForward = m.data2 == 0x7f;}
			if(m.data1 == 0x2a){k2Stop = m.data2 == 0x7f;}
			if(m.data1 == 0x29){k2Play = m.data2 == 0x7f;}
			if(m.data1 == 0x2d){k2Record = m.data2 == 0x7f;}

		}

		//micron
		if(m.channel == (int)micronChannel){
			//Debug.Log ("micron "+m.ToString ());
			if (m.status == 0xb0) {HandleXYPad(m);}
			if (m.status == 0xd1) {HandleM2(m);}
			if (m.status == 0xe1) {HandlePitchBender(m);}
			if (m.status == 0xb1) { //m1 slider, x,y,z knobs
				if(m.data1 == 1){HandleM1(m);}
				if(m.data1 == 6 || m.data1 == 38){HandleXYZ(m);}
			}
		}
	}

	void HandleM1 (MidiBridge.Message m) {m1 = m.data2 / 127f;}
	void HandleM2 (MidiBridge.Message m) {m2 = m.data1 / 127f;}
	void HandlePitchBender (MidiBridge.Message m) {pitchBender = ((m.data2 / 127f) - 0.5f) * 2f;}
	void HandleXYPad (MidiBridge.Message m) 
	{
		if(m.data1 == 1){xyPad.x = m.data2 / 127f;}
		if(m.data1 == 2){xyPad.y = m.data2 / 127f;}
	}
	void HandleXYZ(MidiBridge.Message m)
	{
		if(m.data1 == 6){lastXYZ6 = m.data2;}
		if(m.data1 == 38){lastXYZ38 = m.data2;}
		Debug.Log (lastXYZ6 + ":" + lastXYZ38);
		if (xyzRange == XYZKnobRange.full) {
			xyz = (lastXYZ6 / 8f) + ((lastXYZ38 / 127f) / 8f);
		} else {
			xyz = (lastXYZ38 / 100f);
		}
	}
}