using UnityEngine;
using System.Collections;

namespace FPVR
{
	public class InteractiveObject : MonoBehaviour 
	{
		public string hoverMessage = "";


		void Awake(){gameObject.layer = GAME.Layers.Interactive;}

		public virtual void CrossButton(){Debug.Log (gameObject.name + ": cross button pressed");}
		public virtual void CircleButton(){Debug.Log (gameObject.name + ": circle button pressed");}
		public virtual void SquareButton(){Debug.Log (gameObject.name + ": square button pressed");}
		public virtual void TriangleButton(){Debug.Log (gameObject.name + ": triangle button pressed");}
	}
}