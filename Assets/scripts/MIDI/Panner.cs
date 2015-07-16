using UnityEngine;
using System.Collections;

public class Panner : CCOutput 
{
	public int keyNumber = 0;
	public bool left = false;
	Transform L,R,panner,lerpTarget;
	float 
		distance,
		lastScale = -1;


	void Start()
	{
		L = transform.Find ("L");
		R = transform.Find ("R");
		panner = transform.Find ("panner");
		SetPanTarget ();
		distance = Vector3.Distance (L.localPosition, R.localPosition);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)) {
			SetPanTarget();
			MidiOut.SendNoteOn(channels[0],keyNumber, 1f);
		}
		if (MidiInput.GetKeyDown(keyNumber)) {
			if(Application.platform != RuntimePlatform.Android){
				GetComponent<PannerObserver>().Sync();
			}
			SetPanTarget();
		}
		/*if (MidiInput.GetKeyUp (keyNumber)) {
			if(Application.platform != RuntimePlatform.Android){
				GetComponent<PannerObserver>().Sync();
			}
			SetPanTarget();
		}*/

		panner.position = Vector3.Lerp (panner.position, lerpTarget.position, 0.5f);
		float scale = Vector3.Distance (L.position, panner.position) / distance;
		panner.localScale = new Vector3 (scale * 5, 1,1);

		if (scale != lastScale) {
			SendSignal (scale);
			lastScale = scale;
		}
	}

	public void SetPanTarget()
	{
		lerpTarget = left ? L : R;
		left = !left;
	}

}