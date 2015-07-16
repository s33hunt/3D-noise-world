using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sequencer : MonoBehaviour 
{
	public delegate void OnMeasure();
	public OnMeasure onMeasure;
	public delegate void OnBeat();
	public OnMeasure onBeat;

	public bool setup = false;
	public MidiChannel[] channelMap;
	public int[] keyMap;
	public int
		bpm = 120,
		width = 8, 
		height = 10;

	public Button[,] buttons;
	GameObject[] indicators;
	[HideInInspector] public float space = 0.1f;


	void Start()
	{
		buttons = new Button[width, height];
		indicators = new GameObject[width];

		for(int w=0; w<width; w++){ //per row
			indicators[w] = GameObject.CreatePrimitive (PrimitiveType.Cube);
			indicators[w].name = "indicator "+w;
			indicators[w].transform.parent = transform;
			indicators[w].transform.localPosition = new Vector3(w + (space*w), 1 + space, 0);
			indicators[w].GetComponent<Renderer>().material.color = Color.black;
			Destroy(indicators[w].GetComponent<Collider>());


			for(int h=0; h<height; h++){ //per column in row
				var go = GameObject.CreatePrimitive (PrimitiveType.Cube);
				go.name = w+":"+h;
				go.transform.parent = transform;
				go.transform.localPosition = new Vector3(w + (space*w), -h - (space*h), 0);
				buttons[w,h] = (go.AddComponent<Button>()).Init(w,h);
			}
		}

		StartCoroutine ("Play");
	}

	void RandomizeEnabled()
	{
		foreach(Button b in buttons){
			if(b.randomize){b.RandomEnabled();}
		}
	}

	IEnumerator Play()
	{
		List<int> playedNotes = new List<int>();

		while (true) {//per measure
			RandomizeEnabled();
			if(onMeasure != null){onMeasure();}

			for(int w=0; w<width; w++){//per beat
				if(onBeat != null){ onBeat();}

				var r = indicators[w].GetComponent<Renderer>();
				r.material.color = Color.magenta;

				for(int h=0; h<height; h++){//per selected
					if(buttons[w,h].enabled){
						playedNotes.Add(h);
						MidiChannel ch = (setup ? channelMap[h] : channelMap[0]);
						MidiOut.SendNoteOn(ch, keyMap[h], 1f);
					}
				}

				yield return new WaitForSeconds (60f / (float)bpm);

				while(playedNotes.Count > 0){//after selected
					MidiOut.SendNoteOff((setup ? channelMap[playedNotes[0]] : channelMap[0]), keyMap[playedNotes[0]]);
					playedNotes.Remove(playedNotes[0]);
				}

				r.material.color = Color.black;
			}
		}
	}


	public class Button : MonoBehaviour 
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


		public Button Init(int x, int y)
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
}