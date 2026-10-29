using UnityEngine;

public class ItemStoreMenu : StoreBaseMenu
{
	public VGUIScrollingContainer packContainer;

	public VGUITextTK2D trainDescription;

	public VGUIRunningValue totalPrice;

	public VGUIRunningValue totalHearts;

	public TrainPreview trainPreview;

	public GameObject[] previewImages;

	public MenuButton plusButton;

	public MenuButton minusButton;

	public GameObject defaultItemImage;

	public Bouncer descriptionBouncer;

	public BlinkAnimator storeButtonBlink;

	public CartImage buyCartImage;

	public ParticleEmitter paperEmitterCart;

	private int purchasedItems;

	private int cartNum;

	private int totalPriceValue;

	private int totalHeartsValue;

	private bool wasActivated;

	private bool IsCartEmpty;

	private bool back;

	private int frameCount;

	public override void activate()
	{
		int indexOfActiveTheme = GameData.getIndexOfActiveTheme();
		string cartKey = null;
		switch (indexOfActiveTheme)
		{
		case 1:
			cartKey = "DP_METAL_CART";
			break;
		case 2:
			cartKey = "DP_LOOP_CART";
			break;
		case 3:
			cartKey = "DP_EXTREME_CART";
			break;
		default:
			cartKey = "DP_WOOD_CART";
			break;
		}
		items[5].itemKey = cartKey;
		items[5].originalPrice = -1;
		base.activate();
		BaseMenuManager.getInstance().tokenAmount.setValueImmediately(GameData.getTokenAmount());
		GameData.createTrainForLevel();
		cartNum = GameData.TRAIN_DATA.cartList.Length;
		totalPriceValue = 0;
		totalHeartsValue = 0;
		purchasedItems = 0;
		trainPreview.prepare(GameData.TRAIN_DATA.specialCart);
		trainPreview.setActiveCarts(cartNum, cartNum);
		int heartIndex = 0;
		foreach (GameObject page in packContainer.pages)
		{
			Transform transform = page.transform.Find("BuyAnimalButton/Multiplier");
			if (transform != null)
			{
				transform.gameObject.GetComponent<VGUITextTK2D>().setText(string.Empty + GameData.ANIMAL_HEART_VALUE[heartIndex] + "X");
				heartIndex++;
			}
		}
		buyCartImage.activate(GameData.getIndexOfActiveTheme(), GameData.TRAIN_DATA.specialCart);
		packContainer.setPageVisible(getItem("ITEM_ANIMAL_CAT").button.gameObject, GameData.isFeatureUnlocked(2), true);
		packContainer.setPageVisible(getItem("ITEM_ANIMAL_DOG").button.gameObject, GameData.isFeatureUnlocked(4), true);
		packContainer.setPageVisible(getItem("ITEM_ANIMAL_PIG").button.gameObject, GameData.isFeatureUnlocked(8), true);
		packContainer.setPageVisible(getItem("ITEM_ANIMAL_ELEPHANT").button.gameObject, GameData.isFeatureUnlocked(16), true);
		packContainer.setPageVisible(getItem(cartKey).button.gameObject, GameData.isFeatureUnlocked(GameData.FEATURE_BUY_CART), true);
		int specialAnimal = GameData.ANIMAL_NONE;
		if (GameData.isFeatureUnlocked(2) && PlayerPrefs.GetInt("HAD_CAT_IN_STORE", 0) == 0)
		{
			PlayerPrefs.SetInt("HAD_CAT_IN_STORE", 1);
			specialAnimal = 1;
		}
		if (GameData.isFeatureUnlocked(4) && PlayerPrefs.GetInt("HAD_DOG_IN_STORE", 0) == 0)
		{
			PlayerPrefs.SetInt("HAD_DOG_IN_STORE", 1);
			specialAnimal = 2;
		}
		if (GameData.isFeatureUnlocked(8) && PlayerPrefs.GetInt("HAD_PIG_IN_STORE", 0) == 0)
		{
			PlayerPrefs.SetInt("HAD_PIG_IN_STORE", 1);
			specialAnimal = 3;
		}
		if (GameData.isFeatureUnlocked(16) && PlayerPrefs.GetInt("HAD_ELEPHANT_STORE", 0) == 0)
		{
			PlayerPrefs.SetInt("HAD_ELEPHANT_STORE", 1);
			specialAnimal = 4;
		}
		for (int j = 0; j < GameData.TRAIN_DATA.cartList.Length; j++)
		{
			int cartAnimal = GameData.TRAIN_DATA.cartList[j].animalIndex;
			if (specialAnimal != GameData.ANIMAL_NONE && j < GameData.TRAIN_DATA.cartList.Length - 1)
			{
				cartAnimal = specialAnimal;
			}
			if (cartAnimal != GameData.ANIMAL_NONE)
			{
				purchasedItems++;
				addAnimalToCart(j, cartAnimal, false);
				totalHeartsValue += GameData.ANIMAL_HEART_VALUE[cartAnimal];
			}
		}
		totalPrice.setValueImmediately(0);
		totalHearts.setValueImmediately(totalHeartsValue);
		packContainer.invokeObject = gameObject;
		packContainer.invokeMethod = "updateItemDescription";
		updateTrainDescription();
		updateItemDescription();
		int startStep = 0;
		int indexOfLastUnlockedButton = getIndexOfLastUnlockedButton();
		switch (indexOfLastUnlockedButton)
		{
		case 1:
			startStep = 0;
			break;
		case 2:
			startStep = 1;
			break;
		case 3:
			startStep = 1;
			break;
		default:
			startStep = Mathf.Max(1, indexOfLastUnlockedButton - 2);
			break;
		}
		packContainer.setCurrentStep(startStep, false);
		packContainer.immediatePosition();
		if (RollerCoasterStatistics.getInstance().roundEarnedNewCart && GameManager.getInstance().restarts > 0)
		{
			if (paperEmitterCart != null)
			{
				paperEmitterCart.emit = true;
			}
			Invoke("stopCartEmit", 1f);
			RollerCoasterSounds.getInstance().playFireworks();
		}
		wasActivated = true;
	}

