using UnityEngine;
using System.Collections;

// 2D描画を行うクラス.
public class UtilDraw2D {
	private Material m_lineMaterial = null;
	private float m_lineWidthHalf = 0.0f;		// 描画するラインの太さの半分. 0.0で太さ指定なし.
	private float m_lineDot       = 0.0f;		// ライン描画時の破線間隔 (pixel数). 0.0で破線なし.
	
	public UtilDraw2D() {
		// ライン描画用のマテリアルを生成.
		m_lineMaterial = new Material(
		    "Shader \"myShader\" {" +
		    "  SubShader {" +
		    "    Pass {" +
		    "       ZWrite Off" +
		    "       Cull Off" + 
		    "       BindChannels {" +
		    "         Bind \"vertex\", vertex Bind \"color\", color" +
		    "       }" +
		    "    }" +
		    "  }" +
		    "}"			
		);
		m_lineMaterial.hideFlags = HideFlags.HideAndDontSave;
		m_lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
	}
	
	// ラインの太さ.
	public void SetLineWidth(float width) {
		m_lineWidthHalf = width * 0.5f;
	}
	// 破線間隔.
	public void SetLineDot(float lineDot) {
		m_lineDot = lineDot;
	}
	
	// GL描画を開始する.
	public void Begin() {
		if (m_lineMaterial != null) m_lineMaterial.SetPass(0);
		
		GL.PushMatrix();
		GL.LoadPixelMatrix();
		GL.Viewport(new Rect(0, 0, Screen.width, Screen.height));
	}
	
	// GL描画を終了する.
	public void End() {
		GL.PopMatrix();
	}
	
	// 背景をクリア.
	public void Clear(Color col, bool z_depth = true) {
		GL.Clear(z_depth, true, col);
	}
	
	// 太さのある線を描画.
	// @param[in]  v0   開始位置.
	// @param[in]  v1   終了位置.
	public void m_DrawLine(Vector3 v0, Vector3 v1) {
		if (m_lineWidthHalf < Mathf.Epsilon) {
			GL.Vertex3(v0.x, v0.y, 0.0f);
			GL.Vertex3(v1.x, v1.y, 0.0f);
		} else {
			Vector3 n = ((new Vector3(v1.y, v0.x, 0.0f)) - (new Vector3(v0.y, v1.x, 0.0f))).normalized * m_lineWidthHalf;
			GL.Vertex3(v0.x - n.x, v0.y - n.y, 0.0f);
			GL.Vertex3(v0.x + n.x, v0.y + n.y, 0.0f);
			GL.Vertex3(v1.x + n.x, v1.y + n.y, 0.0f);
			GL.Vertex3(v1.x - n.x, v1.y - n.y, 0.0f);
		}
	}
	
	// 線群を描画.
	// @param[in]  pos     頂点座標.
	private void m_DrawLines(Vector3 [] pos) {
		for (int i = 0; i < pos.Length - 1; i++) m_DrawLine(pos[i], pos[i + 1]);
	}
	
	// 点線群を描画.
	// @param[in]  pos    頂点座標.
	private void m_DrawDotLines(Vector3 [] pos) {
		Vector3 vv0, vv1, v0, v1, dv;
		float len, vpos, dLen;
		bool drawF    = true;
		float chk_len = 0.0f;
		for (int i = 0; i < pos.Length - 1; i++) {
			v0 = pos[i];
			v1 = pos[i + 1];
			len = Vector3.Distance(v0, v1);
			if (len < Mathf.Epsilon) continue;
			dv = Vector3.Normalize(v1 - v0);
			vpos = 0.0f;
			dLen = 0.0f;
			while (len > Mathf.Epsilon) {
				if (chk_len >= m_lineDot - Mathf.Epsilon) {
					drawF = (drawF) ? false : true;
					chk_len = 0.0f;
				}
				if (chk_len + len <= m_lineDot) {
					dLen = len - chk_len;
					if (dLen < 0.0f) dLen = len;
					if (drawF) {
						vv0 = (dv * vpos) + v0;
						vv1 = (dv * (vpos + dLen)) + v0;
						m_DrawLine(vv0, vv1);
					}
					chk_len += dLen;
					len -= dLen;
					break;
				}
				dLen = m_lineDot - chk_len;
				if (drawF) {
					vv0 = (dv * vpos) + v0;
					vv1 = (dv * (vpos + dLen)) + v0;
					m_DrawLine(vv0, vv1);
				}
				chk_len += dLen;
				
				vpos += dLen;
				len  -= dLen;
			}
		}
	}
	
	// ポリゴン描画.
	// @param[in]  pos    頂点座標.
	// @param[in]  col    描画色.
	public void DrawPolyon(Vector3 [] pos, Color col) {
		GL.Begin(GL.TRIANGLES);
		GL.Color(col);
		
		Vector3 v0 = pos[0];
		for (int i = 1; i < pos.Length - 1; i++) {
			Vector3 v1 = pos[i];
			Vector3 v2 = pos[i + 1];
			GL.Vertex3(v0.x, v0.y, v0.z);
			GL.Vertex3(v1.x, v1.y, v1.z);
			GL.Vertex3(v2.x, v2.y, v2.z);
		}
		GL.End();
	}
	
