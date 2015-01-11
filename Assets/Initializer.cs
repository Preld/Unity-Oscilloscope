using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour
{
		void Awake ()
		{
				FixCamera.Instance.Fix ();
		}
}
