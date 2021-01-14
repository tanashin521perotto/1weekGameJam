using UnityEngine;
using System.Collections;

public class LoopSound : MonoBehaviour
{
	public static bool created = false;

	// Use this for initialization
	void Start()
	{
		if (!created)
		{
			DontDestroyOnLoad(this);
			created = true;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}