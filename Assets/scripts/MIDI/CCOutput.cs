using UnityEngine;
using System.Collections;


/* * * * * * * * * * * * * * * * * * * * * * * * *
 * 
 * 			CONTINUOUS CONTROLLER
 * 
 * Use this component to send cc messages
 * 
 * * * * * * * * * * * * * * * * * * * * * * * * */

public class CCOutput : MonoBehaviour 
{
	public enum SignalType { SplitSignalSigned, Normal }
	public enum MathType {
		//get signal value from:
		Sin, 			//sin oscilator
		Saw, 			//saw oscilator
		Square, 		//square oscilator
		Rand, 			//random value
		Continuous, 	//single value
		ExtIn 			//other script
	}

	public SignalType signalType;
	public MathType mathType;
	public bool 
		startImmediately = true, 
		autoSend = true,
		stepped = false;
	[Range(0.02f, 100)] public float frequency = 0;
	[Range(0, 1)] public float signalValue = 0;
	public int[] ccSignalNumbers;
	public MidiChannel[] channels;

	float 
		sinCounter = 0,
		lastFrequency = 0,
		lastval = -1;


	void Start()
	{
		if (ccSignalNumbers.Length < 1) {Debug.LogWarning ("you have not specified any cc numbers, no messages will be sent to midi out");}
		if (startImmediately) {
			StartCoroutine ("UpdateLoop");
		} else {
			autoSend = false;
		}
	}

	void Update()
	{
		if (frequency != lastFrequency) 
		{
			StopCoroutine ("UpdateLoop");
			StartCoroutine ("UpdateLoop");
			lastFrequency = frequency;
		}
	}

	IEnumerator UpdateLoop()
	{
		while (true) {
			if (ccSignalNumbers.Length > 0 && autoSend) 
			{
				//SIGNAL MATH
				if (mathType == MathType.Sin) {
					sinCounter += Time.deltaTime * frequency * Mathf.PI;
					signalValue = (Mathf.Sin (sinCounter) * 0.5f) + 0.5f;
				} else if (mathType == MathType.Square) {
					sinCounter += Time.deltaTime * frequency * Mathf.PI;
					signalValue = Mathf.Sin (sinCounter) > 0 ? 1f : 0f;
				} else if (mathType == MathType.Rand) {
					stepped = true;
					signalValue = Random.Range (0f, 1f);
				}

				//SIGNAL SENDING
				if (signalType == SignalType.SplitSignalSigned) {
					SendSplitSignal (ccSignalNumbers [0], ccSignalNumbers [1]);
				} else if (signalType == SignalType.Normal) {
					SendNormalSignal ();
				}
			}

			yield return (stepped ? new WaitForSeconds(1f / frequency) : null);
		}
	}

	void SendNormalSignal () 
	{
		_SendSignal();
	}

	void SendSplitSignal(int cc1, int cc2)
	{
		if (signalValue < 0.5f) {	
			signalValue = 1f - (signalValue * 2);
			foreach(MidiChannel c in channels){
				_SendSignal(c, cc2, 0);
				_SendSignal(c, cc1, signalValue);
			}
		} else {
			signalValue = (signalValue - 0.5f)*2;
			foreach(MidiChannel c in channels){
				_SendSignal(c, cc1, 0);
				_SendSignal(c, cc2, signalValue);
			}
		}
	}

	public void SendSignal(float value)
	{
		signalValue = value;
		if(signalType == SignalType.Normal)
			_SendSignal(value);
		if(signalType == SignalType.SplitSignalSigned)
			SendSplitSignal (ccSignalNumbers [0], ccSignalNumbers [1]);
	}
	public void SendSignal(int ccNumber, MidiChannel channel, float value)
	{
		signalValue = value;
		_SendSignal(channel, ccNumber, value);
	}


	void _SendSignal(float value = -1)
	{
		foreach(int s in ccSignalNumbers)
		{
			foreach(MidiChannel c in channels){
				if(value != lastval)
					MidiOut.SendControlChange (c, s, value >= 0 ? value : signalValue);
					lastval = value;
			}
		}
	}

	void _SendSignal(MidiChannel channel, int ccNumber, float value)
	{
		if(value != lastval)
			MidiOut.SendControlChange (channel, ccNumber, value);
			lastval = value;
	}
}