	public virtual void stopCartEmit()
	{
		if (paperEmitterCart != null)
		{
			paperEmitterCart.emit = false;
		}
	}

	public override void reactivate()
	{
		base.reactivate();
		trainPreview.setActiveCarts(cartNum, cartNum);
		updateTrainDescription();
		wasActivated = true;
	}

	public override void deactivate()
	{
		base.deactivate();
		GameData.saveTrain();
		if (storeButtonBlink != null)
		{
			storeButtonBlink.deactivate();
		}
		if (paperEmitterCart != null)
		{
			paperEmitterCart.emit = false;
			paperEmitterCart.ClearParticles();
		}
	}

	private void updateItemDescription()
	{
		int currentStep = packContainer.getCurrentStep();
		int offset = 0;
		for (int i = 0; i <= currentStep; i++)
		{
			if (!items[i].button.gameObject.active)
			{
				offset++;
			}
		}
		updateItemDescription(currentStep + offset);
		descriptionBouncer.bounce();
	}

	private void updateItemDescription(int index)
	{
		GameData.getNewFeaturesList();
		if (!GameData.isFeatureUnlocked(16) && GameData.isFeatureUnlocked(GameData.FEATURE_BUY_CART) && index == 4)
		{
			trainDescription.setText(TextData.ANIMAL_DESCRIPTIONS[5]);
		}
		else if (index >= 0 && index < TextData.ANIMAL_DESCRIPTIONS.Length)
		{
			trainDescription.setText(TextData.ANIMAL_DESCRIPTIONS[index]);
		}
	}

	private void updateTrainDescription()
	{
		setPurchaseButtonsEnabled(purchasedItems < cartNum);
		for (int i = 0; i < items.Length; i++)
		{
			MenuButton purchaseButton = items[i].button.purchaseButton;
			bool itemDimmed = !GameData.canBuy(items[i].price) || purchasedItems >= cartNum;
			purchaseButton.setDimmed(itemDimmed);
		}
		if (storeButtonBlink != null)
		{
			if (!GameData.canBuy(items[1].price))
			{
				storeButtonBlink.activate();
			}
			else
			{
				storeButtonBlink.deactivate();
			}
		}
		MenuButton purchaseButton2 = items[5].button.purchaseButton;
		bool cartLocked = cartNum >= GameData.CART_LIMIT || !GameData.canBuy(items[5].price);
		purchaseButton2.setLocked(cartLocked);
		MenuButton purchaseButton3 = items[5].button.purchaseButton;
		bool cartDimmed = cartNum >= GameData.CART_LIMIT || !GameData.canBuy(items[5].price);
		purchaseButton3.setDimmed(cartDimmed);
		GameObject cart = trainPreview.getCart(cartNum - 1);
		if (paperEmitterCart != null)
		{
			paperEmitterCart.transform.position = cart.transform.position;
			float z = paperEmitterCart.transform.position.z + 0.1f;
			Vector3 position = paperEmitterCart.transform.position;
			position.z = z;
			paperEmitterCart.transform.position = position;
		}
	}

	private int getIndexOfLastUnlockedButton()
	{
		int index = 0;
		int hidden = 0;
		for (int i = 0; i < items.Length; i++)
		{
			if (!items[i].button.purchaseButton.isDimmed())
			{
				index = i;
			}
			if (!items[i].button.gameObject.active)
			{
				hidden++;
			}
		}
		index -= hidden;
		return index + 1;
	}

