using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Wave
{

		public LinkedList<float> wave_point;

		private int base_width = 0;
		private int base_height = 0;
		private float base_value;
		private float now_value;

		public Wave (int interval, int base_num, int width)
		{
				base_width = width;
				base_height = base_num * interval;
				base_value = interval / 2.0f;
				wave_point = new LinkedList<float> ();
		}

		public void addData (float data)
		{
				now_value = base_value * data + (base_height + base_value);
	
				if (base_width <= wave_point.Count) {
						wave_point.RemoveFirst ();
						wave_point.AddLast (FixCoordinates.Instance.GetPointY (now_value));
				} else {
						wave_point.AddLast (FixCoordinates.Instance.GetPointY (now_value));
				}
		}
		
}
