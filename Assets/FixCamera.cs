using UnityEngine;
using System.Collections;

public class FixCamera : SingletonMonoBehaviour<FixCamera>
{

		//MeasureDistance ----------------------
		public static int _fixHeight = 720;
		public static int _fixWidth = 1280;
		private bool portrait = true;
		public Camera[] fixedCamera;
		//----------------------------
	
		private Vector2 _offset;
		public static float _scale = -1.0f;

		void Awake ()
		{
				if (this != Instance) {
						Destroy (this);
						return;
				}
		
				DontDestroyOnLoad (this.gameObject);
		}
		
		public void Fix ()
		{
				Camera cam = Camera.main;

				int fw = portrait ? _fixWidth : _fixHeight;
				int fh = portrait ? _fixHeight : _fixWidth;
	
				Rect set_rect = this.calc_aspect (fw, fh, out _scale);
				cam.rect = set_rect;

				// MEMO:NGUIのmanualHeight設定は不要、
				// UI Root下のカメラのアスペクト比固定すればよい
				// UI RootのAutomaticはOFF, Manual Heightは想定heightを設定する
		}
 
 
		// アスペクト比 固定するようにcameraのrect取得
		Rect calc_aspect (float width, float height, out float _scale)
		{
				float target_aspect = width / height;
				float window_aspect = (float)Screen.width / (float)Screen.height;
				float scale = window_aspect / target_aspect;
 
				Rect rect = new Rect (0.0f, 0.0f, 1.0f, 1.0f);
				if (1.0f > scale) {
						rect.x = 0;
						rect.width = 1.0f;
						rect.y = (1.0f - scale) / 2.0f;
						rect.height = scale;
						_scale = (float)Screen.width / width;
				} else {
						scale = 1.0f / scale;
						rect.x = (1.0f - scale) / 2.0f;
						rect.width = scale;
						rect.y = 0.0f;
						rect.height = 1.0f;
						_scale = (float)Screen.height / height;
				}

				_offset.x = rect.x * Screen.width;
				_offset.y = rect.y * Screen.height;
	
				return rect;
		}

		public Rect GetScope (float x, float y, float width, float height)
		{ 
				Rect rect = new Rect 
				(
				x * _scale,
				y * _scale,
				width * _scale, 
				height * _scale
				);
		
				return rect;
		
		}
	
		public Rect GetScope (Rect rect)
		{
				return GetScope (rect.x, rect.y, rect.width, rect.height);
		}

		public Rect GetRect (float x, float y, float width, float height)
		{ 
				Rect rect = new Rect 
				(
				_offset.x + (x * _scale),
				_offset.y + (y * _scale), 
				width * _scale, 
				height * _scale
				);
		
				return rect;
		
		}
	
		public Rect GetRect (Rect rect)
		{
				return GetRect (rect.x, rect.y, rect.width, rect.height);
		}
}
