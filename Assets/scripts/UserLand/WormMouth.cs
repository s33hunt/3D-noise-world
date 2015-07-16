using UnityEngine;
using System.Collections;

public class WormMouth : CCOutput 
{

	public float 
		range = 5,
		xValue = 0,
		yValue = 0;
	Transform tpp, tpn, tnn, tnp, quads, pointer;
	float lastx=0, lasty=0;

	void Start () 
	{
		pointer = transform.Find ("pointer");
		quads = transform.Find ("quadrants");
		tpp = quads.Find ("tpp");
		tpn = quads.Find ("tpn");
		tnn = quads.Find ("tnn");
		tnp = quads.Find ("tnp");
	}

	void Update () 
	{
		quads.localRotation = Quaternion.Euler (0, 0, CCInputManager.instance.xyz * 360);
		//graph quadrants. p = positive, n = negative. name format [x sign][y sign]
		Vector3 
			pp = new Vector3 (range * CCInputManager.instance.m1, range * CCInputManager.instance.m2, 0),
			pn = new Vector3 (range * CCInputManager.instance.m1, -range * CCInputManager.instance.m2, 0),
			nn = new Vector3 (-range * CCInputManager.instance.m1, -range * CCInputManager.instance.m2, 0),
			np = new Vector3 (-range * CCInputManager.instance.m1, range * CCInputManager.instance.m2, 0);

		tpp.localPosition = pp;
		tpn.localPosition = pn;
		tnn.localPosition = nn;
		tnp.localPosition = np;

		pointer.position = tpp.position; //get a transform in the same local space as the measuring point

		//get float val of tpp x dist from center of wormmouth
		xValue = pointer.localPosition.x / range;
		yValue = pointer.localPosition.y / range;

		if(lastx != xValue){SendSignal(ccSignalNumbers[0], channels[0], Mathf.Abs(xValue)); lastx = xValue;}
		if(lasty != yValue){SendSignal(ccSignalNumbers[1], channels[0], Mathf.Abs(yValue)); lasty = yValue;}
	}
}