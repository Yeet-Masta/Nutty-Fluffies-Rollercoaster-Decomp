using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class TextureManager
{
	[Serializable]
	public class UV
	{
		public float u1;

		public float v1;

		public float u2;

		public float v2;

		public float width;

		public float height;

		public float scaleX;

		public float scaleY;

		public virtual void copyFrom(UV uv)
		{
			u1 = uv.u1;
			v1 = uv.v1;
			u2 = uv.u2;
			v2 = uv.v2;
			width = uv.width;
			height = uv.height;
			scaleX = uv.scaleX;
			scaleY = uv.scaleY;
		}
	}

	[Serializable]
	public class TextureDescriptor
	{
		public string filename;

		public UV uv;

		public TextureDescriptor()
		{
			uv = new UV();
		}

		public virtual bool load(string descFile)
		{
			string text = FileManager.getInstance().loadFileText(descFile);
			int result;
			if (text == null)
			{
				result = 0;
			}
			else
			{
				JSONObject jSONObject = new JSONObject(text);
				filename = jSONObject.GetFieldSafe("img", new JSONObject("\"none\"")).str;
				uv.scaleX = (float)jSONObject.GetFieldSafe("scalex", new JSONObject(1f)).n;
				uv.scaleY = (float)jSONObject.GetFieldSafe("scaley", new JSONObject(1f)).n;
				uv.u1 = (float)jSONObject.GetFieldSafe("u1", new JSONObject(0f)).n;
				uv.v1 = (float)jSONObject.GetFieldSafe("v1", new JSONObject(0f)).n;
				uv.u2 = (float)jSONObject.GetFieldSafe("u2", new JSONObject(1f)).n;
				uv.v2 = (float)jSONObject.GetFieldSafe("v2", new JSONObject(1f)).n;
				uv.width = uv.u2 - uv.u1;
				uv.height = uv.v2 - uv.v1;
				result = 1;
			}
			return (byte)result != 0;
		}
	}

	[Serializable]
	public enum TextureType
	{
		OBJECT = 0,
		SPLINE = 1
	}

	[Serializable]
	public class TextureCacheEntry
	{
		public Texture2D tex;

		public UV uvs;
	}

	private static TextureManager instance;

	private static Texture2D errorTexture;

	public static string[] typePaths = new string[2] { "Objects", "Spline" };

	private Dictionary<string, Texture2D> textureCacheList;

	private Dictionary<string, TextureCacheEntry> textureDescriptorList;

	private TextureManager()
	{
		textureCacheList = new Dictionary<string, Texture2D>();
		textureDescriptorList = new Dictionary<string, TextureCacheEntry>();
		if (errorTexture == null)
		{
			errorTexture = createAssetTexture("error.png");
		}
	}

	public virtual void purge()
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(textureCacheList.Values);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Texture2D))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Texture2D));
			}
			Texture2D texture2D = (Texture2D)obj;
			if (texture2D != errorTexture)
			{
				if (GameData.LOG)
				{
					Debug.Log("texture : " + texture2D.name);
					UnityRuntimeServices.Update(enumerator, texture2D);
				}
				UnityEngine.Object.DestroyImmediate(texture2D);
				UnityRuntimeServices.Update(enumerator, texture2D);
			}
		}
		textureCacheList.Clear();
		textureDescriptorList.Clear();
	}

	public static string texturePathForType(TextureType type)
	{
		return typePaths[(int)type];
	}

	public static TextureManager getInstance()
	{
		if (RuntimeServices.EqualityOperator(instance, null))
		{
			instance = new TextureManager();
		}
		return instance;
	}

	public virtual Texture2D loadFile(string fileName)
	{
		object result;
		if (string.IsNullOrEmpty(fileName))
		{
			result = null;
		}
		else if (textureCacheList.ContainsKey(fileName))
		{
			result = textureCacheList[fileName];
		}
		else
		{
			Texture2D texture2D = new Texture2D(4, 4, TextureFormat.ARGB4444, true);
			byte[] array = FileManager.getInstance().loadFileBytes(fileName);
			if (array == null)
			{
				if (GameData.LOG)
				{
					Debug.Log("FAILED!!! NO TEXTURE PNG IN: " + fileName);
				}
				result = null;
			}
			else
			{
				texture2D.LoadImage(array);
				texture2D.filterMode = FilterMode.Bilinear;
				texture2D.anisoLevel = 1;
				texture2D.name = fileName;
				textureCacheList[fileName] = texture2D;
				result = texture2D;
			}
		}
		return (Texture2D)result;
	}

	public virtual Texture2D loadFileWithOutMipmap(string fileName)
	{
		object result;
		if (string.IsNullOrEmpty(fileName))
		{
			result = null;
		}
		else if (textureCacheList.ContainsKey(fileName))
		{
			result = textureCacheList[fileName];
		}
		else
		{
			Texture2D texture2D = new Texture2D(4, 4, TextureFormat.ARGB4444, false);
			byte[] array = FileManager.getInstance().loadFileBytes(fileName);
			if (array == null)
			{
				if (GameData.LOG)
				{
					Debug.Log("FAILED!!! NO TEXTURE PNG IN: " + fileName);
				}
				result = null;
			}
			else
			{
				texture2D.LoadImage(array);
				texture2D.filterMode = FilterMode.Bilinear;
				texture2D.anisoLevel = 1;
				texture2D.name = fileName;
				textureCacheList[fileName] = texture2D;
				result = texture2D;
			}
		}
		return (Texture2D)result;
	}

	public virtual Texture2D createTexture(string fullTexturePath, UV uvs)
	{
		CustomProfiler.begin("createTexture");
		TextureDescriptor textureDescriptor = null;
		string text = fullTexturePath + ".txt";
		TextureCacheEntry textureCacheEntry = null;
		Texture2D result;
		if (textureDescriptorList.ContainsKey(text))
		{
			textureCacheEntry = textureDescriptorList[text];
			if (uvs != null && textureCacheEntry.uvs != null)
			{
				uvs.copyFrom(textureCacheEntry.uvs);
			}
			CustomProfiler.end("createTexture");
			result = textureCacheEntry.tex;
		}
		else
		{
			textureDescriptor = new TextureDescriptor();
			if (textureDescriptor.load(text))
			{
				fullTexturePath = FileManager.getDirectoryName(fullTexturePath) + "/" + textureDescriptor.filename;
			}
			Texture2D texture2D = getInstance().loadFile(fullTexturePath);
			if (texture2D == null)
			{
				texture2D = errorTexture;
			}
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.mipMapBias = -0.5f;
			if (textureDescriptor != null)
			{
				textureCacheEntry = new TextureCacheEntry();
				textureCacheEntry.tex = texture2D;
				textureCacheEntry.uvs = new UV();
				textureCacheEntry.uvs.copyFrom(textureDescriptor.uv);
				if (uvs != null)
				{
					uvs.copyFrom(textureDescriptor.uv);
				}
				textureDescriptorList.Add(text, textureCacheEntry);
			}
			CustomProfiler.end("createTexture");
			result = texture2D;
		}
		return result;
	}

	public virtual Texture2D createAssetTexture(string texture)
	{
		CustomProfiler.begin("createAssetTexture");
		Texture2D result = Resources.Load(texture.ToLower().Replace(".png", string.Empty)) as Texture2D;
		CustomProfiler.end("createAssetTexture");
		return result;
	}

	public virtual Texture2D createTexture(string packPath, TextureType texType, string texture, UV uvs)
	{
		string fullTexturePath = packPath + "/textures/" + CommonData.getDevicePath() + "/" + texturePathForType(texType) + "/" + texture;
		return createTexture(fullTexturePath, uvs);
	}

	public virtual void release(Texture2D tex)
	{
	}
}
