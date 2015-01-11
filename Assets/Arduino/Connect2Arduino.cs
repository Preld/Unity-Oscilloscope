using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class Connect2Arduino : MonoBehaviour
{
		private static SerialPort sp = new SerialPort ("/dev/tty.usbmodem14121", 115200);
		//private static SerialPort sp = new SerialPort ("/dev/tty.usbmodem1451", 115200);
		//private static SerialPort sp = new SerialPort ("COM3", 115200);
		public float[] inputData;
		private const int pointNum = 1;
		
		// Use this for initialization
		void Start ()
		{
				inputData = new float[pointNum];
				OpenConnection ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				RecieveInput ();
		}

		public int lost_count = 0;
		public bool useData = true;
		
		private int receive_val = 0;

		private int high_val = 0;
		private int low_val = 0;
		private int head_high = 0;
		private int head_low = 0;

		bool HighValueJudge (int value)
		{
				int head = value >> 5;
				if (head < 4)
						return false;
				return true;
		}

		void RecieveInput ()
		{
				try {
						/*int read_bytes_length = sp.BytesToRead;
			//Debug.Log ("READ_BYTES:" + read_bytes_length);
			while (read_bytes_length > 1) {
				read_bytes_length -= 2;
				high_val = sp.ReadByte ();
				if (!HighValueJudge (high_val)) {
					lost_count++;
					read_bytes_length--;
					high_val = sp.ReadByte ();
				}
				low_val = sp.ReadByte ();
									
				head_high = ((high_val >> 5) & ((1 << 2) - 1)) << 2;
				head_low = low_val >> 5;
				
				receive_val = ((high_val & ((1 << 5) - 1)) << 5) + (low_val & ((1 << 5) - 1));

				//Debug.Log ("RECEIVE:" + receive_val);
				//Debug.Log ("HEAD:" + (head_high + head_low));
				inputData [head_high + head_low] = receive_val;// / 1023f;
			}*/
						//for Windows

			
						do {
								high_val = sp.ReadByte ();
						} while(!HighValueJudge (high_val));
						low_val = sp.ReadByte ();
			
						head_high = ((high_val >> 5) & ((1 << 2) - 1)) << 2;
						head_low = low_val >> 5;
			
						receive_val = ((high_val & ((1 << 5) - 1)) << 5) + (low_val & ((1 << 5) - 1));

						inputData [head_high + head_low] = receive_val;// / 1023f;

		
						useData = true;
				} catch (Exception errorpiece) {
						/*if (useData) {
								Debug.Log ("Error 1: " + errorpiece);
								useData = false;
						}*/
				}

		}

		void OnApplicationQuit ()
		{
				sp.Close ();
		}

		void OpenConnection ()
		{
				if (sp != null) {
						if (sp.IsOpen) {
								sp.Close ();
								Debug.LogError ("Failed to open Serial Port, already open!");
						} else {
								sp.Open ();
								sp.ReadTimeout = 50;
								Debug.Log ("Open Serial port");
						}
				}
		}

}