using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public float speed;
    float speedyedek;
    private void Start()
    {
        speedyedek = speed;
        
    }
    // Update is called once per frame
    void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * speed);
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered");
        speed = 0;
    }
    private void OnTriggerExit(Collider other)
    {
        speed = speedyedek;
    }

}
