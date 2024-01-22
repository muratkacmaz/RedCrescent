using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAnimationsOfChild : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (Transform Child in transform)
        {
            Child.GetComponent<Animator>().enabled = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
