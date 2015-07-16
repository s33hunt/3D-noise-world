using UnityEngine;
using System.Collections;

public class SequencerButton : MonoBehaviour 
{
	public enum ButtonType {Play, Empty}
	public ButtonType[] buttons;
	protected Sequencer sequencer;


	void Start()
	{
		sequencer = GetComponent<Sequencer> ();
		foreach (ButtonType type in buttons) {
			if(type == ButtonType.Play){
				(GameObject.CreatePrimitive (PrimitiveType.Cube).AddComponent<PlayButton>()).Init(sequencer);
			}
		}
	}

	public class ControlButton : MonoBehaviour
	{
		protected Sequencer sequencer;
		protected Color baseColor = Color.grey;
		protected GameObject button;
		protected static int x, y;
		bool _active = false;
		protected bool active {
			get { return _active;}
			set{ 
				gameObject.GetComponent<Renderer> ().material.color = (value ? Color.red : baseColor);
				_active = value; 
			}
		}

		public ControlButton Init(Sequencer sequencer)
		{
			this.sequencer = sequencer;
			gameObject.name = "play button";
			gameObject.transform.parent = sequencer.transform;
			gameObject.transform.localPosition = new Vector3 ( x+(x*sequencer.space), 2.2f + y +(y*sequencer.space), 0 );

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
		public override void OnActivate(){sequencer.Play ();}
		public override void OnDeactivate(){sequencer.Stop ();}
	}
}