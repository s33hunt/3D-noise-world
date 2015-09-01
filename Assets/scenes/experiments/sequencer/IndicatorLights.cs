using UnityEngine;
using System.Collections;

namespace Sequencer
{
	public class IndicatorLights : MonoBehaviour 
	{
		Renderer[] lights;
		Renderer lastLit;
		Sequencer sequencer;


		void Start()
		{
			sequencer = GetComponent<Sequencer> ();
			lights = new Renderer[sequencer.width];
			BuildGrid ();
		}

		void BuildGrid()
		{
			Transform indiParent = new GameObject ("indicator lights").transform;
			indiParent.parent = transform;
			indiParent.localPosition = Vector3.zero;
			
			for(int w=0; w<sequencer.width; w++){ //per row
				var indi = GameObject.CreatePrimitive (PrimitiveType.Cube);
				indi.name = "indicator "+w;
				indi.transform.parent = indiParent;
				indi.transform.localScale = new Vector3(sequencer.units.buttonWidth,sequencer.units.buttonWidth, 0.005f);
				indi.transform.localPosition = new Vector3((w * sequencer.units.buttonWidth) + (sequencer.units.margin * w), 0,0);
				lights[w] = indi.GetComponent<Renderer>();
				lights[w].material.color = sequencer.colors.indicatorColor;
				Destroy(indi.GetComponent<Collider>());
			}
		}

		public void OnTick(int currentTick)
		{
			if(lastLit != null){lastLit.material.color = sequencer.colors.indicatorColor;}
			lights [currentTick].material.color = sequencer.colors.indicatorActiveColor;
			lastLit = lights [currentTick];
		}

		public void AllOff()
		{
			foreach(Renderer r in lights){
				r.material.color = sequencer.colors.indicatorColor;
			}
		}
	}
}