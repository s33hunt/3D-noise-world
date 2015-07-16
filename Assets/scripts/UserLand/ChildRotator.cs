using UnityEngine;
using System.Collections;

public class ChildRotator : MonoBehaviour 
{
	void Start () 
	{
		foreach (Transform child in transform) {
			child.localRotation = Random.rotation;
		}
	}
}