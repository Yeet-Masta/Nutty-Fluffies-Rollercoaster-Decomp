using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

public class VGUIScrollingContainer : VGUIImageInvoker
{
	public float oneStepLength;

	public int maxSteps;

	public int firstActiveStep;

	public float stepSlideSpeed = 0.5f;

	public float scrollerNearEnoughtBias = 0.01f;

	public VGUIButton scrollLeftButton;

	public VGUIButton scrollRightButton;

	public bool hideOtherThanCurrentPages = true;

	public GameObject[] pages;

	public bool saveCurrentStepToPrefs;

	public string saveCurrentStepPrefsName;

	public bool invokeOnStart = true;

	public Camera cameraRef;

	public float stiffness = 0.1f;

	public float fastSwipeTreshold = 0.02f;

	private int currentStep;

	private int lastStep;

	private float targetPos;

	private bool stopped;

	private bool afterEnable;

	private int startAmount;

	private int pageCount;

	public bool useGestures = true;

	public Vector2 gestureStartAreaTopLeftNormalized = new Vector2(0f, 0f);

	public Vector2 gestureStartAreaBottomRightNormalized = new Vector2(1f, 1f);

	private bool dragging;

	private bool draggedOverMinimumAmount;

	private float dragStartX;

	private float dragAnchorX;

	private float scrollSpeed;

	private Vector2 dragStartPos;

	public override void Awake()
	{
		base.Awake();
		if (cameraRef == null)
		{
			cameraRef = Camera.main;
		}
	}

	public virtual void Start()
	{
		valueType = VGUIImageInvokerValueType.integer;
		bool flag = !invokeOnStart;
		if (!flag)
		{
			flag = startAmount == 0;
		}
		bool silent = flag;
		if (saveCurrentStepToPrefs)
		{
			setCurrentStep(PlayerPrefs.GetInt(saveCurrentStepPrefsName, firstActiveStep), silent);
		}
		else
		{
			setCurrentStep(firstActiveStep, silent);
		}
		immediatePosition();
		afterEnable = true;
		startAmount++;
		int i = 0;
		GameObject[] array = pages;
		for (int length = array.Length; i < length; i++)
		{
			if (array[i] != null)
			{
				pageCount++;
			}
		}
		VGUIElement.blockingButtonPressed = false;
	}

	public virtual void OnEnable()
	{
		Start();
		if (useGestures)
		{
			FingerGestures.OnSwipe += FingerGestureOnSwipe;
			FingerGestures.OnDragBegin += FingerGestureOnDragBegin;
			FingerGestures.OnDragMove += FingerGestureOnDragMove;
			FingerGestures.OnDragEnd += FingerGestureOnDragEnd;
		}
	}

	public virtual void OnDisable()
	{
		if (useGestures)
		{
			FingerGestures.OnSwipe -= FingerGestureOnSwipe;
			FingerGestures.OnDragBegin -= FingerGestureOnDragBegin;
			FingerGestures.OnDragMove -= FingerGestureOnDragMove;
			FingerGestures.OnDragEnd -= FingerGestureOnDragEnd;
		}
	}

	public virtual void immediatePosition()
	{
		float x = targetPos;
		Vector3 position = t.position;
		position.x = x;
		t.position = position;
	}

	public virtual void scrollOneStepLeft()
	{
		if (currentStep > 0)
		{
			setCurrentStep(currentStep - 1, false);
		}
	}

	public virtual void scrollOneStepRight()
	{
		if (currentStep < maxSteps)
		{
			setCurrentStep(currentStep + 1, false);
		}
	}

	public virtual void scrollStepsLeft(int amount)
	{
		if (amount > currentStep)
		{
			amount = currentStep;
		}
		setCurrentStep(currentStep - amount, false);
	}

	public virtual void scrollStepsRight(int amount)
	{
		if (amount + currentStep > maxSteps)
		{
			amount = maxSteps - currentStep;
		}
		setCurrentStep(currentStep + amount, false);
	}

	public virtual void setCurrentStep(int to, bool silent)
	{
		lastStep = currentStep;
		currentStep = to;
		targetPos = (0f - oneStepLength) * (float)currentStep;
		setSideButtonsVisibility();
		if (saveCurrentStepToPrefs)
		{
			PlayerPrefs.SetInt(saveCurrentStepPrefsName, currentStep);
		}
		stopped = false;
		hideNonVisiblePages();
		intValue = currentStep;
		if (!silent)
		{
			activateInvoke(true);
		}
	}

	public virtual int getCurrentStep()
	{
		return currentStep;
	}

	private void setSideButtonsVisibility()
	{
		if (scrollLeftButton != null)
		{
			if (currentStep == 0)
			{
				scrollLeftButton.deactivate();
			}
			else
			{
				scrollLeftButton.activate();
			}
		}
		if (scrollRightButton != null)
		{
			if (currentStep >= maxSteps)
			{
				scrollRightButton.deactivate();
			}
			else
			{
				scrollRightButton.activate();
			}
		}
	}

	private void hideNonVisiblePages()
	{
		if (!hideOtherThanCurrentPages)
		{
			return;
		}
		for (int i = 0; i < Extensions.get_length((System.Array)pages); i++)
		{
			bool flag = i == currentStep;
			if (!flag)
			{
				flag = i == lastStep;
			}
			bool flag2 = flag;
			if (flag2 && !pages[i].active)
			{
				pages[i].SetActiveRecursively(true);
			}
			if (!flag2)
			{
				pages[i].SetActiveRecursively(false);
			}
		}
	}

