using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBeforeLeave : MonoBehaviour
{
	public GameObject TeleportRig;
	private void Start()
	{
		GetComponent<BoxCollider>().enabled = false;
	}

	private void OnMouseDown()
	{
		TeleportRig.GetComponent<Teleport>().AddLeaveObject();
		gameObject.SetActive(false);
		
	}
}
