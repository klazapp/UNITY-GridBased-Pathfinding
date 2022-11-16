﻿using UnityEngine;

public abstract class TMonoSingleton<T> : MonoBehaviour where T : Component
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<T>();
				if (instance == null) 
				{
					GameObject obj = new GameObject();
					obj.name = typeof(T).Name;
					instance = obj.AddComponent<T>();
				}
			}
			return instance;
		}
	}

	public static bool Exist
	{
		get { return (instance != null); }
	}

	protected virtual void Awake()
	{
		if (instance != null && instance != this) 
		{
			Destroy(this);
			return;
		}
		instance = this as T;
	}
}
