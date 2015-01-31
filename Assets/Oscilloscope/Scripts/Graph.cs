using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph : MonoBehaviour
{

		private const int RESOLUTION_WIDTH = 240;
	
		private LineRenderer _lineRenderer;
		private float _backgroundHeight;
		private float _backgroundWidth;
		private Vector3 _centerLineBegin;
		private Vector3 _centerLineEnd;
		private Bounds _myBounds;

		private Vector3 _baseLinePos;
		private Vector3 _baseX;
		private Vector3 _baseY;
		private List<float> _yPoints;

		void Start ()
		{
				// init line
				_lineRenderer = gameObject.AddComponent (typeof(LineRenderer)) as LineRenderer;
				_lineRenderer.material.color = new Color (0, 0, 0, 1.0f);
				_lineRenderer.SetWidth (0.05f, 0.05f);
				_lineRenderer.SetVertexCount (RESOLUTION_WIDTH);	

				// init positions	
				_myBounds = GetBound ();
				Vector3 objectSize = new Vector3 (
						                     _myBounds.extents.x * this.transform.lossyScale.x,
						                     _myBounds.extents.y * this.transform.lossyScale.y,
						                     _myBounds.extents.z * this.transform.lossyScale.z
				                     );
				_baseLinePos = new Vector3 (
						transform.position.x - objectSize.x,
						transform.position.y - objectSize.z,
						transform.position.z - 0.1f //カメラの方へ少しずらす
				);
				/*_centerLineBegin = new Vector3 (
						transform.position.x - objectSize.x,
						transform.position.y,
						transform.position.z - 0.1f //カメラの方へ少しずらす
				);
				_centerLineEnd = new Vector3 (
						transform.position.x + objectSize.x,
						transform.position.y,
						transform.position.z - 0.1f //カメラの方へ少しずらす
				);*/

				// init size
				_backgroundHeight = objectSize.z * 2f; // 回転しているため
				_backgroundWidth = objectSize.x * 2f;
				_baseX = new Vector3 (
						_backgroundWidth / (RESOLUTION_WIDTH - 1),
						0,
						0
				);
				_baseY = new Vector3 (
						0,
						_backgroundHeight,
						0
				);
						
				// dummy data
				_yPoints = new List<float> ();
				for (int i = 0; i < RESOLUTION_WIDTH; i++) {
						_yPoints.Add (0f);
				}
		}

		void Update ()
		{
				for (int i = 0; i < RESOLUTION_WIDTH; i++) {
						_lineRenderer.SetPosition (i, _baseLinePos + _baseX * i + _baseY * _yPoints [i]);
				}
		}

		public void setValue (float value)
		{
				_yPoints.Add (value);
				_yPoints.RemoveAt (0);
		}

		private Bounds GetBound ()
		{
				Bounds bounds = this.GetComponent<MeshFilter> ().mesh.bounds;
				//bounds.center = Vector3.zero;
				return bounds;
		}
}
