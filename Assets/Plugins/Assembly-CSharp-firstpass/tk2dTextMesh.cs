using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/tk2dTextMesh")]
[RequireComponent(typeof(MeshRenderer))]
public class tk2dTextMesh : MonoBehaviour
{
	[Flags]
	private enum UpdateFlags
	{
		UpdateNone = 0,
		UpdateText = 1,
		UpdateColors = 2,
		UpdateBuffers = 4
	}

	[SerializeField]
	private tk2dFontData _font;

	[SerializeField]
	private string _text = string.Empty;

	[SerializeField]
	private Color _color = Color.white;

	[SerializeField]
	private Color _color2 = Color.white;

	[SerializeField]
	private bool _useGradient;

	[SerializeField]
	private int _textureGradient;

	[SerializeField]
	private TextAnchor _anchor = TextAnchor.LowerLeft;

	[SerializeField]
	private Vector3 _scale = new Vector3(1f, 1f, 1f);

	[SerializeField]
	private bool _kerning;

	[SerializeField]
	private int _maxChars = 16;

	[SerializeField]
	private bool _inlineStyling;

	public bool pixelPerfect;

	public float spacing;

	private Vector3[] vertices;

	private Vector2[] uvs;

	private Vector2[] uv2;

	private Color[] colors;

	private UpdateFlags updateFlags = UpdateFlags.UpdateBuffers;

	private Mesh mesh;

	private MeshFilter meshFilter;

	public tk2dFontData font
	{
		get
		{
			return _font;
		}
		set
		{
			_font = value;
			updateFlags |= UpdateFlags.UpdateText;
		}
	}

	public string text
	{
		get
		{
			return _text;
		}
		set
		{
			_text = value;
			updateFlags |= UpdateFlags.UpdateText;
		}
	}

	public Color color
	{
		get
		{
			return _color;
		}
		set
		{
			_color = value;
			updateFlags |= UpdateFlags.UpdateColors;
		}
	}

	public Color color2
	{
		get
		{
			return _color2;
		}
		set
		{
			_color2 = value;
			updateFlags |= UpdateFlags.UpdateColors;
		}
	}

	public bool useGradient
	{
		get
		{
			return _useGradient;
		}
		set
		{
			_useGradient = value;
			updateFlags |= UpdateFlags.UpdateColors;
		}
	}

	public TextAnchor anchor
	{
		get
		{
			return _anchor;
		}
		set
		{
			_anchor = value;
			updateFlags |= UpdateFlags.UpdateText;
		}
	}

	public Vector3 scale
	{
		get
		{
			return _scale;
		}
		set
		{
			_scale = value;
			updateFlags |= UpdateFlags.UpdateText;
		}
	}

	public bool kerning
	{
		get
		{
			return _kerning;
		}
		set
		{
			_kerning = value;
			updateFlags |= UpdateFlags.UpdateText;
		}
	}

	public int maxChars
	{
		get
		{
			return _maxChars;
		}
		set
		{
			_maxChars = value;
			updateFlags |= UpdateFlags.UpdateBuffers;
		}
	}

	public int textureGradient
	{
		get
		{
			return _textureGradient;
		}
		set
		{
			_textureGradient = value % font.gradientCount;
			updateFlags |= UpdateFlags.UpdateText;
		}
	}

	public bool inlineStyling
	{
		get
		{
			return _inlineStyling;
		}
		set
		{
			_inlineStyling = value;
			updateFlags |= UpdateFlags.UpdateText;
		}
	}

	private bool useInlineStyling
	{
		get
		{
			return inlineStyling && _font.textureGradients;
		}
	}

	private void Awake()
	{
		if (pixelPerfect)
		{
			MakePixelPerfect();
		}
		initMesh();
		Init();
	}

	private void initMesh()
	{
		if (mesh == null)
		{
			meshFilter = GetComponent<MeshFilter>();
			mesh = new Mesh();
			meshFilter.mesh = mesh;
		}
	}

