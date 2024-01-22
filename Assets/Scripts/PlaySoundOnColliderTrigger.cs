using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnColliderTrigger : MonoBehaviour {
    public AudioClip mySound;
    AudioSource mySource;
    public GameObject teleportRig;
	// Use this for initialization
	void Start () {
        mySource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "baget")
        {

            mySource.PlayOneShot(mySound,1);
            //Debug.Log("baget girdi");
            this.transform.GetChild(0).gameObject.SetActive(true);
            if (gameObject.tag == "FxTemporaire") {
                teleportRig.GetComponent<Teleport>().ToggleTeleportMode(); teleportRig.GetComponent<Teleport>().afterBaget(); 
            }
        }
    }
}
