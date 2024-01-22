using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionButtons : MonoBehaviour {


    public bool isSelectionBox;
	// Use this for initialization
	public GameObject CardSelection;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnMouseDown()
	{
		CardSelection.GetComponent<CardSelectionController>().SelectedCard(gameObject);
	}
}
