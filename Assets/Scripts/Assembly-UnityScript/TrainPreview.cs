using Boo.Lang.Runtime;
using UnityEngine;

public class TrainPreview : MonoBehaviour
{
	public float cartSpacing = 0.15f;

	public float maxWidth = 1f;

	public CartImage cartImage;

	public Transform trainOverviewRoot;

	public float disabledAlpha = 0.3f;

	public float imageScaler = 1f;

	private CartImage[] activeCart;

	private GameObject[] attachedItems;

	private float originalScale;

	private float originalCartScale = 1f;

	private int specialCartType;

	public virtual void prepare(int cartType)
	{
		useGUILayout = false;
		specialCartType = cartType;
		if (originalScale == 0f)
		{
			originalScale = this.transform.localScale.x;
		}
		if (activeCart != null)
		{
			for (int i = 0; i < activeCart.Length; i++)
			{
				UnityEngine.Object.Destroy(activeCart[i].gameObject);
			}
		}
		if (attachedItems != null)
		{
			for (int j = 0; j < attachedItems.Length; j++)
			{
				if (attachedItems[j] != null)
				{
					UnityEngine.Object.Destroy(attachedItems[j]);
				}
				attachedItems[j] = null;
			}
		}
		activeCart = new CartImage[GameData.CART_LIMIT];
		attachedItems = new GameObject[GameData.CART_LIMIT];
		for (int k = 0; k < GameData.CART_LIMIT; k++)
		{
			Vector3 localPosition = new Vector3(0f + cartSpacing * (float)k, 0f, 0f);
			CartImage cartImage = ((CartImage)UnityEngine.Object.Instantiate(this.cartImage, Vector3.zero, Quaternion.identity)) as CartImage;
			Transform transform = cartImage.transform;
			Vector3 localScale = transform.localScale;
			transform.parent = trainOverviewRoot;
			transform.localPosition = localPosition;
			transform.localScale = localScale;
			activeCart[k] = cartImage;
			cartImage.gameObject.SetActiveRecursively(false);
			cartImage.activate(GameData.getIndexOfActiveTheme(), -1);
			makeCartBlink(k);
		}
		originalCartScale = this.cartImage.transform.localScale.x * imageScaler;
	}

	public virtual void setActiveCarts(int activeCarts, int totalCarts)
	{
		float num = (float)(totalCarts - 1) * cartSpacing;
		float num2 = originalScale;
		if (!(num * originalScale <= maxWidth))
		{
			num2 = maxWidth / num;
		}
		float x = num2;
		Vector3 localScale = transform.localScale;
		localScale.x = x;
		transform.localScale = localScale;
		float y = num2;
		Vector3 localScale2 = transform.localScale;
		localScale2.y = y;
		transform.localScale = localScale2;
		float x2 = (0f - num) * 0.5f * transform.localScale.x;
		Vector3 position = trainOverviewRoot.position;
		position.x = x2;
		trainOverviewRoot.position = position;
		for (int i = 0; i < totalCarts; i++)
		{
			if (i < activeCarts)
			{
				activeCart[i].setAlpha(1f);
				activeCart[i].setAsUsed(true);
			}
			else
			{
				activeCart[i].setAlpha(disabledAlpha);
				activeCart[i].setAsUsed(false);
			}
			activeCart[i].gameObject.SetActiveRecursively(true);
			int cART_NORMAL = specialCartType;
			if (specialCartType == 0 && i != 0)
			{
				cART_NORMAL = Vehicle.CART_NORMAL;
			}
			activeCart[i].activate(GameData.getIndexOfActiveTheme(), cART_NORMAL);
			activeCart[i].transform.localScale = new Vector3(originalCartScale, originalCartScale, 1f);
		}
		for (int j = totalCarts; j < GameData.CART_LIMIT; j++)
		{
			activeCart[j].gameObject.SetActiveRecursively(false);
		}
	}

	public virtual void addItem(int index, GameObject item)
	{
		item.transform.parent = activeCart[index].transform;
		item.transform.localPosition = new Vector3(0f, 0.05f, -0.5f);
		attachedItems[index] = item;
		stopCartBlink(index);
	}

	public virtual void removeItem(int index)
	{
		UnityEngine.Object.Destroy(attachedItems[index]);
		attachedItems[index] = null;
		makeCartBlink(index);
	}

	public virtual void makeCartBlink(int index)
	{
		GameObject cart = getCart(index);
		BlinkAnimator blinkAnimator = (BlinkAnimator)cart.GetComponent(typeof(BlinkAnimator));
		if (blinkAnimator != null)
		{
			blinkAnimator.deactivate();
			UnityEngine.Object.Destroy(blinkAnimator);
		}
		blinkAnimator = (BlinkAnimator)cart.AddComponent(typeof(BlinkAnimator));
		blinkAnimator.target = cart.transform;
		blinkAnimator.blinkRate = 0.5f;
		blinkAnimator.scaleFactor = 1.25f;
		blinkAnimator.duration = 0.35f;
		blinkAnimator.interpolator = VGUIAnimator.Interpolator.OVERSHOOT;
		blinkAnimator.activate();
	}

	public virtual void stopCartBlink(int index)
	{
		GameObject cart = getCart(index);
		BlinkAnimator blinkAnimator = (BlinkAnimator)cart.GetComponent(typeof(BlinkAnimator));
		if (blinkAnimator != null)
		{
			blinkAnimator.deactivate();
			UnityEngine.Object.Destroy(blinkAnimator);
		}
		cart.transform.localScale = Vector3.one;
	}

	public virtual GameObject getCart(int index)
	{
		return activeCart[index].gameObject;
	}

	public virtual GameObject getLastCart()
	{
		return activeCart[activeCart.Length - 1].gameObject;
	}
}
