using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XYRainer : MonoBehaviour 
{
	public MidiChannel channel;
	[Range(1,10)] public float 
		spacing = 1,
		gridWidth = 10, 
		gridHeight = 10;
	[Range(0.1f,3)] public float 
		raiseTime = 1,
		upSpeed = 1f;
	float 
		lastx=-1, 
		lasty=-1;
	Dictionary<string, Transform> activeCubes = new Dictionary<string, Transform>();


	void Start()
	{
		for(int x = 0; x<gridWidth; x++){
			for(int y = 0; y<gridHeight; y++){
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.parent = transform;
				cube.name = x+":"+y;
				cube.transform.localPosition = new Vector3(x*spacing, 0, y*spacing); //y translated to z space
			}
		}
	}

	void Update () 
	{
		float 
			x = CCInputManager.instance.xyPad.x,
			y = CCInputManager.instance.xyPad.y;

		if (x != lastx || y != lasty) 
		{
			int 
				gx = Mathf.RoundToInt(x*gridWidth),
				gy = Mathf.RoundToInt(y*gridHeight);
			string cname = gx+":"+gy;

			if(!activeCubes.ContainsKey(cname))
				StartCoroutine(Note (cname,gx,gy));
				Transform cube = transform.Find(cname);
				activeCubes.Add (cname, cube);

			lastx = x;
			lasty = y;
		}
		Debug.Log (x + ":" + y + ":" + lastx + ":" + lasty);

		foreach (Transform t in activeCubes.Values) {
			t.Translate(0,upSpeed * Time.deltaTime,0);
		}
	}

	IEnumerator Note(string cname, int x, int y)
	{

		while (true) {
			MidiOut.SendNoteOn(channel, x+20, 1);
			MidiOut.SendNoteOn(channel, y+20, 1);

			yield return new WaitForSeconds(raiseTime);

			MidiOut.SendNoteOff(channel, x+20);
			MidiOut.SendNoteOff(channel, y+20);

			Transform cube = transform.Find(cname);
			cube.localPosition = new Vector3(cube.localPosition.x, 0,cube.localPosition.z);

			activeCubes.Remove (cname);
		}
	}
}