	private void addAnimalToCart(int cartIndex, int animal, bool payback)
	{
		if (animal > GameData.ANIMAL_NONE)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(previewImages[animal], Vector3.zero, Quaternion.identity);
			object[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(RemoveItemButton), true);
			RemoveItemButton removeItemButton = componentsInChildren[0] as RemoveItemButton;
			removeItemButton.invokeObject = this.gameObject;
			if (!payback)
			{
				removeItemButton.invokeMethod = "onRemoveItem";
			}
			else
			{
				removeItemButton.invokeMethod = "onRemoveItemAndPayBack";
			}
			GameData.TRAIN_DATA.cartList[cartIndex].animalIndex = animal;
			trainPreview.addItem(cartIndex, gameObject);
			removeItemButton.intValue = cartIndex;
			removeItemButton.setMode(payback);
			BaseMenu.anim.scale(gameObject.transform, 0f, 1f);
		}
	}

	public virtual void onPurchaseItem(int index)
	{
		if (!wasActivated)
		{
			return;
		}
		if (purchasedItems < cartNum)
		{
			if (GameData.canBuy(items[index].price))
			{
				totalPriceValue += items[index].price;
				totalHeartsValue += GameData.ANIMAL_HEART_VALUE[index];
				if (items[index].price > 0)
				{
					GameData.addTokens(-items[index].price);
				}
				BaseMenuManager.getInstance().updateTokenAmount();
				for (int i = 0; i < GameData.TRAIN_DATA.cartList.Length; i++)
				{
					if (GameData.TRAIN_DATA.cartList[i].animalIndex == GameData.ANIMAL_NONE)
					{
						addAnimalToCart(i, index, true);
						break;
					}
				}
				purchasedItems++;
			}
		}
		updateTrainDescription();
		updateItemDescription(index);
	}

	public virtual void onPurchaseCart()
	{
		if (!wasActivated)
		{
			return;
		}
		int price = items[5].price;
		LevelPack activeLevelPack = GameData.getActiveLevelPack();
		if (GameData.canBuy(price))
		{
			if (cartNum < GameData.CART_LIMIT)
			{
				GameData.addTokens(-price);
				cartNum++;
				trainPreview.setActiveCarts(cartNum, cartNum);
				GameData.setNumberOfCarts(activeLevelPack, GameData.SELECTED_LEVEL_INDEX, cartNum);
				BaseMenu.mmgr.updateTokenAmount();
				Transform transform = trainPreview.getCart(cartNum - 1).transform;
				BaseMenu.anim.scale(transform, 0f, transform.localScale.x);
				GameData.saveTrain();
				GameData.createTrainForLevel();
				updateTrainDescription();
			}
		}
	}

	public virtual void onRemoveItem(int index)
	{
		if (wasActivated)
		{
			trainPreview.removeItem(index);
			totalHeartsValue -= GameData.ANIMAL_HEART_VALUE[GameData.TRAIN_DATA.cartList[index].animalIndex];
			GameData.TRAIN_DATA.cartList[index].animalIndex = -1;
			purchasedItems--;
			updateTrainDescription();
		}
	}

	public virtual void onRemoveItemAndPayBack(int index)
	{
		if (wasActivated)
		{
			trainPreview.removeItem(index);
			int animalIndex = GameData.TRAIN_DATA.cartList[index].animalIndex;
			totalHeartsValue -= GameData.ANIMAL_HEART_VALUE[animalIndex];
			GameData.addTokens(items[animalIndex].price);
			GameData.TRAIN_DATA.cartList[index].animalIndex = -1;
			purchasedItems--;
			updateTrainDescription();
			BaseMenu.mmgr.updateTokenAmount();
		}
	}

	public virtual void onContinue()
	{
		int num = 0;
		int num2 = 0;
		foreach (GameData.CartData cartData in GameData.TRAIN_DATA.cartList)
		{
			if (cartData.animalIndex != GameData.ANIMAL_NONE)
			{
				num++;
			}
			num2++;
		}
		if (num != num2)
		{
			IsCartEmpty = true;
			BaseMenu.mmgr.showErrorMessage(TextData.STORE_TITLE_EMPTY_CART, TextData.STORE_TEXT_EMPTY_CART, defaultItemImage);
			return;
		}
		IsCartEmpty = false;
		GameData.saveTrain();
		GameMenuManager.getInstance().startGame();
		wasActivated = false;
		BaseMenuManager.getInstance().setStoreButtonVisible(false);
	}

	public virtual void onMenu()
	{
		if (frameCount >= 10)
		{
			if (GameData.LOG)
			{
				Debug.Log(" ItemStoreMenu  ");
			}
			GameData.saveTrain();
			if (!IsCartEmpty)
			{
				GameManager.getInstance().endGame();
				return;
			}
			IsCartEmpty = false;
			GameManager.getInstance().restart();
		}
	}

	public override void Update()
	{
		frameCount++;
		if (frameCount == 11)
		{
			frameCount = 10;
		}
		if (CustomInput.getKeyDown(27) && back)
		{
			frameCount = 10;
			onMenu();
		}
		back = true;
	}
}
