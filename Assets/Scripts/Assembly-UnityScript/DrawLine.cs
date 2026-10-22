using System;
using UnityEngine;

[Serializable]
public class DrawLine : MonoBehaviour
{
	[NonSerialized]
	public static Texture2D lineTex;

	[NonSerialized]
	public static LineStyle lineStyle;

	[NonSerialized]
	public static Material lineMaterial;

	[NonSerialized]
	public static float lineAnimTime;

	private static void checkTexture()
	{
		if (!lineTex)
		{
			lineTex = new Texture2D(1, 1);
			lineTex.SetPixel(0, 0, Color.white);
			lineTex.Apply();
		}
	}

	public static void StartLines(Color color, LineStyle style)
	{
		if (lineMaterial == null)
		{
			CreateLineMaterial();
		}
		lineStyle = style;
		if (style == LineStyle.SOLID)
		{
			lineAnimTime = Time.time;
		}
		lineMaterial.color = color;
		lineMaterial.SetColor("_Color", color);
		lineMaterial.SetPass(0);
		GL.PushMatrix();
		GL.LoadOrtho();
		GL.LoadPixelMatrix();
		GL.Begin(1);
	}

	public static void EndLines()
	{
		GL.End();
		GL.PopMatrix();
	}

	public static void OneLine(Vector2 pointA, Vector2 pointB)
	{
		int num = 5;
		Camera main = Camera.main;
		GL.Color(lineMaterial.color);
		switch (lineStyle)
		{
		case LineStyle.SOLID:
			GL.Vertex(new Vector2(main.pixelRect.x + main.rect.width * pointA.x, (float)Screen.height - (main.pixelRect.y + main.rect.height * pointA.y)));
			GL.Vertex(new Vector2(main.pixelRect.x + main.rect.width * pointB.x, (float)Screen.height - (main.pixelRect.y + main.rect.height * pointB.y)));
			return;
		case LineStyle.DOTTED:
			num = 2;
			break;
		case LineStyle.DASHED:
			break;
		default:
			return;
		}
		Vector2 vector = new Vector2(pointA.x, pointA.y);
		Vector2 vector2 = new Vector2(pointB.x, pointB.y);
		float num2 = Vector2.Distance(vector, vector2);
		(vector2 - vector).Normalize();
		float num3 = (1f - 2f * (lineAnimTime - Mathf.Floor(lineAnimTime)) + 0.5f) / (float)(num * 2);
		float num4 = (2f - 2f * (lineAnimTime - Mathf.Floor(lineAnimTime)) + 0.5f) / (float)(num * 2);
		float num5 = 0f;
		float num6 = 1f / (float)num;
		for (int i = 0; i < num; i++)
		{
			Vector2 vector3 = Vector2.Lerp(vector, vector2, num3);
			Vector2 vector4 = Vector2.Lerp(vector, vector2, num4);
			GL.Vertex(new Vector2(main.pixelRect.x + main.rect.width * vector3.x, (float)Screen.height - (main.pixelRect.y + main.rect.height * vector3.y)));
			GL.Vertex(new Vector2(main.pixelRect.x + main.rect.width * vector4.x, (float)Screen.height - (main.pixelRect.y + main.rect.height * vector4.y)));
			num3 += num6;
			num4 += num6;
		}
	}

	public static void DrawLineImpl(Vector2 pointA, Vector2 pointB, Color color, float width)
	{
		StartLines(color, LineStyle.SOLID);
		OneLine(pointA, pointB);
		EndLines();
	}

	public static void DrawLineImpl(Rect rect, Color color, float width)
	{
		DrawLineImpl(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), color, width);
	}

	public static void DrawLineImpl(Rect rect, float width)
	{
		DrawLineImpl(rect, GUI.contentColor, width);
	}

	public static void DrawLineImpl(Rect rect)
	{
		DrawLineImpl(rect, GUI.contentColor, 1f);
	}

	public static void DrawLineImpl(Rect rect, Color color)
	{
		DrawLineImpl(rect, color, 1f);
	}

	public static void DrawLineImpl(Vector2 pointA, Vector2 pointB)
	{
		DrawLineImpl(pointA, pointB, GUI.contentColor, 1f);
	}

	public static void DrawLineImpl(Vector2 pointA, Vector2 pointB, Color color)
	{
		DrawLineImpl(pointA, pointB, color, 1f);
	}

	public static void DrawLineImpl(Vector2 pointA, Vector2 pointB, float width)
	{
		DrawLineImpl(pointA, pointB, GUI.contentColor, width);
	}

	public static void CreateLineMaterial()
	{
		if (!lineMaterial)
		{
			// NFPort: materials can no longer be created from shader source text.
			// Unity's built-in colored shader with the same states: vertex colors,
			// unlit, ZTest Always, ZWrite Off, Cull Front, no blending.
			lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
			lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
			lineMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
			lineMaterial.SetInt("_ZWrite", 0);
			lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Front);
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	public static void DrawLine3D(Vector2 v1, Vector2 v2, Color color, float width)
	{
		Vector3 vector = default(Vector3);
		Vector3 vector2 = default(Vector3);
		vector = Camera.main.WorldToScreenPoint(v1);
		vector.y = (float)Screen.height - vector.y;
		vector2 = Camera.main.WorldToScreenPoint(v2);
		vector2.y = (float)Screen.height - vector2.y;
		DrawLineImpl(vector, vector2, color, width);
	}

	public static void DrawBox3D(Vector2 v1, Vector2 v2, Color color, float width)
	{
		Vector3 vector = default(Vector3);
		Vector3 vector2 = default(Vector3);
		vector = Camera.main.WorldToScreenPoint(v1);
		vector.y = (float)Screen.height - vector.y;
		vector2 = Camera.main.WorldToScreenPoint(v2);
		vector2.y = (float)Screen.height - vector2.y;
		DrawLineImpl(new Vector3(vector.x, vector.y, vector.z), new Vector3(vector2.x, vector.y, vector.z), color, width);
		DrawLineImpl(new Vector3(vector2.x, vector.y, vector.z), new Vector3(vector2.x, vector2.y, vector.z), color, width);
		DrawLineImpl(new Vector3(vector2.x, vector2.y, vector.z), new Vector3(vector.x, vector2.y, vector.z), color, width);
		DrawLineImpl(new Vector3(vector.x, vector2.y, vector.z), new Vector3(vector.x, vector.y, vector.z), color, width);
	}

	public static void FillRect(float x, float y, float w, float h, Color color)
	{
		checkTexture();
		Color color2 = GUI.color;
		GUI.color = color;
		checkTexture();
		GUI.DrawTexture(new Rect(x, y, w, h), lineTex);
		GUI.color = color2;
	}

	public virtual void Main()
	{
	}
}