	// ライン描画.
	// @param[in]  pos    頂点座標.
	// @param[in]  col    描画色.
	public void DrawLines(Vector3 [] pos, Color col) {
		if (m_lineWidthHalf > Mathf.Epsilon) {
			GL.Begin(GL.QUADS);
			GL.Color(col);
			if (m_lineDot > Mathf.Epsilon) m_DrawDotLines(pos);
			else m_DrawLines(pos);
			GL.End();
		} else {
			GL.Begin(GL.LINES);
			GL.Color(col);
			if (m_lineDot > Mathf.Epsilon) m_DrawDotLines(pos);
			else m_DrawLines(pos);
			GL.End();
		}
	}

	// ボックスの描画.
	// @param[in]  p0   開始点.
	// @param[in]  p1   終了点.
	// @param[in]  col  描画色.
	public void DrawRectangle(Vector3 p0, Vector3 p1, Color col) {
		float x1 = p0.x;
		float x2 = p1.x;
		if (x1 > x2) {
			float v = x1;
			x1 = x2;
			x2 = v;
		}
		float y1 = p0.y;
		float y2 = p1.y;
		if (y1 > y2) {
			float v = y1;
			y1 = y2;
			y2 = v;
		}

		Vector3 [] pos = new Vector3[5];
		pos[0] = new Vector3(x1, y1, 0.0f);
		pos[1] = new Vector3(x1, y2, 0.0f);
		pos[2] = new Vector3(x2, y2, 0.0f);
		pos[3] = new Vector3(x2, y1, 0.0f);
		pos[4] = new Vector3(x1, y1, 0.0f);
		DrawLines(pos, col);
	}
	
	// ボックスの塗りつぶし描画.
	// @param[in]  p0   開始点.
	// @param[in]  p1   終了点.
	// @param[in]  col  描画色.
	public void DrawRectangleFill(Vector3 p0, Vector3 p1, Color col) {
		float x1 = p0.x;
		float x2 = p1.x;
		if (x1 > x2) {
			float v = x1;
			x1 = x2;
			x2 = v;
		}
		float y1 = p0.y;
		float y2 = p1.y;
		if (y1 > y2) {
			float v = y1;
			y1 = y2;
			y2 = v;
		}

		GL.Begin(GL.QUADS);
		GL.Color(col);
		GL.Vertex3(x1, y1, 0.0f);
		GL.Vertex3(x1, y2, 0.0f);
		GL.Vertex3(x2, y2, 0.0f);
		GL.Vertex3(x2, y1, 0.0f);
		GL.End();		
	}
	
	
	// 円の塗りつぶし描画.
	// @param[in]  center  中心位置.
	// @param[in]  radius  半径.
	// @param[in]  col     描画色.
	public void DrawCircleFill(Vector3 center, float radius, Color col) {
		int divCou = 32;
		GL.Begin(GL.TRIANGLES);
		GL.Color(col);
	
		float dPos = 0.0f;
		float dd   = (Mathf.PI * 2.0f) / (float)divCou;
		Vector3 v0 = new Vector3(0.0f, 0.0f);
		Vector3 v1 = new Vector3(0.0f, 0.0f);
		for (int i = 0; i <= divCou; i++) {
			v1.x = Mathf.Cos(dPos) * radius;
			v1.y = Mathf.Sin(dPos) * radius;
			v1.z = 0.0f;
			v1 += center;
			if (i != 0) {
				GL.Vertex3(center.x, center.y, center.z);
				GL.Vertex3(v1.x, v1.y, v1.z);
				GL.Vertex3(v0.x, v0.y, v0.z);
			}
			v0 = v1;
			dPos += dd; 
		}
		GL.End();
	}
	
	// 円の描画.
	// @param[in]  center  中心位置.
	// @param[in]  radius  半径.
	// @param[in]  col     描画色.
	public void DrawCircle(Vector3 center, float radius, Color col) {
		int divCou = 32;
		float dPos = 0.0f;
		float dd   = (Mathf.PI * 2.0f) / (float)divCou;
		Vector3 v0 = new Vector3(0.0f, 0.0f);
	
		Vector3 [] posA = new Vector3[divCou + 1];
		for (int i = 0; i <= divCou; i++) {
			v0.x = Mathf.Cos(dPos) * radius;
			v0.y = Mathf.Sin(dPos) * radius;
			v0.z = 0.0f;
			posA[i] = v0 + center;
			dPos += dd;
		}
		DrawLines(posA, col);
	}
	
	// 4頂点のベジェの位置を取得.
	private Vector3 m_GetBezierPos(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float pos) {
		double t = pos;
		double t1 = 1.0 - t;
		double t2 = t1 * t1;
		double t3 = t2 * t1;
		double b1 = t3;
		double b2 = 3.0 * t * t2;
		double tt = t * t;
		double b3 = 3.0 * tt * t1;
		double b4 = tt * t;
		return (p0 * (float)b1 + p1 * (float)b2 + p2 * (float)b3 + p3 * (float)b4);
	}
	
	// ベジェ曲線を描画.
	public void DrawBezier(Vector3 [] pos, Color col) {
		int divCou = 8;
		float dd = 1.0f / (float)divCou;
		float dPos;
		
		Vector3 [] posA = new Vector3[divCou * ((pos.Length) / 4)];
		int iPos = 0;
		for (int i = 0; i < pos.Length; i += 4) {
			dPos = 0.0f;
			for (int j = 0; j < divCou; j++) {
				posA[iPos] = m_GetBezierPos(pos[i], pos[i + 1], pos[i + 2], pos[i + 3], dPos);
				iPos++;
				dPos += dd;
			}
		}
		DrawLines(posA, col);
	}
}
