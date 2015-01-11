using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveMasterArduino : MonoBehaviour
{
		private UtilDraw2D m_draw2D;
		private Connect2Arduino m_C2A;

		private int max_width;
		private int max_height;
		private int interval_width = 3;
		private LinkedList<float> wave_point_x;

		private Wave[] m_wave;
		public int wave_num = 2;
		
		private float before_data;

		void Start ()
		{
				m_C2A = GameObject.FindGameObjectWithTag ("Arduino").GetComponent<Connect2Arduino> ();
				m_draw2D = new UtilDraw2D ();
				wave_point_x = new LinkedList<float> ();
				m_wave = new Wave[wave_num];

				max_width = (int)Screen.width / interval_width;
				for (int i = 0; i < max_width; i++) {
						wave_point_x.AddLast (i * interval_width);
				}

				max_height = (int)Screen.height;
				int interval_height = max_height / wave_num;

				for (int i = 0; i < wave_num; i++) {
						m_wave [i] = new Wave (interval_height, i, max_width);
				}
		}

		void Update ()
		{
				for (int i = 0; i < wave_num; i += 2) {
						m_wave [i].addData (m_C2A.inputData [i] / 1023f);
						float tmp = (m_C2A.inputData [i] * 0.1f + before_data * 0.9f);
						m_wave [i + 1].addData (tmp / 1023f);
						before_data = tmp;
				}	
		}

		private Vector3[] posA;

		void OnPostRender ()
		{
				// 描画開始.
				m_draw2D.Begin ();
				{
						Vector3[] Line = new Vector3[2];
						Line [0] = new Vector3 (max_width * interval_width, max_height / 2, 0.0f);
						Line [1] = new Vector3 (0, max_height / 2, 0.0f);
		
						m_draw2D.DrawLines (Line, new Color (0.0f, 0.0f, 0.0f));
		
						for (int i = 0; i < wave_num; i++) {
								posA = new Vector3[m_wave [i].wave_point.Count];

								for (int j = 0; j < m_wave [i].wave_point.Count; j += 1) {
										posA [j] = new Vector3 (wave_point_x.ElementAt (j), m_wave [i].wave_point.ElementAt (j), 0.0f);
								}
								if (posA.Count () >= 2)
										m_draw2D.DrawLines (posA, new Color (0.0f, 0.0f, 0.0f));
						}
				}
				// 描画終了.
				m_draw2D.End ();
		}

}
