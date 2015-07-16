using UnityEngine;
using System.Collections;

public class ProgressIndicator
{
	public float start, end, increment, total, progress;
	public int steps, step;
	public bool complete { get { return progress >= total; } }
	public float percent {get{return 100f * (progress / total);}}


	public ProgressIndicator(float from = 0, float to = 100, int steps = 100)
	{
		if (from >= to) {throw new UnityException("Start must be less than End");}
		this.steps = steps;
		start = from;
		end = to;
		total = end - start;
		increment = total / (float)steps;
	}

	public float Step()
	{
		if ( (step++) >= steps) {
			step = steps;
			return total;
		}
		progress = increment * step;
		return progress;
	}

	public float JumpToPercent(float percent)
	{
		float p = Mathf.Clamp (percent, 0, 100) * 0.01f;
		progress = p * total;
		step = Mathf.RoundToInt(steps * p);
		return progress;
	}

	public string GetProgressString(){return _GetProgressString (steps);}
	public string GetProgressString(int dots){return _GetProgressString (dots);}
	private string _GetProgressString(int dots)
	{
		string s = "[";
		for (int i=0; i < dots; i++) {
			s +=  (float)i/(float)dots < (float)progress/(float)total ? "." : " ";
		}
		return s + "]";
		
	}
}
