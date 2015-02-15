using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph : MonoBehaviour
{

		private const int RESOLUTION_WIDTH = 240;

		// Wave
		private LineRenderer _waveRenderer;

		// To decide draw point;
		private Vector3 _baseLinePos;
		private Vector3 _baseX;
		private Vector3 _baseY;
		private List<float> _yPoints;

		void Start ()
		{
				// Ready drawing line
				_waveRenderer = gameObject.AddComponent (typeof(LineRenderer)) as LineRenderer;
				_waveRenderer.material.color = new Color (0, 0, 0, 1.0f);
				_waveRenderer.SetWidth (0.05f, 0.05f);
				_waveRenderer.SetVertexCount (RESOLUTION_WIDTH);	

				// Cale drawing base point
				Vector3 objectSize = GetObjectSize (this.gameObject);
				_baseLinePos = new Vector3 (
						this.transform.position.x - objectSize.x / 2.0f,
						this.transform.position.y - objectSize.y / 2.0f,
						this.transform.position.z - 0.1f //カメラの方へ少しずらす
				);
				_baseX = new Vector3 (
						objectSize.x / (RESOLUTION_WIDTH - 1),
						0,
						0
				);
				_baseY = new Vector3 (
						0,
						objectSize.y,
						0
				);
						
				// init variable 
				_yPoints = new List<float> ();
				for (int i = 0; i < RESOLUTION_WIDTH; i++) {
						_yPoints.Add (0f);
				}
		}

		void Update ()
		{
				for (int i = 0; i < RESOLUTION_WIDTH; i++) {
						_waveRenderer.SetPosition (i, _baseLinePos + _baseX * i + _baseY * _yPoints [i]);
				}
		}

		public void SetValue (float value)
		{
				// Correct range of value is between 0f and 1f.
				_yPoints.Add (value);
				_yPoints.RemoveAt (0);
		}

		private Vector3 GetObjectSize (GameObject gameObject)
		{
				Vector3 originObjectSize = GetOriginalObjectSize (gameObject);
				return gameObject.transform.rotation * originObjectSize;
		}

		private Vector3 GetOriginalObjectSize (GameObject gameObject)
		{
				Bounds myBounds = GetBound (gameObject);
				Vector3 objectSize = new Vector3 (
						                     myBounds.size.x * gameObject.transform.lossyScale.x,
						                     myBounds.size.y * gameObject.transform.lossyScale.y,
						                     myBounds.size.z * gameObject.transform.lossyScale.z
				                     );
				return objectSize;
		}

		private Bounds GetBound (GameObject gameObject)
		{
				return gameObject.GetComponent<MeshFilter> ().mesh.bounds;
		}
}
