using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class duvaraCakma : MonoBehaviour
{
    public AudioClip mySound;
    public ParticleSystem myParticle;
    AudioSource mySource;
    public GameObject teleportRig;

    float myNumber;

    // Use this for initialization
    void Start()
    {
        myNumber = 0;
        mySource = GetComponent<AudioSource>();
        myNumber = transform.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        myNumber = myNumber - 2;
        mySource.PlayOneShot(mySound, 1);
        myParticle.Play();
        //Debug.Log("baget girdi");
        transform.localEulerAngles = new Vector3(myNumber, transform.localEulerAngles.y, 0);

        if (myNumber <= 0)
        {
            teleportRig.GetComponent<Teleport>().ToggleTeleportMode();
            teleportRig.GetComponent<Teleport>().afterBaget();
            GetComponent<BoxCollider>().enabled = false;
            teleportRig.GetComponent<Teleport>().ShowTebriklerDuvaraCak();
            enabled = false;
        }
    }
}