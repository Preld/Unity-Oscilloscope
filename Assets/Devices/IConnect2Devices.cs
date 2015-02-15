using UnityEngine;
using System.Collections;

public interface IConnect2Devices
{
		float[] GetVoltages ();

		bool SetPort (string portName, int baudRate, int inputNum);
}
