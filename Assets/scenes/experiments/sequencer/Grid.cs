using UnityEngine;
using System.Collections;

namespace Sequencer
{
	public class Grid : MonoBehaviour 
	{
		public Sequencer sequencer;
		public GridButton[,] gridButtons;
		public int 
			width = 8, 
			height = 10;
		private int 
			tickCount = 0, 
			currentTick = 0;

		
		void Start()
		{
			sequencer = transform.GetComponent<Sequencer> ();
			width = sequencer.width;
			height = sequencer.height;
			BuildGrid ();
		}

		void BuildGrid()
		{
			gridButtons = new GridButton[width, height];


			for(int w=0; w<width; w++){ //per row
				for(int h=0; h<height; h++){ //per column in row
					var go = GameObject.CreatePrimitive (PrimitiveType.Cube);
					go.name = "("+w+","+h+")";
					go.transform.parent = transform;
					go.transform.localScale = new Vector3(sequencer.units.buttonWidth,sequencer.units.buttonWidth, 0.005f);
					go.transform.localPosition = new Vector3(
						(w * sequencer.units.buttonWidth) + (sequencer.units.margin * w), 
						sequencer.units.gridHeight + (h * sequencer.units.buttonWidth) + (sequencer.units.margin * h), 
						0
					);
					gridButtons[w,h] = (go.AddComponent<GridButton>()).Init(w,h);
				}
			}
		}

		public class GridButton : MonoBehaviour 
		{
			public int x, y;
			Grid grid;
			Renderer renderer;
			Color baseColor;
			bool 
				_enabled = false,
				_randomize = false;
			
			public bool randomize {
				get { return _randomize; }
				set { 
					baseColor = value ? grid.sequencer.colors.gridRandomizedColor : grid.sequencer.colors.gridColor; 
					renderer.material.color = baseColor;
					_randomize = value; 
				}
			}
			public bool enabled {
				get { return _enabled;}
				set {
					renderer.material.color = (value ? grid.sequencer.colors.gridActiveColor : baseColor);
					_enabled = value;
				}
			}
			
			
			void Awake()
			{
				this.grid = transform.parent.GetComponent<Grid> ();
				renderer = GetComponent<Renderer> ();
				randomize = false; // to set baseColor
				renderer.material.color = baseColor;
			}
			
			public GridButton Init(int x, int y)
			{
				this.x = x;
				this.y = y;
				return this;
			}
			
			public void RandomizeEnabled()
			{
				enabled = Random.Range(0, 2) >= 1 ? true : false;
			}
			
			void OnMouseDown(){
				enabled = !enabled;
				if (Input.GetKey (KeyCode.LeftShift)) {
					randomize = !randomize;
					enabled = false;
					renderer.material.color = baseColor;
				}
				
			}
			void OnMouseEnter(){
				if (Input.GetMouseButton (0)) {
					OnMouseDown();
				}
			}
		}
	}
}