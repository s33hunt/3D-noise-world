using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sequencer
{
	[RequireComponent(typeof(IndicatorLights))]
	[RequireComponent(typeof(Colors))]
	public class Sequencer : MonoBehaviour 
	{

		Units _units;
		public Units units {get{return _units ?? new Units(this);}}
		[HideInInspector] public bool playing = false;
		[HideInInspector] public IndicatorLights lights;
		[HideInInspector] public Colors colors;
		[HideInInspector] public int height;
		public delegate void OnMeasure();
		public delegate void OnBeat();
		public OnMeasure onBeat;
		public OnMeasure onMeasure;
		public MidiChannel[] channelMap;
		public int[] keyMap;
		public int 
			width = 8, 
			beatResolution = 4;
		private Grid noteGrid;
		private List<Grid> FXGrids = new List<Grid>();
		private List<int> playedNotes = new List<int>();
		private int 
			tickCount = 0, 
			currentTick = 0;


		void Start()
		{
			//init 
			height = keyMap.Length;
			lights = GetComponent<IndicatorLights> ();
			colors = GetComponent<Colors> ();

			//create note grid
			noteGrid = gameObject.AddComponent<Grid> ();
		}

		public void Play(){
			Clock.instance.onTick += OnTick;
		}
		public void Stop(){
			ReleasePlayedNotes ();
			lights.AllOff ();
			Clock.instance.onTick -= OnTick;
		}
		public void ResetPlayhead()
		{
			tickCount = 0;
		}
		
		void RandomizeEnabled()
		{
			//foreach(GridButton b in gridButtons){
				//if(b.randomize){b.RandomEnabled();}
			//}
		}

		void OnTick()
		{
			//reset from last tick
			//ReleasePlayedNotes ();
			//indicatorLights[currentTick].material.color = indicatorColor;
			
			//update pointers for current tick
			currentTick = tickCount % width;
			tickCount++;
			
			
			if(currentTick == 0){//per measure
				RandomizeEnabled();
				if(onMeasure != null){onMeasure();}
			}
			if(currentTick % Clock.instance.ticksPerBeat == 0){//per beat
				if(onBeat != null){ onBeat();}
			}
			
			//per tick
			//indicatorLights[currentTick].material.color = indicatorColorActive;

			//fx grid loop
			for(int h=0; h<noteGrid.height; h++){//per selected
				if(noteGrid.gridButtons[currentTick,h].enabled){
					playedNotes.Add(h);
					MidiChannel ch = (channelMap.Length > 1 ? channelMap[h] : channelMap[0]);
					MidiOut.SendNoteOn(ch, keyMap[h], 1f);
				}
			}
			//fx grid loops
		}

		
		void ReleasePlayedNotes()
		{
			while(playedNotes.Count > 0){//after selected
				MidiOut.SendNoteOff((channelMap.Length > 1 ? channelMap[playedNotes[0]] : channelMap[0]), keyMap[playedNotes[0]]);
				playedNotes.Remove(playedNotes[0]);
			}
		}

		public class Units
		{
			public float 
				buttonWidth,
				gridHeight,
				margin = 0.002f, 
				oneFoot = 0.3048f;

			public Units(Sequencer sequencer){
				buttonWidth = (oneFoot - (margin * (16 - 1))) / 16;
				gridHeight = -((sequencer.height * buttonWidth) + (sequencer.height * margin) - margin);
			}
		}
	}
}