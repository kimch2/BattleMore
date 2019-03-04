using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMasterMind : MonoBehaviour
{
	[Tooltip("If this is null, actions will be unlimited")]
	public AIAPM apmManager;

	public static AIMasterMind main;

	private void Awake()
	{
		main = this;
	}







	public int requestActions(int requestNumber)
	{
		return requestNumber;
	}

	public void RemoveTask(ITask toRemove)
	{
		if (apmManager)
		{
			apmManager.RemoveTask(toRemove);
		}
	}

	public void addTask(ITask toAdd)
	{
		if (apmManager)
		{
			apmManager.addTask(toAdd);
		}
		else
		{
			toAdd.Execute();
		}
	}

}