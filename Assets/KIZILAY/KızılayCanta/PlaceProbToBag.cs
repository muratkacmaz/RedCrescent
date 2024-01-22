using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlaceProbToBag : MonoBehaviour
{
     
    //public GameObject myObj, controllerObj;
    //firattan once
    public bool TeleportEnabled
    {
        get { return teleportEnabled; }
    }


    public GameObject teleportRig;
    public AudioClip clickSound;
    public AudioClip correctSound;

    // public Text touchDeger;
    public Bezier bezier;
    public GameObject teleportSprite;

   
    public GameObject teleportParticleAnim;

    private bool teleportEnabled;
    //private bool firstClick;
    //private float firstClickTime;
    private float doubleClickTimeLimit = 0.5f;
    int successCounter = 0;

    // Vector3 angVel;
    //Vector3 positionBeforGrab;
    //Quaternion rotationBeforGrab;
    public GameObject hittedGameObj;

    GameObject dummy = null;

    void Start()
    {

        // teleportEnabled = true;
        // //firstClick = false;
        // //firstClickTime = 0f;
        // teleportSprite.SetActive(false);
        // bezier.ToggleDraw(teleportEnabled);

    }

    void Update()
    {
        //UpdateTeleportEnabled();
        OVRInput.Controller activeController = OVRInput.GetActiveController();
        // angVel = OVRInput.GetLocalControllerAngularVelocity(activeController);


        // hittedGameObj = bezier.getHitObject();
        // Debug.Log(hittedGameObj.gameObject.tag);
        // HandleBezier();
        // HandleTeleport();

    }

    public void AddToBag(GameObject hittedGameObj)
    {
        GetComponent<AudioSource>().PlayOneShot(clickSound, 1);

        if(hittedGameObj.gameObject.tag == "BagProp")
        {
            successCounter++;
            hittedGameObj.GetComponent<CantaObjesi>().putInBag();
            GetComponent<AudioSource>().PlayOneShot(correctSound, 1);
            if (successCounter == 6) { Invoke("enableTeleport", 2); }
        }
    }
    void HandleTeleport()
    {

        if (bezier.endPointDetected)
        { // There is a point to teleport to.
            string mybakilanObj = PlayerPrefs.GetString("bakilanObj");
            // touchDeger.text = mybakilanObj;
            // Display the teleport point.
            teleportSprite.SetActive(true);
            teleportSprite.transform.position = bezier.EndPoint;


            if (OVRInput.GetDown(OVRInput.Button.One))
            { // The trigger is pressed.
                GetComponent<AudioSource>().Play();
                if (hittedGameObj.gameObject.tag == "Floor")
                {
                    TeleportToPosition(bezier.EndPoint);
                }




                //if (!firstClick)
                //{ // The first click is detected.
                //    firstClick = true;
                //    firstClickTime = Time.unscaledTime;
                //}
                //else
                //{ // The second click detected, so toggle teleport mode.
                //    firstClick = false;
                //    ToggleTeleportMode();
                //}
            }


            else if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) // Teleport to the position.
                                                                            //  TeleportToPosition(bezier.EndPoint); firattan once sadece bu if ici yok
            {
               GetComponent<AudioSource>().PlayOneShot(clickSound, 1);

               if(hittedGameObj.gameObject.tag == "BagProp")
                {
                    successCounter++;
                    hittedGameObj.GetComponent<CantaObjesi>().putInBag();
                    GetComponent<AudioSource>().PlayOneShot(correctSound, 1);
                    if (successCounter == 6) { Invoke("enableTeleport", 2); }
                }

            }
            
        }
        //else {
        //    teleportSprite.SetActive(false);
        //}
    }

    void TeleportToPosition(Vector3 teleportPos)
    {
        teleportRig.transform.position = teleportPos + Vector3.up;
        teleportParticleAnim.SetActive(true);
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

    void ToggleTeleportMode()
    {
        SceneManager.LoadScene("TeleporterScene", LoadSceneMode.Single);
        /*  teleportEnabled = !teleportEnabled;
          bezier.ToggleDraw(teleportEnabled);
          if (!teleportEnabled) {
              teleportSprite.SetActive(false);
          }*/
    }

    //public void hepsiniKapat()
    //{
    //    myObj.transform.SetParent(controllerObj.transform);
    //    myObj.transform.localPosition = new Vector3(0, 0, 0);
    //}

    public void enableTeleport()
    {
        gameObject.SetActive(false);
        teleportRig.GetComponent<Teleport>().enabled = true;
        teleportRig.GetComponent<Teleport>().afterBaget();
        teleportRig.GetComponent<Teleport>().ShowTebriklerCantaYapma();
    }
}



 
