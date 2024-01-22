using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCanvasInAnim : MonoBehaviour {


    //public GameObject cameraCanvas;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenCameraCanvas()
    {
       // cameraCanvas.SetActive(true);
    }
    public void CloseCameraCanvas()
    {
        transform.parent.gameObject.SetActive(false);
    }

}

