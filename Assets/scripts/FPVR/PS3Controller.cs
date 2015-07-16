using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FPVR
{
	public sealed class PS3Controller 
	{
		public static class Axes //this is just to avoid typing the strings out over and over
		{
			public static string 
				right_stick_horizontal = "right_stick_horizontal",
				right_stick_vertical = "right_stick_vertical",
				left_stick_horizontal = "left_stick_horizontal",
				left_stick_vertical = "left_stick_vertical",
				button_square = "button_square",
				button_triangle = "button_triangle",
				button_cross = "button_cross",
				button_circle = "button_circle",
				l1 = "l1",
				l2 = "l2",
				r1 = "r1",
				r2 = "r2",
				start = "start",
				select = "select",
				right_stick_click = "right_stick_click",
				left_stick_click = "left_stick_click",
				dpad_horizontal = "dpad_horizontal",
				dpad_vertical = "dpad_vertical";
		}

		private static Dictionary<string, bool> axisStates = new Dictionary<string, bool>()
		{
			{"right_stick_horizontal", false},
			{"right_stick_vertical", false},
			{"left_stick_horizontal", false},
			{"left_stick_vertical", false},
			{"button_square", false},
			{"button_triangle", false},
			{"button_cross", false},
			{"button_circle", false},
			{"l1", false},
			{"l2", false},
			{"r1", false},
			{"r2", false},
			{"start", false},
			{"select", false},
			{"right_stick_click", false},
			{"left_stick_click", false},
			{"dpad_horizontal", false},
			{"dpad_vertical", false}
		};
		
		/// <summary>
		/// this method will return true in the frame after the given button was pressed. much like Input.GetButtonDown
		/// </summary>
		/// <param name="axisName">Axis name.</param>
		public static bool GetButtonDown(string axisName)
		{
			if(axisStates.ContainsKey(axisName)){
				float axisValue = Input.GetAxis (axisName);



				if (axisValue == 1){
					if(!axisStates[axisName]){
						if(axisName == Axes.button_cross){Debug.Log ("cross down");}
						axisStates [axisName] = true;
						return true;
					}
				}else{
					axisStates[axisName] = false;
				}
			}
			return false;
		}

		public static float GetAxis(string axisName)
		{
			if (axisStates.ContainsKey (axisName)) {
				return Input.GetAxis (axisName);
			}
			Debug.LogWarning ("axis "+axisName+" not found");
			return 0;
		}
	}
}