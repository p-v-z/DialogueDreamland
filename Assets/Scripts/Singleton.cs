using UnityEngine;

/// <summary>
/// This class is used to create a singleton object. It is intended for use in a single threaded environment.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T instance;
	
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType<T>();

				if (instance == null)
				{
					var obj = new GameObject(typeof(T).Name);
					instance = obj.AddComponent<T>();
				}
			}

			return instance;
		}
	}

	protected virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
			if (instance.gameObject.transform.parent != null)
			{
				instance.gameObject.transform.parent = null;
			}
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
