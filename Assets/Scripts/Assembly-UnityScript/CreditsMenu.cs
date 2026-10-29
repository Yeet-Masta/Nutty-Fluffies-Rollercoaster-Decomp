using UnityEngine;

public class CreditsMenu : BaseMenu
{
	private VGUIScroller scroller;

	private tk2dTextMesh Texts;

	private tk2dTextMesh Texts2;

	private float currentY = 0.18f;

	public static bool isLoaded;

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
		string path = FileManager.getInstance().locateAnyFile("Credits_text.txt");
		bool flag = false;
		string titleMarker = "0";
		string textMarker = "0";
		if (path != null)
		{
			string content = FileManager.getInstance().loadFileText(path);
			string[] lines = content.Split("\n"[0]);
			titleMarker = lines[1];
			textMarker = lines[2];
			int i = 0;
			string[] array2 = lines;
			for (int length = array2.Length; i < length; i++)
			{
				if (array2[i] == titleMarker)
				{
					flag = true;
				}
				else if (array2[i] == textMarker)
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
		Texts2 = UnityEngine.Object.Instantiate(Texts, Texts.transform.position, Quaternion.identity);
		Vector3 position = Texts2.transform.position;
		position.x = -0.23f;
		Texts2.transform.position = position;
		currentY -= 0.1f;
		float y = currentY;
		Vector3 position2 = Texts2.transform.position;
		position2.y = y;
		Texts2.transform.position = position2;
		Vector3 position3 = Texts2.transform.position;
		position3.z = 0f;
		Texts2.transform.position = position3;
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
		Texts2 = UnityEngine.Object.Instantiate(Texts, Texts.transform.position, Quaternion.identity);
		Vector3 position = Texts2.transform.position;
		position.x = -0.23f;
		Texts2.transform.position = position;
		currentY -= 0.15f;
		float y = currentY;
		Vector3 position2 = Texts2.transform.position;
		position2.y = y;
		Texts2.transform.position = position2;
		Vector3 position3 = Texts2.transform.position;
		position3.z = 0f;
		Texts2.transform.position = position3;
		Texts2.setText(title);
		Texts2.transform.parent = scroller.transform;
	}

	public override void onBackButton()
	{
		BaseMenuManager.getInstance().onPreviousMenu();
	}
}
