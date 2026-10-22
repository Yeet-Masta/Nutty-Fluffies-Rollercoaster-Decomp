using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ObjectMonitor : MonoBehaviour
{
	[NonSerialized]
	public static int ROW_HEIGHT = 20;

	public virtual void Awake()
	{
		useGUILayout = false;
	}

	public static Texture[] getUniqueTextures()
	{
		object[] array = Resources.FindObjectsOfTypeAll(typeof(Texture));
		List<Texture> list = new List<Texture>();
		int i = 0;
		object[] array2 = array;
		for (int length = array2.Length; i < length; i++)
		{
			Texture texture = array2[i] as Texture;
			bool flag = false;
			if (texture.name.Length > 0)
			{
				IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(list);
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					if (!(obj is Texture))
					{
						obj = RuntimeServices.Coerce(obj, typeof(Texture));
					}
					Texture texture2 = (Texture)obj;
					if (texture2.name == texture.name)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				list.Add(texture);
			}
		}
		return list.ToArray();
	}

	public virtual void OnGUI()
	{
		Rect position = new Rect(220f, 20f, 400f, ROW_HEIGHT);
		GUI.Label(position, "All " + Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)).Length);
		position.y += ROW_HEIGHT;
		GUI.Label(position, "Textures " + Resources.FindObjectsOfTypeAll(typeof(Texture)).Length);
		position.y += ROW_HEIGHT;
		GUI.Label(position, "AudioClips " + Resources.FindObjectsOfTypeAll(typeof(AudioClip)).Length);
		position.y += ROW_HEIGHT;
		GUI.Label(position, "Meshes " + Resources.FindObjectsOfTypeAll(typeof(Mesh)).Length);
		position.y += ROW_HEIGHT;
		GUI.Label(position, "GameObjects " + Resources.FindObjectsOfTypeAll(typeof(GameObject)).Length);
		position.y += ROW_HEIGHT;
		GUI.Label(position, "Components " + Resources.FindObjectsOfTypeAll(typeof(Component)).Length);
	}

	public static void printTextureNames()
	{
		Texture[] uniqueTextures = getUniqueTextures();
		int num = 0;
		int i = 0;
		Texture[] array = uniqueTextures;
		for (int length = array.Length; i < length; i++)
		{
			Debug.Log("texture " + num + ": " + array[i].name + "(" + array[i].width + " x " + array[i].height + ") ID=" + array[i].GetNativeTexturePtr());
			if (array[i] is Texture2D)
			{
				Texture2D texture2D = array[i] as Texture2D;
				if (texture2D.format == TextureFormat.ARGB32 || texture2D.format == TextureFormat.RGB24)
				{
					byte[] bytes = texture2D.EncodeToPNG();
					string rhs = texture2D.name.Replace("/", "_");
					File.WriteAllBytes("TEMP/" + num + " (" + rhs + ").png", bytes);
				}
			}
			num++;
		}
	}

	public static void printTextureNames2()
	{
		UnityScript.Lang.Array array = Resources.FindObjectsOfTypeAll(typeof(Texture));
		int num = 0;
		Debug.Log("There are " + array.Count + " textures loaded");
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(array);
		while (enumerator.MoveNext())
		{
			object current = enumerator.Current;
			Texture texture = current as Texture;
			UnityRuntimeServices.Update(enumerator, current);
			Debug.Log("texture " + num + ": " + texture.name + "(" + texture.width + " x " + texture.height + ")");
			num++;
		}
	}

	public static void printMeshNames()
	{
		UnityScript.Lang.Array obj = Resources.FindObjectsOfTypeAll(typeof(Mesh));
		int num = 0;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(obj);
		while (enumerator.MoveNext())
		{
			object current = enumerator.Current;
			Mesh mesh = current as Mesh;
			UnityRuntimeServices.Update(enumerator, current);
			Debug.Log("mesh " + num + ": " + mesh.name);
			num++;
		}
	}

	public static void printAudioClipNames()
	{
		UnityScript.Lang.Array obj = Resources.FindObjectsOfTypeAll(typeof(AudioClip));
		int num = 0;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(obj);
		while (enumerator.MoveNext())
		{
			object current = enumerator.Current;
			AudioClip audioClip = current as AudioClip;
			UnityRuntimeServices.Update(enumerator, current);
			Debug.Log("audioClip: " + num + ": " + audioClip.name + "(" + audioClip.length + " seconds)");
			num++;
		}
	}

	public static void checkSprites()
	{
		UnityScript.Lang.Array array = Resources.FindObjectsOfTypeAll(typeof(tk2dSprite));
		int num = 0;
		Debug.Log("THERE ARE " + array.Count + " SPRITES");
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(array);
		while (enumerator.MoveNext())
		{
			object current = enumerator.Current;
			tk2dSprite tk2dSprite2 = current as tk2dSprite;
			UnityRuntimeServices.Update(enumerator, current);
			Vector3 scale = tk2dSprite2.getScale();
			if (scale.x > 1f || !(scale.y <= 1f))
			{
				Debug.LogWarning("SPRITE " + tk2dSprite2.gameObject.name + " HAS ILLEGAL SCALE: " + scale);
				if (tk2dSprite2.transform.parent != null)
				{
					Debug.Log("PARENT IS " + tk2dSprite2.transform.parent.gameObject.name);
				}
				else
				{
					Debug.Log("OBJECT IS NOT IN THE SCENE");
				}
			}
		}
	}

	public static void checkSprites(Transform rootNode)
	{
		object[] array = (tk2dSprite[])UnityEngine.Object.FindObjectsOfType(typeof(tk2dSprite));
		int num = 0;
		Debug.Log("THERE ARE " + array.Length + " SPRITES");
		int i = 0;
		object[] array2 = array;
		for (int length = array2.Length; i < length; i++)
		{
			tk2dSprite tk2dSprite2 = array2[i] as tk2dSprite;
			Vector3 scale = tk2dSprite2.getScale();
			if (scale.x > 1f || !(scale.y <= 1f))
			{
				Debug.LogWarning("SPRITE " + tk2dSprite2.gameObject.name + " HAS ILLEGAL SCALE: " + scale);
				if (tk2dSprite2.transform.parent != null)
				{
					Debug.Log("PARENT IS " + tk2dSprite2.transform.parent.gameObject.name);
				}
				else
				{
					Debug.Log("OBJECT IS NOT IN THE SCENE");
				}
			}
		}
	}
}
