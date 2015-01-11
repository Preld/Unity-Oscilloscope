using UnityEngine;
using System.Collections;

// Main Cameraに割り当てたスクリプトの「OnPostRender」もしくは「OnRenderObject」で呼び出す。
public class Graph : MonoBehaviour
{

		private UtilDraw2D m_draw2D;
		void Start ()
		{
				m_draw2D = new UtilDraw2D ();
		}
 
		void OnPostRender ()
		{
				// 描画開始.
				m_draw2D.Begin ();
				{
						m_draw2D.SetLineWidth (0.0f);  // ラインの太さを指定 (0.0なら太さ指定なし）.
						m_draw2D.SetLineDot (0.0f);    // ラインを破線にする場合の間隔指定（0.0なら破線なし）.
						Vector3 [] posA = new Vector3[5];
						posA [0] = new Vector3 (10.0f, 10.0f, 0.0f);
						posA [1] = new Vector3 (20.0f, 80.0f, 0.0f);
						posA [2] = new Vector3 (70.0f, 50.0f, 0.0f);
						posA [3] = new Vector3 (50.0f, 20.0f, 0.0f);
						posA [4] = new Vector3 (10.0f, 10.0f, 0.0f);

						// ポリゴン描画（時計周りに頂点を指定のこと）.
						m_draw2D.DrawPolyon (posA, new Color (0.0f, 0.0f, 0.3f));
   
						// ライン描画.
						m_draw2D.DrawLines (posA, Color.red);
           
						// 円の描画.
						{
								m_draw2D.SetLineWidth (2.0f);
								m_draw2D.SetLineDot (4.0f);
               
								Vector3 center = new Vector3 (100.0f, 150.0f, 0.0f);
								float r = 50.0f;
								m_draw2D.DrawCircleFill (center, r, new Color (0.0f, 0.4f, 0.0f));
								m_draw2D.DrawCircle (center, r, Color.red);
						}

						// 矩形の描画.
						{
								m_draw2D.SetLineWidth (1.0f);
								m_draw2D.SetLineDot (0.0f);
               
								Vector3 v0 = new Vector3 (Screen.width - 20, Screen.height - 20);
								Vector3 v1 = new Vector3 (Screen.width - 80.0f, Screen.height - 80.0f);              
								m_draw2D.DrawRectangleFill (v0, v1, new Color (0.0f, 0.0f, 0.4f));
								m_draw2D.DrawRectangle (v0, v1, Color.red);
						}
       
						// ベジェの描画.
						{
								m_draw2D.SetLineWidth (1.0f);
								m_draw2D.SetLineDot (0.0f);

								Vector3 [] posB = new Vector3[8];
           
								posB [0] = new Vector3 (30, 140);
								posB [1] = new Vector3 (60, 180);
								posB [2] = new Vector3 (90, 160);
								posB [3] = new Vector3 (125, 100);
								posB [4] = new Vector3 (125, 100);
								posB [5] = new Vector3 (180, 60);
								posB [6] = new Vector3 (210, 140);
								posB [7] = new Vector3 (240, 120);
								m_draw2D.DrawBezier (posB, new Color (0.2f, 0.7f, 1.0f));
               
								//Vector3 v0 = new Vector3 (0.0f, 0.0f);
								//Vector3 v1 = new Vector3 (0.0f, 0.0f);
						}
				}
				// 描画終了.
				m_draw2D.End ();
		}
}
