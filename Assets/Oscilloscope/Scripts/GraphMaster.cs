using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GraphMaster : MonoBehaviour
{
		private ConnectInterface _devices;
		private Graph[] _graphs;

		private float beforeValue;

		void Start ()
		{
				_devices = GameObject.FindGameObjectWithTag ("Arduino").GetComponent<Connect2Arduino> ();
				_devices.setPort ("COM3", 115200, 1);
				//("/dev/tty.usbmodem14121", 115200);
				//("/dev/tty.usbmodem1451", 115200);
				//("COM3", 115200);
				GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Graph");
				_graphs = new Graph[gameObjects.Length];
				for (int i = 0; i < _graphs.Length; i++) {
						_graphs [i] = gameObjects [i].GetComponent<Graph> ();
				}
		}

		void Update ()
		{
				//float[] data = _devices.getVoltages ();
				float[] data = new float[2];
				data [0] = Mathf.Abs (Mathf.Sin (Time.time * 3f));
				data [1] = Mathf.Abs (Mathf.Sin (Time.time * 5f));

				// processing data
				//_graphs [0].setValue (data [0] / 1023f);
				//float tmp = (data [0] * 0.1f + before_data * 0.9f);
				//m_wave [1].addData (tmp / 1023f);
				//before_data = tmp;

				// set value
				_graphs [0].setValue (data [0]);
				_graphs [1].setValue (data [1]);
		}

		public void fitButton ()
		{
		}
				
}
