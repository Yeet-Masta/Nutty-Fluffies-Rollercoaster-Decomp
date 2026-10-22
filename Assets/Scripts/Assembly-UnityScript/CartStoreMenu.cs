using System;
using UnityEngine;

[Serializable]
public class CartStoreMenu : StoreBaseMenu
{
	private int currentTheme;

	private int purchasedCarts;

	private int activeCarts;

	private int cartPrice;

	public VGUITextTK2D trainDescription;

	public VGUITextTK2D infoText;

	public MenuButton minusButton;

	public MenuButton plusButton;

	public TrainPreview trainPreview;

	public CartImage cartImage;

	public override void activate()
	{
		LevelPack activeLevelPack = GameData.getActiveLevelPack();
		base.activate();
		currentTheme = GameData.getIndexOfActiveTheme();
		purchasedCarts = GameData.getNumberOfCarts(activeLevelPack, GameData.SELECTED_LEVEL_INDEX);
		trainPreview.prepare(GameData.TRAIN_DATA.specialCart);
		cartPrice = items[currentTheme].price;
		items[currentTheme].updatePriceText();
		updateTrainDescription();
		cartImage.activate(currentTheme, GameData.TRAIN_DATA.specialCart);
	}

	public override void reactivate()
	{
		base.reactivate();
		updateTrainDescription();
		cartImage.activate(currentTheme, GameData.TRAIN_DATA.specialCart);
	}

	private void updateTrainDescription()
	{
		if (activeCarts == 1)
		{
			trainDescription.setText("This ride runs now with " + activeCarts + " cart");
		}
		else
		{
			trainDescription.setText("This ride runs now with " + activeCarts + " carts");
		}
		trainPreview.setActiveCarts(activeCarts, purchasedCarts);
		if (activeCarts < GameData.CARTS_TO_UNLOCK_NEXT_LEVEL)
		{
			infoText.setText("You need at least\n" + GameData.CARTS_TO_UNLOCK_NEXT_LEVEL + " carts to unlock\nnext track.");
		}
		else if (activeCarts < GameData.CART_LIMIT)
		{
			infoText.setText("Get and drive with\nmore carts to earn\nmore coins.");
		}
		else
		{
			infoText.setText("You have maximum\namount of carts,\nthats awesome!!");
		}
		float num = trainPreview.transform.localScale.x * trainPreview.cartSpacing + 0.005f;
		float num2 = (float)(purchasedCarts - 1) * num;
		float x = (0f - num2) * 0.5f - num;
		Vector3 position = minusButton.transform.position;
		float num3 = (position.x = x);
		Vector3 vector = (minusButton.transform.position = position);
		float y = trainPreview.transform.position.y;
		Vector3 position2 = minusButton.transform.position;
		float num4 = (position2.y = y);
		Vector3 vector3 = (minusButton.transform.position = position2);
		float x2 = num2 * 0.5f + num;
		Vector3 position3 = plusButton.transform.position;
		float num5 = (position3.x = x2);
		Vector3 vector5 = (plusButton.transform.position = position3);
		float y2 = trainPreview.transform.position.y;
		Vector3 position4 = plusButton.transform.position;
		float num6 = (position4.y = y2);
		Vector3 vector7 = (plusButton.transform.position = position4);
		MenuButton menuButton = plusButton;
		bool flag = activeCarts >= GameData.CART_LIMIT;
		if (!flag)
		{
			flag = activeCarts >= purchasedCarts;
			if (flag)
			{
				flag = !GameData.canBuy(cartPrice);
			}
		}
		menuButton.setLocked(flag);
		minusButton.setLocked(activeCarts <= GameData.MIN_CART_COUNT);
		bool flag2 = purchasedCarts < GameData.CART_LIMIT;
		if (flag2)
		{
			flag2 = GameData.canBuy(cartPrice);
		}
		setPurchaseButtonsEnabled(flag2);
	}

	public virtual void onBuyCart()
	{
		MonoBehaviour.print("buying cart");
		if (GameData.canBuy(cartPrice))
		{
			if (purchasedCarts < GameData.CART_LIMIT)
			{
				GameData.addTokens(-cartPrice);
				purchasedCarts++;
				GameData.setNumberOfCarts(GameData.getActiveLevelPack(), GameData.SELECTED_LEVEL_INDEX, purchasedCarts);
				onAddCart();
				BaseMenu.mmgr.updateTokenAmount();
				Transform transform = trainPreview.getCart(purchasedCarts - 1).transform;
				BaseMenu.anim.scale(transform, 0f, transform.localScale.x);
			}
			else
			{
				MonoBehaviour.print("too many carts");
			}
		}
		else
		{
			MonoBehaviour.print("need more tokens..");
		}
	}

	public virtual void onRemoveCart()
	{
		if (activeCarts > GameData.MIN_CART_COUNT)
		{
			activeCarts--;
			Transform target = trainPreview.getCart(activeCarts).transform;
			BaseMenu.anim.alpha(target, 1f, trainPreview.disabledAlpha);
		}
		updateTrainDescription();
	}

	public virtual void onAddCart()
	{
		if (activeCarts == purchasedCarts)
		{
			onBuyCart();
		}
		if (activeCarts < purchasedCarts)
		{
			Transform target = trainPreview.getCart(activeCarts).transform;
			BaseMenu.anim.alpha(target, trainPreview.disabledAlpha, 1f);
			activeCarts++;
		}
		updateTrainDescription();
	}

	public virtual void onContinue()
	{
		GameData.TRAIN_DATA.cartList = new GameData.CartData[activeCarts];
		for (int i = 0; i < activeCarts; i++)
		{
			GameData.TRAIN_DATA.cartList[i] = new GameData.CartData();
			GameData.TRAIN_DATA.cartList[i].animalIndex = GameData.ANIMAL_NONE;
		}
		GameData.TRAIN_DATA.trainType = GameData.getIndexOfActiveTheme();
		GameData.TRAIN_DATA.specialCart = GameData.getSelectedLevel().settings.getIntValue("specialcart", Vehicle.CART_NORMAL);
		MenuManager.getInstance().onNextMenu();
	}
}
