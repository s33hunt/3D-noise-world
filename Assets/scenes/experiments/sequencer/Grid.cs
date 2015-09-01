using UnityEngine;
using System.Collections;

namespace Sequencer
{
	public class Grid : MonoBehaviour 
	{
		[HideInInspector] public Sequencer sequencer;
		[HideInInspector] public GridButton[,] gridButtons;
		[HideInInspector] public int 
			width = 8,
			height = 10;
		private int 
			tickCount = 0, 
			currentTick = 0;

		
		protected virtual void Awake()
		{
			sequencer = transform.GetComponent<Sequencer> ();
			width = sequencer.width;
			height = sequencer.height;
		}

		void Start()
		{
			BuildGrid ();
		}

		void BuildGrid()
		{
			gridButtons = new GridButton[width, height];

			Transform gridParent = new GameObject ("grid").transform;
			gridParent.parent = transform;
			gridParent.localPosition = Vector3.zero;

			for(int w=0; w<width; w++){ //per row
				for(int h=0; h<height; h++){ //per column in row
					var go = GameObject.CreatePrimitive (PrimitiveType.Cube);
					go.name = "("+w+","+(height-1-h)+")";
					go.transform.parent = gridParent;
					go.transform.localScale = new Vector3(sequencer.units.buttonWidth,sequencer.units.buttonWidth, 0.005f);
					go.transform.localPosition = new Vector3(
						(w * sequencer.units.buttonWidth) + (sequencer.units.margin * w), 
						- (h * sequencer.units.buttonWidth) - (sequencer.units.margin * h), 
						0
					);
					gridButtons[w,height-1-h] = (go.AddComponent<GridButton>()).Init(w,h);
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
				this.grid = transform.parent.parent.GetComponent<Grid> ();
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