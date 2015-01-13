using UnityEngine;
using System.Collections;

public interface ConnectInterface
{
		float[] getVoltages ();

		void setPort (string portName, int baudRate, int inputNum);
}
