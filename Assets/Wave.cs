using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Wave
{

		public LinkedList<float> row_point;
		public LinkedList<float> wave_point;

		private int base_width = 0;
		private float base_height = 0;
		private float base_value;
		private float now_value;

		// 代入される値は常に0~1
		private float maxValue = 0.0f;
		private float minValue = 1.0f;
		private float highValue = 1.0f;
		private float lowValue = 0.0f;

		public Wave (float interval, int base_num, int width)
		{
				base_width = width;
				base_height = base_num * interval;
				base_value = interval;
				wave_point = new LinkedList<float> ();
		}

		public void addData (float data)
		{
				now_value = base_value * (fitRange (data) - 0.5f) + (base_height + base_value / 2f);
				maxValue = Mathf.Max (maxValue, data);
				minValue = Mathf.Min (minValue, data);
	
				if (base_width <= wave_point.Count) {
						wave_point.RemoveFirst ();
						wave_point.AddLast (now_value);
				} else {
						wave_point.AddLast (now_value);
				}
		}

		private float fitRange (float data)
		{
				return (data - lowValue) / (highValue - lowValue);
		}

		public void fitData ()
		{
				// 範囲を更新
				highValue = maxValue;
				lowValue = minValue;
				/*LinkedList<float> new_point = new LinkedList<float> ();
				float value = 0.0f;
				for (int i = 0; i < wave_point.Count; i++) {
						value = wave_point.ElementAt (i) - (base_height + base_value / 2f);
						value = value / base_value + 0.5f;
						value = base_value * (fitRange (value) - 0.5f) + (base_height + base_value / 2f);
						new_point.AddLast (value);	
				}
				wave_point = new_point;*/
				maxValue = 0.0f;
				minValue = 1.0f;
		}
		
}
