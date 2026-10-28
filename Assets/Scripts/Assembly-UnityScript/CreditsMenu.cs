using System;
using UnityEngine;

[Serializable]
public class CreditsMenu : BaseMenu
{
	private VGUIScroller scroller;

	private tk2dTextMesh Texts;

	private tk2dTextMesh Texts2;

	private float currentY;

	[NonSerialized]
	public static bool isLoaded;

	public CreditsMenu()
	{
		currentY = 0.18f;
	}

	public override void activate()
	{
		base.activate();
		BaseMenu.mmgr.setStoreButtonVisible(false);
		BaseMenu.mmgr.setTokenCounterVisible(false);
		scroller = (VGUIScroller)GetComponentInChildren(typeof(VGUIScroller));
		loadFile();
	}

	public virtual void loadFile()
	{
		if (isLoaded)
		{
			return;
		}
		isLoaded = true;
		string text = FileManager.getInstance().locateAnyFile("Credits_text.txt");
		bool flag = false;
		string text2 = "0";
		string text3 = "0";
		if (text != null)
		{
			string text4 = FileManager.getInstance().loadFileText(text);
			string[] array = text4.Split("\n"[0]);
			text2 = array[1];
			text3 = array[2];
			int i = 0;
			string[] array2 = array;
			for (int length = array2.Length; i < length; i++)
			{
				if (array2[i] == text2)
				{
					flag = true;
				}
				else if (array2[i] == text3)
				{
					currentY += 0.08f;
					flag = true;
				}
				else if (flag)
				{
					setTitle(array2[i]);
					flag = false;
				}
				else
				{
					setText(array2[i]);
				}
			}
		}
		else
		{
			Debug.LogError("RKN File NOT FOUND");
		}
		scroller.maxLimit.y = currentY * -1f;
	}

	public virtual void setText(string name)
	{
		Texts = (tk2dTextMesh)GameObject.Find("Text").GetComponent(typeof(tk2dTextMesh));
		Texts2 = (tk2dTextMesh)UnityEngine.Object.Instantiate(Texts, Texts.transform.position, Quaternion.identity);
		float x = -0.23f;
		Vector3 position = Texts2.transform.position;
		float num = (position.x = x);
		Vector3 vector = (Texts2.transform.position = position);
		currentY -= 0.1f;
		float y = currentY;
		Vector3 position2 = Texts2.transform.position;
		float num2 = (position2.y = y);
		Vector3 vector3 = (Texts2.transform.position = position2);
		float z = 0f;
		Vector3 position3 = Texts2.transform.position;
		float num3 = (position3.z = z);
		Vector3 vector5 = (Texts2.transform.position = position3);
		Texts2.setText(name);
		Texts2.transform.parent = scroller.transform;
	}

	public virtual void setNextLine(string text, bool isTitel)
	{
		if (isTitel)
		{
			setTitle(text);
		}
		else
		{
			setText(text);
		}
	}

	public virtual void setTitle(string title)
	{
		Texts = (tk2dTextMesh)GameObject.Find("TextT").GetComponent(typeof(tk2dTextMesh));
		Texts2 = (tk2dTextMesh)UnityEngine.Object.Instantiate(Texts, Texts.transform.position, Quaternion.identity);
		float x = -0.23f;
		Vector3 position = Texts2.transform.position;
		float num = (position.x = x);
		Vector3 vector = (Texts2.transform.position = position);
		currentY -= 0.15f;
		float y = currentY;
		Vector3 position2 = Texts2.transform.position;
		float num2 = (position2.y = y);
		Vector3 vector3 = (Texts2.transform.position = position2);
		float z = 0f;
		Vector3 position3 = Texts2.transform.position;
		float num3 = (position3.z = z);
		Vector3 vector5 = (Texts2.transform.position = position3);
		Texts2.setText(title);
		Texts2.transform.parent = scroller.transform;
	}

	public override void onBackButton()
	{
		BaseMenuManager.getInstance().onPreviousMenu();
	}

	public virtual void Main()
	{
	}
}
