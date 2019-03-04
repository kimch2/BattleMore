using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAPM : MonoBehaviour
{
	public float updateRate = .1f;
	public int APM = 100;
	public int perSecondMax = 4;

	List<float> minuteActions = new List<float>(); // Addstuff to this list plus 60 seconds, which is the time it needs to be removed
	List<float> secondActions = new List<float>();

	List<ITask> TaskQueue = new List<ITask>();

	private void Start()
	{
		InvokeRepeating("UpdateTasks", updateRate, updateRate);
	}


	private void UpdateTasks()
	{
		TrimList(minuteActions);
		TrimList(secondActions);

		int TasksAvailable = Mathf.Min(APM - minuteActions.Count, perSecondMax - secondActions.Count);

		for (int i = 0; i < TasksAvailable; i++)
		{
			int ActionsUsed = GetNextTask().Execute();
			LogAPM(ActionsUsed);
		}
	}

	public void RemoveTask(ITask toRemove)
	{
		TaskQueue.Remove(toRemove);
	}

	// To be used if we arent using a MasterMind queue
	public int requestActions(int requestNumber) // Callback for when there are things to be done?
	{
		int numLeft = Mathf.Min(perSecondMax - secondActions.Count, APM - minuteActions.Count);
		numLeft = Mathf.Min(numLeft, requestNumber);
		LogAPM(numLeft);

		return numLeft;
	}


	void TrimList(List<float> lister)
	{
		int CutOff = 0;
		for (int i = 0; i < lister.Count; i++)
		{
			if (lister[i] > Time.time)
			{
				CutOff = i;
				break;
			}
		}
		if (CutOff != 0)
		{
			lister.RemoveRange(0, CutOff);
		}
	}


	public void addTask(ITask toAdd)
	{
		if (TaskQueue.Count == 0 && minuteActions.Count < APM && secondActions.Count < perSecondMax)
		{
			LogAPM(toAdd.Execute());
		}
		else
		{
			TaskQueue.Add(toAdd);
		}
	}


	void LogAPM(int actions)
	{
		for (int i = 0; i < actions; i++)
		{
			minuteActions.Add(Time.time + 60);
			secondActions.Add(Time.time + 1);
		}
	}

	private ITask GetNextTask()
	{
		float tempPriority = 0;
		ITask best = null;
		float bestPriority = 0;

		foreach (ITask t in TaskQueue)
		{
			if (t == null)
			{
				continue;
			}

			tempPriority = t.GetPriority();
			if (tempPriority > bestPriority)
			{
				bestPriority = tempPriority;
				best = t;
			}
		}
		TaskQueue.Remove(best);
		return best;
	}
}

public interface ITask
{
	float GetPriority();
	int Execute(); // returns tasks used
}
