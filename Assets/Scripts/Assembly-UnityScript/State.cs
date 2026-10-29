using UnityEngine;

public class State : MonoBehaviour
{
	protected StateMachine machine;

	public State nextState;

	public virtual void setNextState(State to)
	{
		nextState = to;
	}

	public virtual void setNextState()
	{
		if (nextState != null)
		{
			machine.setNextState();
		}
	}

	public virtual State getNextState()
	{
		return nextState;
	}

	public virtual void setNextStateAfter(float time)
	{
		if (nextState != null)
		{
			machine.Invoke("setNextState", time);
		}
	}

	public virtual void stateInit(StateMachine m)
	{
		machine = m;
	}

	public virtual void stateUpdate()
	{
	}

	public virtual void stateLateUpdate()
	{
	}

	public virtual void stateFixedUpdate()
	{
	}

	public virtual void stateFirstUpdate()
	{
	}

	public virtual void stateAwake()
	{
	}

	public virtual void stateSleep()
	{
	}
}
