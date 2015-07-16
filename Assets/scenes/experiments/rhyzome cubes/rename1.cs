using UnityEngine;
using System.Collections;

public class rename1 : MonoBehaviour 
{
	public static int desiredGenerations = 0;
	public int 
		parentGenerationSize = 0, 
		generationNumber = 0, 
		childCount = 0;


	void Update ()
	{
		float a = ((CCInputManager.instance.k2Sliders [1] * 5) + 1) * Time.deltaTime;

		transform.Rotate (a,a,a);

		if (desiredGenerations > generationNumber && transform.childCount == 0) {
			SpawnGeneration ();
		} else if (desiredGenerations <= generationNumber && transform.childCount > 0) {
			DeleteChildren ();
		}
	}

	void SpawnGeneration()
	{
		if (parentGenerationSize != 0) {
			if (parentGenerationSize <= 1) {
				//return;
			}
		}

		childCount = Random.Range (1, 4);

		for(int i=0; i<childCount; i++)
		{
			var child = GameObject.CreatePrimitive (PrimitiveType.Cube);
			
			child.transform.parent = transform;
			child.transform.localPosition = new Vector3(0,0,2);
			child.transform.localRotation = Quaternion.Euler(
				Random.Range(0,360),
				Random.Range(0,360),
				Random.Range(0,360)
			);
			child.transform.localScale = new Vector3(
				Random.Range(1f,1.2f),
				Random.Range(1f,1.2f),
				Random.Range(1f,1.2f)
			);
			
			var rn = child.AddComponent<rename1>();
			rn.parentGenerationSize = childCount;
			rn.generationNumber = generationNumber + 1;
		}
	}

	void DeleteChildren()
	{
		if(generationNumber > 0){Destroy(gameObject);}
	}
}
