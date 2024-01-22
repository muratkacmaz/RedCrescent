using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuvenliYer : MonoBehaviour {


    public GuvenliYerSecme GuvenliYerSecme;
    public bool isSafe;

	private void OnMouseDown()
	{
		GuvenliYerSecme.SetClickedArea(gameObject);
	}
	

}
