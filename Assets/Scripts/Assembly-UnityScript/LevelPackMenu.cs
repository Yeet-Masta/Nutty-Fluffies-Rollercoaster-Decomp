using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class LevelPackMenu : BaseMenu
{
	public LevelPackButton levelPackButton;

	public VGUIScrollingContainer packContainer;

	private bool wasActive;

	private List<GameObject> extraPackButtons;

	public LevelPackMenu()
	{
		extraPackButtons = new List<GameObject>();
	}

	public override void activate()
	{
		VGUIElement.blockingButtonPressed = false;
		base.activate();
		if (!RuntimeServices.EqualityOperator(packContainer.pages, null))
		{
			int i = 0;
			GameObject[] pages = packContainer.pages;
			for (int length = pages.Length; i < length; i++)
			{
				UnityEngine.Object.Destroy(pages[i]);
			}
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(extraPackButtons);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is GameObject))
				{
					obj = RuntimeServices.Coerce(obj, typeof(GameObject));
				}
				GameObject gameObject = (GameObject)obj;
				UnityEngine.Object.Destroy(gameObject);
				UnityRuntimeServices.Update(enumerator, gameObject);
			}
			extraPackButtons = new List<GameObject>();
		}
		int num = 0;
		for (int j = 0; j < GameData.getLevelPackCount(); j++)
		{
			if (!GameData.getLevelPack(j).isExternal())
			{
				num++;
			}
		}
		Vector3 zero = Vector3.zero;
		packContainer.pages = new GameObject[num];
		packContainer.maxSteps = num - 1;
		for (int k = 0; k < num; k++)
		{
			if (!GameData.getLevelPack(k).isExternal())
			{
				packContainer.pages[k] = createLevelPackButton(k, zero);
				packContainer.pages[k].transform.parent = packContainer.transform;
				float z = 0f;
				Vector3 localPosition = packContainer.pages[k].transform.localPosition;
				float num2 = (localPosition.z = z);
				Vector3 vector = (packContainer.pages[k].transform.localPosition = localPosition);
				float x = zero.x;
				Vector3 localPosition2 = packContainer.pages[k].transform.localPosition;
				float num3 = (localPosition2.x = x);
				Vector3 vector3 = (packContainer.pages[k].transform.localPosition = localPosition2);
				zero.x += packContainer.oneStepLength;
			}
		}
		if (GameData.EXTRA_LEVEL_PACKS)
		{
			GameObject gameObject2 = createLevelPackButton(4, new Vector3(0.5f, -0.35f, 0f));
			gameObject2.transform.parent = transform;
			float x2 = 0.2695867f;
			Vector3 localPosition3 = gameObject2.transform.localPosition;
			float num4 = (localPosition3.x = x2);
			Vector3 vector5 = (gameObject2.transform.localPosition = localPosition3);
			float y = -0.3547182f;
			Vector3 localPosition4 = gameObject2.transform.localPosition;
			float num5 = (localPosition4.y = y);
			Vector3 vector7 = (gameObject2.transform.localPosition = localPosition4);
			float z2 = 0f;
			Vector3 localPosition5 = gameObject2.transform.localPosition;
			float num6 = (localPosition5.z = z2);
			Vector3 vector9 = (gameObject2.transform.localPosition = localPosition5);
			float x3 = 0.6669993f;
			Vector3 localScale = gameObject2.transform.localScale;
			float num7 = (localScale.x = x3);
			Vector3 vector11 = (gameObject2.transform.localScale = localScale);
			float y2 = 0.6669993f;
			Vector3 localScale2 = gameObject2.transform.localScale;
			float num8 = (localScale2.y = y2);
			Vector3 vector13 = (gameObject2.transform.localScale = localScale2);
			LevelPackButton component = gameObject2.GetComponent<LevelPackButton>();
			component.extraPack = true;
			if (GameData.isFeatureUnlocked(2))
			{
				component.setLocked(false);
				float x4 = -0.009937096f;
				Vector3 localPosition6 = component.progress.transform.localPosition;
				float num9 = (localPosition6.x = x4);
				Vector3 vector15 = (component.progress.transform.localPosition = localPosition6);
				float y3 = -0.1313739f;
				Vector3 localPosition7 = component.progress.transform.localPosition;
				float num10 = (localPosition7.y = y3);
				Vector3 vector17 = (component.progress.transform.localPosition = localPosition7);
				component.progress.transform.rotation = Quaternion.Euler(0f, 0f, 346f);
				component.progress.gameObject.SetActiveRecursively(true);
			}
			else
			{
				component.setLocked(true);
				float y4 = -0.1079767f;
				Vector3 localPosition8 = component.@lock.transform.localPosition;
				float num11 = (localPosition8.y = y4);
				Vector3 vector19 = (component.@lock.transform.localPosition = localPosition8);
			}
			component.levelName.gameObject.SetActiveRecursively(false);
			extraPackButtons.Add(gameObject2);
			GameData.getLevelPack(4).flags = GameData.getLevelPack(4).flags | 1;
		}
		if (GameData.getLevelPack(1).isUnlocked())
		{
			GameData.unlockFeature(1024);
		}
		if (GameData.getLevelPack(2).isUnlocked())
		{
			GameData.unlockFeature(2048);
		}
		if (GameData.getLevelPack(3).isUnlocked())
		{
			GameData.unlockFeature(4096);
		}
		packContainer.pages[0].GetComponent<LevelPackButton>().setLocked(false);
		packContainer.pages[1].GetComponent<LevelPackButton>().setLocked(!GameData.isFeatureUnlocked(1024));
		packContainer.pages[2].GetComponent<LevelPackButton>().setLocked(!GameData.isFeatureUnlocked(2048));
		packContainer.pages[3].GetComponent<LevelPackButton>().setLocked(!GameData.isFeatureUnlocked(4096));
		packContainer.Start();
		int num12 = PlayerPrefs.GetInt("LEVEL_PACK_SELECTION", 0);
		if (num12 < 0 || num12 >= num)
		{
			num12 = 0;
		}
		packContainer.firstActiveStep = num12;
		packContainer.setCurrentStep(num12, true);
		packContainer.immediatePosition();
		wasActive = true;
	}

	public override void deactivate()
	{
		base.deactivate();
		if (wasActive)
		{
			PlayerPrefs.SetInt("LEVEL_PACK_SELECTION", packContainer.intValue);
			PlayerPrefs.Save();
			wasActive = false;
		}
	}

	public override void onBackButton()
	{
		BaseMenuManager.getInstance().onPreviousMenu();
	}

	public virtual void onSelectLevelPack(int packIndex)
	{
		GameData.setActiveLevelPack(packIndex);
		if (GameData.getActiveLevelPack().isUnlocked())
		{
			StoreBaseMenu.COMMON_PRICE_FACTOR = 1f;
			MenuManager.getInstance().onNextMenu();
		}
	}

	public virtual GameObject createLevelPackButton(int levelIndex, Vector3 position)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(levelPackButton.gameObject, position, Quaternion.identity);
		LevelPackButton component = gameObject.GetComponent<LevelPackButton>();
		bool activeRecursively = GameData.getLevelPack(levelIndex).isUnlocked();
		string text = FileManager.getInstance().locateLevelPack(GameData.getLevelPack(levelIndex).fileName) + "/icons/" + CommonData.getIconPath() + "/pack-icon.png";
		Renderer renderer = component.graphic.GetComponent<Renderer>();
		renderer.material.mainTexture = TextureManager.getInstance().loadFileWithOutMipmap(text);
		if (renderer.material.mainTexture == null)
		{
			Debug.LogWarning("Texture " + text + " not found");
		}
		component.progress.setText(string.Empty + GameData.getLevelPackProgress(levelIndex) + "%");
		component.progress.gameObject.SetActiveRecursively(activeRecursively);
		component.invokeObject = this.gameObject;
		component.invokeMethod = "onSelectLevelPack";
		component.intValue = levelIndex;
		string text2 = LocalizationManager.localize(GameData.getLevelPack(levelIndex).fileName.ToUpper());
		component.levelName.setText(text2);
		return gameObject;
	}
}
