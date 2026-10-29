using System.Collections.Generic;
using UnityEngine;

public class LevelPackMenu : BaseMenu
{
	public LevelPackButton levelPackButton;

	public VGUIScrollingContainer packContainer;

	private bool wasActive;

	private List<GameObject> extraPackButtons = new List<GameObject>();

	public override void activate()
	{
		VGUIElement.blockingButtonPressed = false;
		base.activate();
		if (packContainer.pages != null)
		{
			foreach (GameObject page in packContainer.pages)
			{
				UnityEngine.Object.Destroy(page);
			}
			foreach (GameObject gameObject in extraPackButtons)
			{
				UnityEngine.Object.Destroy(gameObject);
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
				Vector3 localPosition = packContainer.pages[k].transform.localPosition;
				localPosition.z = 0f;
				packContainer.pages[k].transform.localPosition = localPosition;
				float x = zero.x;
				Vector3 localPosition2 = packContainer.pages[k].transform.localPosition;
				localPosition2.x = x;
				packContainer.pages[k].transform.localPosition = localPosition2;
				zero.x += packContainer.oneStepLength;
			}
		}
		if (GameData.EXTRA_LEVEL_PACKS)
		{
			GameObject gameObject2 = createLevelPackButton(4, new Vector3(0.5f, -0.35f, 0f));
			gameObject2.transform.parent = transform;
			Vector3 localPosition3 = gameObject2.transform.localPosition;
			localPosition3.x = 0.2695867f;
			gameObject2.transform.localPosition = localPosition3;
			Vector3 localPosition4 = gameObject2.transform.localPosition;
			localPosition4.y = -0.3547182f;
			gameObject2.transform.localPosition = localPosition4;
			Vector3 localPosition5 = gameObject2.transform.localPosition;
			localPosition5.z = 0f;
			gameObject2.transform.localPosition = localPosition5;
			Vector3 localScale = gameObject2.transform.localScale;
			localScale.x = 0.6669993f;
			gameObject2.transform.localScale = localScale;
			Vector3 localScale2 = gameObject2.transform.localScale;
			localScale2.y = 0.6669993f;
			gameObject2.transform.localScale = localScale2;
			LevelPackButton component = gameObject2.GetComponent<LevelPackButton>();
			component.extraPack = true;
			if (GameData.isFeatureUnlocked(2))
			{
				component.setLocked(false);
				Vector3 localPosition6 = component.progress.transform.localPosition;
				localPosition6.x = -0.009937096f;
				component.progress.transform.localPosition = localPosition6;
				Vector3 localPosition7 = component.progress.transform.localPosition;
				localPosition7.y = -0.1313739f;
				component.progress.transform.localPosition = localPosition7;
				component.progress.transform.rotation = Quaternion.Euler(0f, 0f, 346f);
				component.progress.gameObject.SetActiveRecursively(true);
			}
			else
			{
				component.setLocked(true);
				Vector3 localPosition8 = component.@lock.transform.localPosition;
				localPosition8.y = -0.1079767f;
				component.@lock.transform.localPosition = localPosition8;
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
