using UnityEngine;
using System.Collections;

public class FixCoordinates : SingletonMonoBehaviour<FixCoordinates>
{

		void Awake ()
		{
				if (this != Instance) {
						Destroy (this);
						return;
				}
		
				DontDestroyOnLoad (this.gameObject);
		}

		// Standard
		public Rect GetRect (float x, float y, float width, float height)
		{
				return FixCamera.Instance.GetRect (x, y, width, height);
		}

		public Rect GetRect (Rect rect)
		{
				return GetRect (rect.x, rect.y, rect.width, rect.height);
		}

		// Move Center
		public Rect GetCenterRect (float x, float y, float width, float height)
		{
				x = x - width / 2.0f;
				y = y - height / 2.0f;
				return FixCamera.Instance.GetRect (x, y, width, height);
		}

		public Rect GetCenterRect (Rect rect)
		{
				return GetCenterRect (rect.x, rect.y, rect.width, rect.height);
		}

		// Scope
		public Rect GetScope (float x, float y, float width, float height)
		{
				return FixCamera.Instance.GetScope (x, y, width, height);
		}

		public Rect GetScope (Rect rect)
		{
				return GetScope (rect.x, rect.y, rect.width, rect.height);
		}

		// Unity height -6.4 ~ +6.4 width -3.6 ~ +3.6
		private float standard_height = 6.4f;
		private float standard_width = 3.6f;
		public Rect GetUnityRect (float x, float y, float width, float height)
		{
				
				width = width * 100;
				height = height * 100;
				y = (standard_height + y) * 100;
				x = (standard_width + x) * 100;
				x = x - width / 2.0f;
				y = y - height / 2.0f;
				return FixCamera.Instance.GetRect (x, y, width, height);
		}

		public Rect GetUnityRect (Rect rect)
		{
				return GetUnityRect (rect.x, rect.y, rect.width, rect.height);
		}

		// Size
		public Vector2 GetSize (float width, float height)
		{ 
				Rect scope = FixCamera.Instance.GetRect (0, 0, width, height);
				return new Vector2 (scope.width, scope.height);
		}
	
		public Vector2 GetSize (Vector2 vec2)
		{
				return GetSize (vec2.x, vec2.y);
		}

		// Point
		public Vector2 GetPoint (float x, float y)
		{ 
				Rect point = FixCamera.Instance.GetRect (x, y, 0, 0);
				return new Vector2 (point.x, point.y);
		}
		
		public float GetPointX (float x)
		{
				Rect point = FixCamera.Instance.GetRect (x, 0, 0, 0);
				return point.x;
		}

		public float GetPointY (float y)
		{
				Rect point = FixCamera.Instance.GetRect (0, y, 0, 0);
				return point.y;
		}
}
