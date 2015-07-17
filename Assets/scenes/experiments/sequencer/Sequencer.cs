using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sequencer : MonoBehaviour 
{
	public enum ButtonType {Play, Stop, Empty}
	public delegate void OnMeasure();
	public delegate void OnBeat();
	public OnMeasure onBeat;
	public OnMeasure onMeasure;

	public GridButton[,] gridButtons;
	public ButtonType[] controlButtons;
	public MidiChannel[] channelMap;
	public int[] keyMap;
	public int
		bpm = 120,
		width = 8;

	[HideInInspector] public float space = 0.1f;
	Renderer[] indicatorLights;
	bool playing = false;
	List<int> playedNotes = new List<int>();
	int 
		height,
		tickCount = 0, 
		currentTick = 0;


	
	void Start()
	{
		//init 
		height = keyMap.Length;

		//control buttons
		foreach (ButtonType type in controlButtons) {
			if(type == ButtonType.Play){
				(GameObject.CreatePrimitive (PrimitiveType.Cube).AddComponent<PlayButton>()).Init(this);
			}
			if(type == ButtonType.Stop){
				(GameObject.CreatePrimitive (PrimitiveType.Cube).AddComponent<StopButton>()).Init(this);
			}
		}
		ControlButton.ResetStaticGrid ();

		//note grid
		gridButtons = new GridButton[width, height];
		indicatorLights = new Renderer[width];

		for(int w=0; w<width; w++){ //per row
			var indi = GameObject.CreatePrimitive (PrimitiveType.Cube);
			indi.name = "indicator "+w;
			indi.transform.parent = transform;
			indi.transform.localPosition = new Vector3(w + (space*w), 1 + space, 0);
			indicatorLights[w] = indi.GetComponent<Renderer>();
			indicatorLights[w].material.color = Color.black;
			Destroy(indi.GetComponent<Collider>());


			for(int h=0; h<height; h++){ //per column in row
				var go = GameObject.CreatePrimitive (PrimitiveType.Cube);
				go.name = w+":"+h;
				go.transform.parent = transform;
				go.transform.localPosition = new Vector3(w + (space*w), -((height-1)+(height*space))   + (h + (space*h)), 0);
				gridButtons[w,h] = (go.AddComponent<GridButton>()).Init(w,h);
			}
		}
	}

	public void Play(){
		Clock.instance.onTick += OnTick;
	}
	public void Stop(){
		ReleasePlayedNotes ();
		TurnOffIndicators ();
		Clock.instance.onTick -= OnTick;
	}
	public void ResetPlayhead()
	{
		tickCount = 0;
	}

	void RandomizeEnabled()
	{
		foreach(GridButton b in gridButtons){
			if(b.randomize){b.RandomEnabled();}
		}
	}
	void TurnOffIndicators()
	{
		foreach(Renderer r in indicatorLights){
			r.material.color = Color.black;
		}
	}

	void OnTick()
	{
		//reset from last tick
		ReleasePlayedNotes ();
		indicatorLights[currentTick].material.color = Color.black;

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
		indicatorLights[currentTick].material.color = Color.magenta;
		
		for(int h=0; h<height; h++){//per selected
			if(gridButtons[currentTick,h].enabled){
				playedNotes.Add(h);
				MidiChannel ch = (channelMap.Length > 1 ? channelMap[h] : channelMap[0]);
				MidiOut.SendNoteOn(ch, keyMap[h], 1f);
			}
		}
	}

	void ReleasePlayedNotes()
	{
		while(playedNotes.Count > 0){//after selected
			MidiOut.SendNoteOff((channelMap.Length > 1 ? channelMap[playedNotes[0]] : channelMap[0]), keyMap[playedNotes[0]]);
			playedNotes.Remove(playedNotes[0]);
		}
	}


	public class GridButton : MonoBehaviour 
	{
		public int x, y;
		Color baseColor = Color.white;
		bool 
			_enabled = false,
			_randomize = false;

		public bool randomize {
			get { return _randomize; }
			set { 
				baseColor = value ? Color.green : Color.white; 
				_randomize = value; 
			}
		}
		public bool enabled {
			get { return _enabled;}
			set {
				gameObject.GetComponent<Renderer>().material.color = (value ? Color.blue : baseColor);
				_enabled = value;
			}
		}


		public GridButton Init(int x, int y)
		{
			this.x = x;
			this.y = y;
			return this;
		}

		public void RandomEnabled()
		{
			enabled = Random.Range(0, 2) >= 1 ? true : false;
		}

		void OnMouseDown(){
			enabled = !enabled;
			if (Input.GetKey (KeyCode.LeftShift)) {
				randomize = !randomize;
				enabled = false;
			}
		}
		void OnMouseEnter(){
			if (Input.GetMouseButton (0)) {
				enabled = !enabled;
				if (Input.GetKey (KeyCode.LeftShift)) {
					randomize = !randomize;
					enabled = false;
				}
			}
		}
	}
	public class ControlButton : MonoBehaviour
	{
		protected string buttonName = "button";
		protected Sequencer sequencer;
		protected Color baseColor = Color.grey;
		protected GameObject button;
		protected static int x, y;
		Renderer renderer;
		bool _active = false;
		protected bool active {
			get { return _active;}
			set{ 
				renderer.material.color = (value ? Color.red : baseColor);
				_active = value; 
			}
		}

		public static void ResetStaticGrid(){x = 0;y = 0;}

		public ControlButton Init(Sequencer sequencer)
		{
			this.sequencer = sequencer;
			gameObject.name = buttonName;
			gameObject.transform.parent = sequencer.transform;
			gameObject.transform.localPosition = new Vector3 ( x+(x*sequencer.space), 2.2f + y +(y*sequencer.space), 0 );
			renderer = gameObject.GetComponent<Renderer> ();
			renderer.material.color = baseColor;

			x++;
			if(x >= sequencer.width){x=0;y++;}
			
			return this;
		}
		
		public virtual void OnActivate(){}
		public virtual void OnDeactivate(){}
		void OnMouseDown(){
			active = !active;
			if(active){OnActivate();}else{OnDeactivate();}
		}
	}
	public class PlayButton : ControlButton
	{
		void Awake(){buttonName = "play button";}
		public override void OnActivate(){sequencer.Play ();}
		public override void OnDeactivate(){sequencer.Stop ();}
	}
	public class StopButton : ControlButton
	{
		void Awake(){buttonName = "stop button";}
		public override void OnActivate(){
			sequencer.Stop ();
			sequencer.ResetPlayhead();
			active = false;}
	}
}