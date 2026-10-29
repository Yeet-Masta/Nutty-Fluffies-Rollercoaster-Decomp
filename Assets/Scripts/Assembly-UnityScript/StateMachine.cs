using UnityEngine;

public class StateMachine : MonoBehaviour
{
	protected State currentState;

	protected int currentStateUpdates;

	protected State delayedNextState;

	public bool automaticallyAddChildStates;

	public virtual void Awake()
	{
		foreach (Transform transform in this.transform)
		{
			transform.gameObject.SetActiveRecursively(false);
		}
		if (automaticallyAddChildStates)
		{
			addChildStates();
		}
	}

	public virtual void Update()
	{
		if (currentState != null)
		{
			if (currentStateUpdates == 0)
			{
				currentState.stateFirstUpdate();
			}
			currentState.stateUpdate();
			currentStateUpdates++;
		}
	}

	public virtual void LateUpdate()
	{
		if (currentState != null)
		{
			currentState.stateLateUpdate();
		}
	}

	public virtual void FixedUpdate()
	{
		if (currentState != null)
		{
			currentState.stateFixedUpdate();
		}
	}

	public virtual void addState(State s)
	{
		s.stateInit(this);
	}

	public virtual void addChildStates()
	{
		Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(State), true);
		int i = 0;
		Component[] array = componentsInChildren;
		for (int length = array.Length; i < length; i++)
		{
			addState((State)array[i]);
		}
	}

	public virtual void setDelayedState()
	{
		setState(delayedNextState);
	}

	public virtual void setState(State to)
	{
		if (currentState != null)
		{
			currentState.stateSleep();
			currentState.gameObject.SetActiveRecursively(false);
		}
		currentState = to;
		currentStateUpdates = 0;
		currentState.gameObject.SetActiveRecursively(true);
		currentState.stateAwake();
	}

	public virtual void setStateAfter(float time, State to)
	{
		delayedNextState = to;
		Invoke("setDelayedState", time);
	}

	public virtual void setNextState()
	{
		if (currentState != null)
		{
			setState(currentState.getNextState());
		}
	}
}
