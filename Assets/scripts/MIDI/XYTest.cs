using UnityEngine;
using System.Collections;

public class XYTest : CCOutput 
{
	public enum XY {x,y}
	public XY xy;
	float lastx=-1, lasty=-1;


	void Update () 
	{
		float 
			x = CCInputManager.instance.xyPad.x,
			y = CCInputManager.instance.xyPad.y;

		if (xy == XY.x) {
			if (x != lastx) {SendSignal (x);}
		} else {
			if (y != lasty) {SendSignal (y);}
		}

		transform.position = new Vector3(
			x,
			y,
			0
		);
	}
}