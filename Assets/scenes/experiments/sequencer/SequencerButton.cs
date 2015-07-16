using UnityEngine;
using System.Collections;

public class SequencerButton : MonoBehaviour 
{
	public int x, y;
	bool _active = false;
	protected Sequencer sequencer;
	protected bool active {
		get { return _active;}
		set{ 
			gameObject.GetComponent<Renderer> ().material.color = (value ? Color.green : Color.grey);
			_active = value; 
		}
	}
	

	void Start(){
		sequencer = GetComponent<Sequencer> ();
		transform.parent = sequencer.transform;
		transform.localPosition = new Vector3 ( x+(x*sequencer.space), y+(y*sequencer.space), 0 );
	}
}