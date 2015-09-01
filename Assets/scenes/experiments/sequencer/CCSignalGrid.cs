using UnityEngine;
using System.Collections;

namespace Sequencer
{
	public class CCSignalGrid : Grid {
		public int controlNumber = 1;

		protected override void Awake()
		{
			base.Awake ();
			height = 10;
		}
	}
}