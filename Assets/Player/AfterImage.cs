using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
	private void Awake()
	{
		Destroy(gameObject, 0.25f);
	}
}
