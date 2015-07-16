using UnityEngine;
using System.Collections;

public class SequenceRandomizer : MonoBehaviour 
{
	public int x, y, width, height;
	Sequencer sequencer;

	void Start()
	{
		sequencer = GetComponent<Sequencer> ();
		sequencer.onMeasure += OnMeasureHandler;
		OnMeasureHandler ();
	}

	public SequenceRandomizer Init(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		return this;
	}

	void OnMeasureHandler()
	{
		for(int w=x; w<width+x; w++){
			for(int h=y; h<height+y; h++){
				sequencer.buttons[w,h].RandomEnabled();
          	}
		}
	}
}