	private void hideLastPage()
	{
		if (hideOtherThanCurrentPages && lastStep != currentStep)
		{
			pages[lastStep].SetActiveRecursively(false);
		}
	}

	public virtual int getAmountOfVisiblePages()
	{
		int num = 0;
		for (int i = 0; i < Extensions.get_length((System.Array)pages); i++)
		{
			if (pages[i].active)
			{
				num++;
			}
		}
		return num;
	}

	public virtual void setPage(GameObject page, int index)
	{
		pages[index] = page;
		pages[index].transform.parent = transform;
		pages[index].transform.localPosition = new Vector3((float)index * oneStepLength, 0f, 0f);
	}

	public virtual void removePage(GameObject page)
	{
		for (int i = 0; i < pages.Length; i++)
		{
			if (!(page == pages[i]))
			{
				continue;
			}
			UnityEngine.Object.Destroy(pages[i]);
			GameObject[] array = new GameObject[pages.Length - 1];
			int num = 0;
			for (int j = 0; j < pages.Length; j++)
			{
				if (j != i)
				{
					pages[j].transform.localPosition = new Vector3((float)num * oneStepLength, 0f, 0f);
					array[num++] = pages[j];
				}
			}
			pages = array;
			maxSteps = array.Length - 1;
			break;
		}
	}

	public virtual void setPageVisible(GameObject page, bool visible, bool reorder)
	{
		for (int i = 0; i < pages.Length; i++)
		{
			if (page == pages[i])
			{
				if (page.active != visible)
				{
					pages[i].SetActiveRecursively(visible);
				}
				break;
			}
		}
		int num = 0;
		int num2 = 0;
		int j = 0;
		GameObject[] array = pages;
		for (int length = array.Length; j < length; j++)
		{
			if (array[j].active)
			{
				if (reorder)
				{
					pages[num2].transform.localPosition = new Vector3((float)num * oneStepLength, 0f, 0f);
				}
				num++;
			}
			num2++;
		}
		maxSteps = num - 1;
	}

	public virtual void removePages()
	{
		if (!RuntimeServices.EqualityOperator(pages, null))
		{
			int i = 0;
			GameObject[] array = pages;
			for (int length = array.Length; i < length; i++)
			{
				UnityEngine.Object.Destroy(array[i]);
			}
		}
	}

	public override void Update()
	{
		if (afterEnable)
		{
			setSideButtonsVisibility();
			afterEnable = false;
		}
		if (!stopped && !dragging)
		{
			if (!(Mathf.Abs(t.position.x - targetPos) <= scrollerNearEnoughtBias))
			{
				float x = t.position.x;
				float num = stepSlideSpeed * Time.deltaTime;
				float x2 = x * (1f - num) + targetPos * num;
				Vector3 position = t.position;
				position.x = x2;
				t.position = position;
				scrollSpeed *= 1f - stiffness;
			}
			else
			{
				stopped = true;
				hideLastPage();
			}
		}
		if (dragging)
		{
			float x3 = t.localPosition.x;
			float x4 = GetWorldPos(Input.mousePosition).x + dragAnchorX;
			Vector3 position2 = t.position;
			position2.x = x4;
			t.position = position2;
			scrollSpeed += (x3 - t.localPosition.x) * stiffness;
		}
	}

	public virtual void FingerGestureOnSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
		if (fingerIndex == 0)
		{
			float num = Mathf.Abs(dragStartX - transform.position.x);
			if (!(num >= oneStepLength) && !(Mathf.Abs(scrollSpeed) <= fastSwipeTreshold))
			{
				num = oneStepLength;
			}
			float num2 = Mathf.Round(num / oneStepLength);
			switch (direction)
			{
			case FingerGestures.SwipeDirection.Left:
				scrollStepsRight((int)num2);
				break;
			case FingerGestures.SwipeDirection.Right:
				scrollStepsLeft((int)num2);
				break;
			}
		}
	}

	public virtual void FingerGestureOnDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
		if (fingerIndex == 0)
		{
			Vector2 vector = new Vector2(startPos.x / (float)Screen.width, startPos.y / (float)Screen.height);
			if (!(vector.x < gestureStartAreaTopLeftNormalized.x) && !(vector.y < gestureStartAreaTopLeftNormalized.y) && !(vector.x > gestureStartAreaBottomRightNormalized.x) && vector.y <= gestureStartAreaBottomRightNormalized.y)
			{
				Vector3 worldPos = GetWorldPos(fingerPos);
				dragAnchorX = transform.position.x - worldPos.x;
				dragStartX = transform.position.x;
				dragging = true;
				stopped = false;
				dragStartPos = startPos;
				scrollSpeed = 0f;
			}
		}
	}

	public virtual void FingerGestureOnDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
		float magnitude = (fingerPos - dragStartPos).magnitude;
		if (!(magnitude <= (float)Screen.width * 0.07f))
		{
			draggedOverMinimumAmount = true;
			VGUIElement.blockingButtonPressed = true;
		}
	}

	public virtual void FingerGestureOnDragEnd(int fingerIndex, Vector2 fingerPos)
	{
		if (fingerIndex == 0)
		{
			dragging = false;
			VGUIElement.blockingButtonPressed = false;
			draggedOverMinimumAmount = false;
		}
	}

	public virtual Vector3 GetWorldPos(Vector2 screenPos)
	{
		Ray ray = cameraRef.ScreenPointToRay(screenPos);
		float distance = (0f - ray.origin.z) / ray.direction.z;
		return ray.GetPoint(distance);
	}

	public virtual bool isDragging()
	{
		return dragging;
	}

	public virtual bool isDraggingOverMinimumAmount()
	{
		return draggedOverMinimumAmount;
	}
}
