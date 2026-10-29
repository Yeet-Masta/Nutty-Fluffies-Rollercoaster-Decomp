using UnityEngine;

public class LevelButton : MenuButton
{
	private static float PROGRESS_TEXT_CHANGE_TIME = 2f;

	public VGUIText levelName;

	public GameObject @lock;

	public VGUIText cartCount;

	public CartImage cartImage;

	public GameObject buyGraphic;

	public VGUIText priceText;

	public Texture levelNotBuiltImage;

	public Texture placeHolderImage;

	public GameObject getCoins;

	public GameObject needMoreCoins;

	public GameObject levelImage;

	public GameObject priceData;

	public AudioClip[] pressSounds;

	public VGUIText progress;

	public GameObject namePadge;

	public VGUIText nameText;

	public tk2dTextMesh buildText;

	public tk2dTextMesh[] needCoinsText;

	private string ldboardname;

	private int heartRecord;

	private int heartRecordPercentage;

	private bool localized;

	public GameObject[] previewImages;

	public LevelButton()
	{
		heartRecord = -1;
		heartRecordPercentage = -1;
	}

	public virtual void activate(LevelPack.LevelInfo level, int index, int price)
	{
		int cartType = level.settings.getIntValue("wagontype", 0);
		int num = level.settings.getIntValue("specialcart", Vehicle.CART_NORMAL);
		bool flag = level.isUnlocked();
		bool flag2 = level.testFlag(GameData.LEVEL_FLAG_BUILT);
		bool flag3 = GameData.canBuy(price);
		@lock.GetComponent<Renderer>().enabled = !flag;
		setLocked(!flag);
		GameObject gameObject = buyGraphic;
		bool flag4 = flag;
		if (flag4)
		{
			flag4 = !flag2;
		}
		if (flag4)
		{
			flag4 = flag3;
		}
		gameObject.SetActiveRecursively(flag4);
		cartImage.gameObject.SetActiveRecursively(flag2);
		int activeLevelPackIndex = GameData.getActiveLevelPackIndex();
		GameObject gameObject2 = priceData;
		bool flag5 = flag2;
		if (!flag5)
		{
			flag5 = !flag;
		}
		gameObject2.SetActiveRecursively(!flag5);
		priceText.setText(string.Empty + price);
		namePadge.SetActiveRecursively(false);
		ldboardname = GameData.genLeaderboard(activeLevelPackIndex, index);
		if (GameData.LOG)
		{
			Debug.Log("LEADERBOARD: " + ldboardname);
		}
		if (!localized)
		{
			buildText.setText(LocalizationManager.localize(TextData.BUTTON_LEVEL_BUILD_NOW));
			needCoinsText[0].setText(LocalizationManager.localize(TextData.BUTTON_LEVEL_NEED_COINS1));
			needCoinsText[1].setText(LocalizationManager.localize(TextData.BUTTON_LEVEL_NEED_COINS2));
			localized = true;
		}
		progress.gameObject.SetActiveRecursively(flag2);
		if (flag2)
		{
			pressSound = pressSounds[num + 1];
			cartImage.activate(cartType, num);
			int numberOfCarts = GameData.getNumberOfCarts(GameData.getActiveLevelPack(), index);
			cartCount.setText("X" + numberOfCarts);
			if (numberOfCarts < GameData.CART_LIMIT)
			{
				activateBlinkAnimation();
			}
			heartRecord = GameData.getHeartRecord(index);
			heartRecordPercentage = GameData.getHeartRecordPercentage(index);
			if (heartRecord > -1)
			{
				setProgressAsHearts();
			}
			else
			{
				progress.gameObject.SetActiveRecursively(false);
			}
		}
		string fileName = FileManager.getInstance().locateLevelPack(GameData.getActiveLevelPack().fileName) + "/icons/" + CommonData.getIconPath() + "/" + level.fileName + ".png";
		levelImage.GetComponent<Renderer>().material.mainTexture = TextureManager.getInstance().loadFileWithOutMipmap(fileName);
		getCoins.SetActiveRecursively(false);
		GameObject gameObject3 = needMoreCoins;
		bool flag6 = !flag3;
		if (flag6)
		{
			flag6 = flag;
		}
		if (flag6)
		{
			flag6 = !flag2;
		}
		gameObject3.SetActiveRecursively(flag6);
		if (!flag2)
		{
			if (!flag3 || !flag)
			{
				SpriteUtil.setColor(levelImage.GetComponent<Renderer>(), 0.3f, 0.3f, 0.3f);
			}
			if (flag3 && flag)
			{
				activateBlinkAnimation();
			}
		}
	}

