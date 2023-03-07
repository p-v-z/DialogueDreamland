using UnityEngine;

/// <summary>
/// This class is used to create a singleton object. It is intended for use in a single threaded environment.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindAnyObjectByType<T>();

				if (_instance == null)
				{
					var obj = new GameObject(typeof(T).Name);
					_instance = obj.AddComponent<T>();
				}
			}

			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if (_instance == null)
		{
			_instance = this as T;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