	public void setText(string txt)
	{
		text = txt;
		updateFlags |= UpdateFlags.UpdateText;
		Commit();
	}

	public int NumDrawnCharacters()
	{
		bool flag = useInlineStyling;
		int num = 0;
		for (int i = 0; i < _text.Length && num < _maxChars; i++)
		{
			int num2 = _text[i];
			if (num2 >= _font.chars.Length)
			{
				num2 = 0;
			}
			if (num2 == 10)
			{
				continue;
			}
			if (flag && num2 == 94 && i + 1 < _text.Length)
			{
				i++;
				if (_text[i] != '^')
				{
					continue;
				}
			}
			num++;
		}
		return num;
	}

	private void PostAlignTextData(int targetStart, int targetEnd, float offsetX)
	{
		for (int i = targetStart * 4; i < targetEnd * 4; i++)
		{
			Vector3 vector = vertices[i];
			vector.x += offsetX;
			vertices[i] = vector;
		}
	}

	private int FillTextData()
	{
		Vector2 vector = new Vector2((float)_textureGradient / (float)font.gradientCount, 0f);
		float yAnchorForHeight = GetYAnchorForHeight(GetMeshDimensionsForString(_text).y);
		bool flag = useInlineStyling;
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < _text.Length && num3 < _maxChars; i++)
		{
			int num5 = _text[i];
			if (num5 >= _font.chars.Length)
			{
				num5 = 0;
			}
			tk2dFontChar tk2dFontChar2 = _font.chars[num5];
			if (num5 == 10)
			{
				float lineWidth = num;
				int targetEnd = num3;
				if (num4 != num3)
				{
					float xAnchorForWidth = GetXAnchorForWidth(lineWidth);
					PostAlignTextData(num4, targetEnd, xAnchorForWidth);
				}
				num4 = num3;
				num = 0f;
				num2 -= _font.lineHeight * _scale.y;
				continue;
			}
			if (flag && num5 == 94 && i + 1 < _text.Length)
			{
				i++;
				if (_text[i] != '^')
				{
					int num6 = _text[i] - 48;
					vector = new Vector2((float)num6 / (float)font.gradientCount, 0f);
					continue;
				}
			}
			vertices[num3 * 4] = new Vector3(num + tk2dFontChar2.p0.x * _scale.x, yAnchorForHeight + num2 + tk2dFontChar2.p0.y * _scale.y, 0f);
			vertices[num3 * 4 + 1] = new Vector3(num + tk2dFontChar2.p1.x * _scale.x, yAnchorForHeight + num2 + tk2dFontChar2.p0.y * _scale.y, 0f);
			vertices[num3 * 4 + 2] = new Vector3(num + tk2dFontChar2.p0.x * _scale.x, yAnchorForHeight + num2 + tk2dFontChar2.p1.y * _scale.y, 0f);
			vertices[num3 * 4 + 3] = new Vector3(num + tk2dFontChar2.p1.x * _scale.x, yAnchorForHeight + num2 + tk2dFontChar2.p1.y * _scale.y, 0f);
			uvs[num3 * 4] = new Vector2(tk2dFontChar2.uv0.x, tk2dFontChar2.uv0.y);
			uvs[num3 * 4 + 1] = new Vector2(tk2dFontChar2.uv1.x, tk2dFontChar2.uv0.y);
			uvs[num3 * 4 + 2] = new Vector2(tk2dFontChar2.uv0.x, tk2dFontChar2.uv1.y);
			uvs[num3 * 4 + 3] = new Vector2(tk2dFontChar2.uv1.x, tk2dFontChar2.uv1.y);
			if (_font.textureGradients)
			{
				uv2[num3 * 4] = vector + tk2dFontChar2.gradientUv[0];
				uv2[num3 * 4 + 1] = vector + tk2dFontChar2.gradientUv[1];
				uv2[num3 * 4 + 2] = vector + tk2dFontChar2.gradientUv[2];
				uv2[num3 * 4 + 3] = vector + tk2dFontChar2.gradientUv[3];
			}
			num += (tk2dFontChar2.advance + spacing) * _scale.x;
			if (_kerning && i < _text.Length - 1)
			{
				tk2dFontKerning[] array = _font.kerning;
				foreach (tk2dFontKerning tk2dFontKerning2 in array)
				{
					if (tk2dFontKerning2.c0 == _text[i] && tk2dFontKerning2.c1 == _text[i + 1])
					{
						num += tk2dFontKerning2.amount * _scale.x;
						break;
					}
				}
			}
			num3++;
		}
		if (num4 != num3)
		{
			float lineWidth2 = num;
			int targetEnd2 = num3;
			float xAnchorForWidth2 = GetXAnchorForWidth(lineWidth2);
			PostAlignTextData(num4, targetEnd2, xAnchorForWidth2);
		}
		return num3;
	}

	public void Init(bool force)
	{
		if (force)
		{
			updateFlags |= UpdateFlags.UpdateBuffers;
		}
		Init();
	}

	public void Init()
	{
		if (!_font || (updateFlags & UpdateFlags.UpdateBuffers) == 0)
		{
			return;
		}
		Color color = _color;
		Color color2 = _useGradient ? _color2 : _color;
		vertices = new Vector3[_maxChars * 4];
		uvs = new Vector2[_maxChars * 4];
		colors = new Color[_maxChars * 4];
		if (_font.textureGradients)
		{
			uv2 = new Vector2[_maxChars * 4];
		}
		int[] array = new int[_maxChars * 6];
		int num = FillTextData();
		for (int i = 0; i < num; i++)
		{
			colors[i * 4] = (colors[i * 4 + 1] = color);
			colors[i * 4 + 2] = (colors[i * 4 + 3] = color2);
			array[i * 6] = i * 4;
			array[i * 6 + 1] = i * 4 + 1;
			array[i * 6 + 2] = i * 4 + 3;
			array[i * 6 + 3] = i * 4 + 2;
			array[i * 6 + 4] = i * 4;
			array[i * 6 + 5] = i * 4 + 3;
		}
		for (int j = num; j < _maxChars; j++)
		{
			vertices[j * 4] = (vertices[j * 4 + 1] = (vertices[j * 4 + 2] = (vertices[j * 4 + 3] = Vector3.zero)));
			uvs[j * 4] = (uvs[j * 4 + 1] = (uvs[j * 4 + 2] = (uvs[j * 4 + 3] = Vector2.zero)));
			if (_font.textureGradients)
			{
				uv2[j * 4] = (uv2[j * 4 + 1] = (uv2[j * 4 + 2] = (uv2[j * 4 + 3] = Vector2.zero)));
			}
			colors[j * 4] = (colors[j * 4 + 1] = color);
			colors[j * 4 + 2] = (colors[j * 4 + 3] = color2);
			array[j * 6] = j * 4;
			array[j * 6 + 1] = j * 4 + 1;
			array[j * 6 + 2] = j * 4 + 3;
			array[j * 6 + 3] = j * 4 + 2;
			array[j * 6 + 4] = j * 4;
			array[j * 6 + 5] = j * 4 + 3;
		}
		if (mesh == null)
		{
			initMesh();
		}
		if (mesh != null)
		{
			mesh.Clear();
			if (mesh.vertices == null)
			{
				return;
			}
			mesh.vertices = vertices;
			mesh.uv = uvs;
			if (font.textureGradients)
			{
				mesh.uv2 = uv2;
			}
			mesh.triangles = array;
			mesh.colors = colors;
			mesh.RecalculateBounds();
		}
		updateFlags = UpdateFlags.UpdateNone;
	}

	public void Commit()
	{
		if ((updateFlags & UpdateFlags.UpdateBuffers) != 0)
		{
			Init();
		}
		else
		{
			if ((updateFlags & UpdateFlags.UpdateText) != 0)
			{
				int num = FillTextData();
				for (int i = num; i < _maxChars; i++)
				{
					vertices[i * 4] = (vertices[i * 4 + 1] = (vertices[i * 4 + 2] = (vertices[i * 4 + 3] = Vector3.zero)));
				}
				if (mesh == null || mesh.vertices == null)
				{
					return;
				}
				mesh.vertices = vertices;
				mesh.uv = uvs;
				if (font.textureGradients)
				{
					mesh.uv2 = uv2;
				}
			}
			if ((updateFlags & UpdateFlags.UpdateColors) != 0)
			{
				Color color = _color;
				Color color2 = _useGradient ? _color2 : _color;
				for (int j = 0; j < colors.Length; j += 4)
				{
					colors[j] = (colors[j + 1] = color);
					colors[j + 2] = (colors[j + 3] = color2);
				}
				mesh.colors = colors;
			}
		}
		mesh.RecalculateBounds();
		updateFlags = UpdateFlags.UpdateNone;
	}

	private Vector2 GetMeshDimensionsForString(string str)
	{
		bool flag = useInlineStyling;
		float b = 0f;
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		for (int i = 0; i < _text.Length && num3 < _maxChars; i++)
		{
			int num4 = _text[i];
			if (num4 >= _font.chars.Length)
			{
				num4 = 0;
			}
			tk2dFontChar tk2dFontChar2 = _font.chars[num4];
			if (num4 == 10)
			{
				b = Mathf.Max(num, b);
				num = 0f;
				num2 -= _font.lineHeight * _scale.y;
				continue;
			}
			if (flag && num4 == 94 && i + 1 < _text.Length)
			{
				i++;
				if (_text[i] != '^')
				{
					continue;
				}
			}
			num += (tk2dFontChar2.advance + spacing) * _scale.x;
			if (_kerning && i < _text.Length - 1)
			{
				tk2dFontKerning[] array = _font.kerning;
				foreach (tk2dFontKerning tk2dFontKerning2 in array)
				{
					if (tk2dFontKerning2.c0 == _text[i] && tk2dFontKerning2.c1 == _text[i + 1])
					{
						num += tk2dFontKerning2.amount * _scale.x;
						break;
					}
				}
			}
			num3++;
		}
		b = Mathf.Max(num, b);
		num2 -= _font.lineHeight * _scale.y;
		return new Vector2(b, num2);
	}

	private float GetYAnchorForHeight(float textHeight)
	{
		int num = (int)_anchor / 3;
		float num2 = _font.lineHeight * _scale.y;
		switch (num)
		{
		case 0:
			return 0f - num2;
		case 1:
			return (0f - textHeight) / 2f - num2;
		case 2:
			return 0f - textHeight - num2;
		default:
			return 0f - num2;
		}
	}

	private float GetXAnchorForWidth(float lineWidth)
	{
		switch ((int)_anchor % 3)
		{
		case 0:
			return 0f;
		case 1:
			return (0f - lineWidth) / 2f;
		case 2:
			return 0f - lineWidth;
		default:
			return 0f;
		}
	}

	public void MakePixelPerfect()
	{
		float num = 1f;
		tk2dPixelPerfectHelper inst = tk2dPixelPerfectHelper.inst;
		if ((bool)inst)
		{
			num = inst.CameraIsOrtho ? inst.scaleK : (inst.scaleK + inst.scaleD * transform.position.z);
		}
		else if ((bool)Camera.main)
		{
			if (Camera.main.orthographic)
			{
				num = Camera.main.orthographicSize;
			}
			else
			{
				float zdist = transform.position.z - Camera.main.transform.position.z;
				num = tk2dPixelPerfectHelper.CalculateScaleForPerspectiveCamera(Camera.main.fieldOfView, zdist);
			}
		}
		scale = new Vector3(Mathf.Sign(scale.x) * num, Mathf.Sign(scale.y) * num, Mathf.Sign(scale.z) * num);
	}
}