	private void activateBlinkAnimation()
	{
		BlinkAnimator blinkAnimator = (BlinkAnimator)gameObject.GetComponent("BlinkAnimator");
		if (blinkAnimator != null)
		{
			blinkAnimator.activationDelay = UnityEngine.Random.value * 1.5f;
			blinkAnimator.activate();
		}
	}

	public virtual void setProgressAsHearts()
	{
		if (heartRecord > -1)
		{
			progress.setText(string.Empty + heartRecord);
			Invoke("setProgressAsPercentage", PROGRESS_TEXT_CHANGE_TIME);
		}
		else
		{
			progress.gameObject.SetActiveRecursively(false);
		}
	}

	public virtual void setProgressAsPercentage()
	{
		if (heartRecordPercentage > -1)
		{
			progress.setText(string.Empty + heartRecordPercentage + "%");
			Invoke("setProgressAsHearts", PROGRESS_TEXT_CHANGE_TIME);
		}
		else
		{
			progress.gameObject.SetActiveRecursively(false);
		}
	}

	public override void clickButton()
	{
		base.clickButton();
		if (isLocked() && !VGUIElement.blockingButtonPressed)
		{
			string message = TextData.LEVELS_TEXT_LOCKED.Replace("%d", string.Empty + GameData.CARTS_TO_UNLOCK_NEXT_LEVEL);
			MenuManager.getInstance().showErrorMessage(TextData.LEVELS_TITLE_LOCKED, message, MenuManager.getInstance().featureImages[0]);
		}
		OnlineServiceManager.getInstance().loadLeaderboardScores(ldboardname);
	}

	public virtual void createAnimalImages(LevelPack.LevelInfo level, int levelPackIndex, int index)
	{
		GameData.VehicleData vehicleData = GameData.loadTrain(levelPackIndex, index);
		int length = vehicleData.cartList.Length;
		for (int i = 0; i < length; i++)
		{
			int animalIndex = vehicleData.cartList[i].animalIndex;
			if (animalIndex != GameData.ANIMAL_NONE)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(previewImages[animalIndex], Vector3.zero, Quaternion.identity);
				gameObject.transform.parent = this.gameObject.transform;
				float y = 0f - (float)length * 0.5f * 0.05f + 0.05f * (float)i;
				Vector3 localPosition = gameObject.transform.localPosition;
				localPosition.y = y;
				gameObject.transform.localPosition = localPosition;
				float x = -0.18f;
				Vector3 localPosition2 = gameObject.transform.localPosition;
				localPosition2.x = x;
				gameObject.transform.localPosition = localPosition2;
				float z = -0.5f;
				Vector3 localPosition3 = gameObject.transform.localPosition;
				localPosition3.z = z;
				gameObject.transform.localPosition = localPosition3;
				float x2 = 0.7f;
				Vector3 localScale = gameObject.transform.localScale;
				localScale.x = x2;
				gameObject.transform.localScale = localScale;
				float y2 = 0.7f;
				Vector3 localScale2 = gameObject.transform.localScale;
				localScale2.y = y2;
				gameObject.transform.localScale = localScale2;
			}
		}
	}
}
