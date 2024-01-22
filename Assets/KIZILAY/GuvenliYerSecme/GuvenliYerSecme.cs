using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GuvenliYerSecme : MonoBehaviour
{
    //public GameObject myObj, controllerObj;
    //firattan once
    public bool TeleportEnabled
    {
        get { return teleportEnabled; }
    }


    public GameObject teleportRig, masasesi;
    public AudioClip dolapsesi, yataksesi, penceresesi;
    Vector3 oldCameraPosition;
    Quaternion oldCameraRotation;

    // public Text touchDeger;
    public Bezier bezier;
    public GameObject teleportSprite;


    public GameObject flag;
    public GameObject flagCurser;


    private bool teleportEnabled;

    //private bool firstClick;
    //private float firstClickTime;
    private float doubleClickTimeLimit = 0.5f;


    // Vector3 angVel;
    //Vector3 positionBeforGrab;
    //Quaternion rotationBeforGrab;
    public GameObject hittedGameObj;


    void Start()
    {
        oldCameraPosition = teleportRig.transform.position;
        oldCameraRotation = teleportRig.transform.rotation;
        teleportEnabled = true;
        flagCurser.SetActive(true);
        //firstClick = false;
        //firstClickTime = 0f;
        teleportSprite.SetActive(false);
    }


    void Update()
    {
        // //UpdateTeleportEnabled();
        // OVRInput.Controller activeController = OVRInput.GetActiveController();
        // // angVel = OVRInput.GetLocalControllerAngularVelocity(activeController);
        //
        //
        // hittedGameObj = bezier.getHitObject();
    }


    // On double-click, toggle teleport mode on and off.
    //void UpdateTeleportEnabled() {
    //    if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) { // The trigger is pressed.
    //        if (!firstClick) { // The first click is detected.
    //            firstClick = true;
    //            firstClickTime = Time.unscaledTime;
    //        } else { // The second click detected, so toggle teleport mode.
    //            firstClick = false;
    //            ToggleTeleportMode();
    //        }
    //    }

    //    if (Time.unscaledTime - firstClickTime > doubleClickTimeLimit) { // Time for the double click has run out.
    //        firstClick = false;
    //    }
    //}


    GameObject dummy;

    public void SetClickedArea(GameObject clickedArea)
    {
        hittedGameObj = clickedArea;
        string mybakilanObj = PlayerPrefs.GetString("bakilanObj");
        // touchDeger.text = mybakilanObj;
        // Display the teleport point.
        // teleportSprite.SetActive(true);
        // teleportSprite.transform.position = bezier.EndPoint;

        if (hittedGameObj.gameObject.tag == "SecilebilirBolge")
        {
            dummy = hittedGameObj;

            foreach (Transform child in hittedGameObj.transform)
            {
                child.GetComponent<Renderer>().material.color = Color.blue;
                //child is your child transform
            }
        }

        else
        {
            foreach (Transform child in dummy.transform)
            {
                child.GetComponent<Renderer>().material.color = Color.red;
                //child is your child transform
            }
        }

        switch (hittedGameObj.gameObject.name)
        {
            case "MasaBolgesi":

                flag.transform.localPosition =new Vector3(0.055f,1.282f,0.349f);
                flag.SetActive(true);
                masasesi.SetActive(true);
                Invoke("enableTeleport", 6f);
                flagCurser.SetActive(false);

                break;

            case "DolapBolgesi":
                gameObject.GetComponent<AudioSource>().PlayOneShot(dolapsesi, 1);

                // Dolap Bölgesi Sesii

                break;

            case "YatakBolgesi":
                gameObject.GetComponent<AudioSource>().PlayOneShot(yataksesi, 1);

                // Yatak Bölgesi Sesi

                break;

            case "Pencere":
                gameObject.GetComponent<AudioSource>().PlayOneShot(penceresesi, 1);
                // Pencere Bölgesi sesiii

                break;
        }
    }


    // Optional: use the touchpad to move the teleport point closer or further.
    void HandleBezier()
    {
        Vector2 touchCoords = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

        if (Mathf.Abs(touchCoords.y) > 0.8f)
        {
            //  bezier.ExtensionFactor = touchCoords.y > 0f ? 1f : -1f;
            bezier.ExtensionFactor = 1f;
        }
        else
        {
            bezier.ExtensionFactor = 0f;
        }
    }


    public void enableTeleport()
    {
        gameObject.SetActive(false);
        teleportRig.GetComponent<Teleport>().enabled = true;
        teleportRig.GetComponent<Teleport>().StartMoveAndRotation();
        teleportRig.GetComponent<Teleport>().ShowTebriklerGuvenliYerSecme();
        teleportRig.GetComponent<Teleport>().ResetPosition();

    }
}