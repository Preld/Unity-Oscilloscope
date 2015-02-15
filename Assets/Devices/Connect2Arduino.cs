using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class Connect2Arduino : MonoBehaviour,IConnect2Devices
{

		#region IConnect2Devices implementation

		public float[] GetVoltages ()
		{
				// Correct range of each values is between 0f and 1f. 
				return _inputVoltages;
		}

		public bool SetPort (string portName, int baudRate, int inputNum)
		{
				_serialPort = new SerialPort (portName, baudRate);
				_inputNum = inputNum;
				_inputVoltages = new float[inputNum];
				return OpenConnection ();
		}

		#endregion

		private SerialPort _serialPort;
		private int _inputNum;
		private float[] _inputVoltages;

		void Start ()
		{
		}

		void Update ()
		{
				RecieveInput ();
		}

		private int _lostCount = 0;

		private bool HighValueJudge (int value)
		{
				int head = value >> 5;
				if (head < 4)
						return false;
				return true;
		}

		void RecieveInput ()
		{
				int _voltageValueHigh = 0;
				int _voltageValueLow = 0;
				int _portIndexHigh = 0;
				int _portIndexLow = 0;

				if (!_serialPort.IsOpen)
						return;

				try {
						#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
						int _readBytesLength = _serialPort.BytesToRead;

						while (_readBytesLength > 1) {
								_readBytesLength -= 2;
								while (!HighValueJudge (_voltageValueHigh = _serialPort.ReadByte ())) {
										_lostCount++;
										_readBytesLength--;
								}
								while (HighValueJudge (_voltageValueLow = _serialPort.ReadByte ())) {
										_lostCount++;
										_readBytesLength--;
								}
									
								_portIndexHigh = ((_voltageValueHigh >> 5) & ((1 << 2) - 1)) << 2;
								_portIndexLow = _voltageValueLow >> 5;
				
								int getPortIndex = _portIndexHigh + _portIndexLow;
								int receiveValue = ((_voltageValueHigh & ((1 << 5) - 1)) << 5) + (_voltageValueLow & ((1 << 5) - 1));
								_inputVoltages [getPortIndex] = receiveValue / 1023f;
								//Debug.Log ("RECEIVE:" + receive_val);
								//Debug.Log ("HEAD:" + (head_high + head_low));
						}
						#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
						do {
								high_val = sp.ReadByte ();
						} while(!HighValueJudge (high_val));
						low_val = sp.ReadByte ();
			
						head_high = ((high_val >> 5) & ((1 << 2) - 1)) << 2;
						head_low = low_val >> 5;
			
						receive_val = ((high_val & ((1 << 5) - 1)) << 5) + (low_val & ((1 << 5) - 1));

						inputVoltages [head_high + head_low] = receive_val;// / 1023f;
						#endif
				} catch (Exception errorpiece) {
						Debug.LogError ("Error : " + errorpiece);
				}

		}

		void OnApplicationQuit ()
		{
				_serialPort.Close ();
		}

		bool OpenConnection ()
		{
				if (_serialPort == null)
						return false;

				if (_serialPort.IsOpen) {
						Debug.LogError ("Failed to open Serial Port, already open!");
						return false;
				} else {
						try {
								_serialPort.Open ();
								_serialPort.ReadTimeout = 50;
								Debug.Log ("Open Serial port");
								return true;
						} catch {
								Debug.LogError ("Failed to open Serial Port");
								return false;
						}
				}
		}

}
