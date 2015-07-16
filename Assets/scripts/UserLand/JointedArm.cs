using UnityEngine;
using System.Collections;

public class JointedArm : MonoBehaviour {

	public int 
		joints = 5,
		maxAngle = 90;

	GameObject[] jointObjects;
	float lastCCVal = -1;


	void Start () 
	{
		jointObjects = new GameObject[joints];

		for(int i=0; i<joints; i++)
		{
			jointObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			jointObjects[i].name = "joint_"+i;
			jointObjects[i].transform.parent = i == 0 
				? transform
				: jointObjects[i-1].transform;
			//jointObjects[i].transform.localScale = new Vector3(0.3f, 0.2f, 1);
			jointObjects[i].transform.localPosition = new Vector3(0,1,0);
		}
	}
	
	void Update () 
	{
		if (lastCCVal != CCInputManager.instance.k2Knobs [7]) 
		{
			foreach (GameObject j in jointObjects) 
			{
				j.transform.localRotation = Quaternion.Euler (CCInputManager.instance.k2Knobs [7] * maxAngle, 0, 0);
			}
			lastCCVal = CCInputManager.instance.k2Knobs[7];
		}
	}
}
