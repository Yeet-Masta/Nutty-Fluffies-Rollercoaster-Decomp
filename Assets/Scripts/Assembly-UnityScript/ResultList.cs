using System;
using UnityEngine;

public class ResultList : VGUIList
{
	[Serializable]
	public class BonusEntry
	{
		public int type;

		public int value;

		public int multiplier;
	}

	public BonusItem[] bonusItems;

	public VGUIRunningValue tokenSum;

	public VGUIRunningValue heartSum;

	public VGUIAnimator listEnryAnimator;

	public VGUIAnimator listScrollAnimator;

	public Transform resultLine;

	public int maxEntries;

	private float thisY;

	private bool firstTime;

	public ResultList()
	{
		firstTime = true;
	}

	public override void activate()
	{
		clearList();
		if (!firstTime)
		{
			tokenSum.setValueImmediately(0);
			heartSum.setValueImmediately(0);
			tokenSum.setText("0");
			heartSum.setText("0");
		}
		else
		{
			firstTime = false;
		}
		(listScrollAnimator as SlideInAnimator).from = new Vector3(0f, 0f - verticalSpacing, 0f);
		if (thisY == 0f)
		{
			thisY = transform.localPosition.y;
			return;
		}
		float y = thisY;
		Vector3 localPosition = transform.localPosition;
		localPosition.y = y;
		transform.localPosition = localPosition;
	}

	public override void deactivate()
	{
		clearList();
		listScrollAnimator.deactivate();
		listEnryAnimator.deactivate();
		CancelInvoke();
	}

	public virtual void instatiateBonus(BonusEntry bonus, bool inlined)
	{
		BonusItem component = createEntry(bonusItems[bonus.type].gameObject).GetComponent<BonusItem>();
		component.setMultiplier(bonus.multiplier);
		component.setHeartAmount(bonus.value);
		if (inlined)
		{
			float x = component.transform.position.x + 0.025f;
			Vector3 position = component.transform.position;
			position.x = x;
			component.transform.position = position;
		}
		Component[] componentsInChildren = component.gameObject.GetComponentsInChildren(typeof(CartImage));
		int i = 0;
		Component[] array = componentsInChildren;
		for (int length = array.Length; i < length; i++)
		{
			int num2 = GameManager.getInstance().getSpecialCartType();
			if (num2 == 0 && ((CartImage)array[i]).gameObject.name != "CartImageFirst")
			{
				num2 = -1;
			}
			((CartImage)array[i]).activate(GameManager.getInstance().getTrainType(), num2);
		}
		listEnryAnimator.target = component.transform;
		listEnryAnimator.activate();
		if (entryRefs.Count > maxEntries)
		{
			entryRefs[entryRefs.Count - maxEntries - 1].SetActiveRecursively(false);
			float y = transform.localPosition.y + verticalSpacing;
			Vector3 localPosition = transform.localPosition;
			localPosition.y = y;
			transform.localPosition = localPosition;
			listScrollAnimator.activate();
		}
	}
}
