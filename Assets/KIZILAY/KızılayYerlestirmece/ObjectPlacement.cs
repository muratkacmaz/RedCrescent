using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacement : MonoBehaviour
{
    AudioSource mySource;
    public AudioClip yanlisAsagiKoymamalisin, yanlisRafaKoymamalisin;
    public GameObject teleportObj, objYerlestir;

    int successCounter = 0;

    //public GameObject myObj, controllerObj;
    //firattan once
    public bool TeleportEnabled
    {
        get { return teleportEnabled; }
    }

    public GameObject[] up_places;
    public GameObject[] down_places;

    int u;
    int d;


    // public Text touchDeger;
    public Bezier bezier;
    public GameObject teleportSprite;

    public GameObject UstRaf;
    public GameObject AltRaf;

    public GameObject controllerObj;

    private bool teleportEnabled;

    // private bool firstClick;
    // private float firstClickTime;
    private float doubleClickTimeLimit = 0.5f;


    // Vector3 angVel;
    //Vector3 positionBeforGrab;
    //Quaternion rotationBeforGrab;
    public GameObject hittedGameObj;
    GameObject dummy = null;
    public GameObject dummyObject;

    void Start()
    {
        mySource = this.GetComponent<AudioSource>();
        teleportEnabled = true;
        //  firstClick = false;
        //  firstClickTime = 0f;
        teleportSprite.SetActive(false);
        //bezier.ToggleDraw(teleportEnabled);
        u = 0;
        d = 0;
    }

    void Update()
    {
        //UpdateTeleportEnabled();
        //OVRInput.Controller activeController = OVRInput.GetActiveController();
        // angVel = OVRInput.GetLocalControllerAngularVelocity(activeController);


        //hittedGameObj = bezier.getHitObject();
        //HandleBezier();
        //HandleTeleport();
    }

    public void placeToUp()
    {
        if (dummy != null)
        {
            dummy.transform.position = up_places[u].transform.position;
            u++;
            dummy.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void placeToDown()
    {
        if (dummy != null)
        {
            dummy.transform.position = down_places[d].transform.position;
            d++;
            dummy.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void handleGrab()
    {
        hittedGameObj.transform.SetParent(controllerObj.transform);
        hittedGameObj.transform.localEulerAngles = new Vector3(0, 0, 30);
        hittedGameObj.transform.localPosition = new Vector3(-1, -1, 2);
        hittedGameObj.GetComponent<Objects>().isRotating = false;
        hittedGameObj.GetComponent<Rigidbody>().isKinematic = true;
    }


    public GameObject GetClickedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }

        return null;
    }

    public void HoldFlowingObject(GameObject flowingObject)
    {
        if (flowingObject.tag == "FlowingObject")
        {
            hittedGameObj = flowingObject;
            UstRaf.GetComponent<Collider>().enabled = true;
            AltRaf.GetComponent<Collider>().enabled = true;
            //hittedGameObj.transform.SetParent(controllerObj.transform);
            //hittedGameObj.transform.localPosition = new Vector3(-1, -1, 2);
            hittedGameObj.GetComponent<Objects>().isRotating = false;
            // hittedGameObj.GetComponent<Rigidbody>().isKinematic = true;
            dummy = hittedGameObj;
        }
    }

    public void DropFlowingObject(GameObject flowingObject)
    {
         if (dummy.GetComponent<Objects>().collideWith == 0)
            {
                dummy.transform.parent = null;
                dummy.GetComponent<Objects>().placeBack();
                dummy.GetComponent<Objects>().isRotating = true;
                UstRaf.GetComponent<Collider>().enabled = false;
                AltRaf.GetComponent<Collider>().enabled = false;
                dummy = null;
            }
            else if (dummy.GetComponent<Objects>().collideWith == 1)
            {
                if (dummy.GetComponent<Objects>().isBreakable == false)
                {
                    dummy.transform.parent = null;
                    placeToUp();
                    dummy.GetComponent<Objects>().gameObject.tag = "PlacedObject";
                    successCounter++;
                    UstRaf.GetComponent<Collider>().enabled = false;
                    AltRaf.GetComponent<Collider>().enabled = false;
                    if (successCounter == 6)
                    {
                        Invoke("enableTeleport", 2);
                    }
                }
                else if (dummy.GetComponent<Objects>().isBreakable == true)
                {
                    mySource.PlayOneShot(yanlisRafaKoymamalisin, 1);
                    dummy.transform.parent = null;
                    dummy.GetComponent<Objects>().placeBack();
                    UstRaf.GetComponent<Collider>().enabled = false;
                    AltRaf.GetComponent<Collider>().enabled = false;
                    dummy = null;
                }
            }
            else if (dummy.GetComponent<Objects>().collideWith == -1)
            {
                if (dummy.GetComponent<Objects>().isBreakable == true)
                {
                    dummy.transform.parent = null;
                    placeToDown();
                    successCounter++;
                    dummy.GetComponent<Objects>().gameObject.tag = "PlacedObject";
                    UstRaf.GetComponent<Collider>().enabled = false;
                    AltRaf.GetComponent<Collider>().enabled = false;
                    if (successCounter == 6)
                    {
                        Invoke("enableTeleport", 2);
                    }
                }
                else if (dummy.GetComponent<Objects>().isBreakable == false)
                {
                    mySource.PlayOneShot(yanlisAsagiKoymamalisin, 1);
                    dummy.transform.parent = null;
                    dummy.GetComponent<Objects>().placeBack();
                    UstRaf.GetComponent<Collider>().enabled = false;
                    AltRaf.GetComponent<Collider>().enabled = false;
                    dummy = null;
                }
            }
    }
    void HandleTeleport()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // The trigger is pressed.
            hittedGameObj = GetClickedObject();
            GetComponent<AudioSource>().Play();
            switch (hittedGameObj.tag)
            {
                case "FlowingObject":
                    UstRaf.GetComponent<Collider>().enabled = true;
                    AltRaf.GetComponent<Collider>().enabled = true;
                    hittedGameObj.transform.SetParent(controllerObj.transform);
                    //hittedGameObj.transform.localPosition = new Vector3(-1, -1, 2);
                    hittedGameObj.GetComponent<Objects>().isRotating = false;
                    // hittedGameObj.GetComponent<Rigidbody>().isKinematic = true;
                    dummy = hittedGameObj;
                    break;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(dummy == null) return;
            if (dummy.GetComponent<Objects>().collideWith == 0)
            {
                dummy.transform.parent = null;
                dummy.GetComponent<Objects>().placeBack();
                dummy.GetComponent<Objects>().isRotating = true;
                UstRaf.GetComponent<Collider>().enabled = false;
                AltRaf.GetComponent<Collider>().enabled = false;
                dummy = null;
            }
            else if (dummy.GetComponent<Objects>().collideWith == 1)
            {
                if (dummy.GetComponent<Objects>().isBreakable == false)
                {
                    dummy.transform.parent = null;
                    placeToUp();
                    dummy.GetComponent<Objects>().gameObject.tag = "PlacedObject";
                    successCounter++;
                    UstRaf.GetComponent<Collider>().enabled = false;
                    AltRaf.GetComponent<Collider>().enabled = false;
                    if (successCounter == 6)
                    {
                        Invoke("enableTeleport", 2);
                    }
                }
                else if (dummy.GetComponent<Objects>().isBreakable == true)
                {
                    mySource.PlayOneShot(yanlisRafaKoymamalisin, 1);
                    dummy.transform.parent = null;
                    dummy.GetComponent<Objects>().placeBack();
                    UstRaf.GetComponent<Collider>().enabled = false;
                    AltRaf.GetComponent<Collider>().enabled = false;
                    dummy = null;
                }
            }
            else if (dummy.GetComponent<Objects>().collideWith == -1)
            {
                if (dummy.GetComponent<Objects>().isBreakable == true)
                {
                    dummy.transform.parent = null;
                    placeToDown();
                    successCounter++;
                    dummy.GetComponent<Objects>().gameObject.tag = "PlacedObject";
                    UstRaf.GetComponent<Collider>().enabled = false;
                    AltRaf.GetComponent<Collider>().enabled = false;
                    if (successCounter == 6)
                    {
                        Invoke("enableTeleport", 2);
                    }
                }
                else if (dummy.GetComponent<Objects>().isBreakable == false)
                {
                    mySource.PlayOneShot(yanlisAsagiKoymamalisin, 1);
                    dummy.transform.parent = null;
                    dummy.GetComponent<Objects>().placeBack();
                    UstRaf.GetComponent<Collider>().enabled = false;
                    AltRaf.GetComponent<Collider>().enabled = false;
                    dummy = null;
                }
            }
        }


        /*else if (Input.GetMouseButtonDown(0)) // Teleport to the position.   //  TeleportToPosition(bezier.EndPoint); firattan once sadece bu if ici yok
                                                                     
        {
            GetComponent<AudioSource>().Play();

            switch (hittedGameObj.tag)
            {
                case "FlowingObject":
                    UstRaf.GetComponent<Collider>().enabled = true;
                    AltRaf.GetComponent<Collider>().enabled = true;
                    hittedGameObj.transform.SetParent(controllerObj.transform);
                    hittedGameObj.GetComponent<Objects>().isRotating = false;
                    //hittedGameObj.GetComponent<Rigidbody>().isKinematic = true;
                    dummy = hittedGameObj;
                    break;
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {

            if (dummy.GetComponent<Objects>().collideWith == 0)
            {
                dummy.transform.parent = null;
                dummy.GetComponent<Objects>().placeBack();
                dummy.GetComponent<Objects>().isRotating = true;
                UstRaf.GetComponent<Collider>().enabled = false;
                AltRaf.GetComponent<Collider>().enabled = false;
                dummy = null;
            }
            else if (dummy.GetComponent<Objects>().collideWith == 1)
            {
                if (dummy.GetComponent<Objects>().isBreakable == false)
                {
                    dummy.transform.parent = null;
                    placeToUp();
                    successCounter++; 
                    dummy.GetComponent<Objects>().gameObject.tag = "PlacedObject";
                    UstRaf.GetComponent<Collider>().enabled = false;
                    AltRaf.GetComponent<Collider>().enabled = false;
                    if (successCounter == 6) { Invoke("enableTeleport", 2); }
                }
                else if (dummy.GetComponent<Objects>().isBreakable == true)
                {
                    mySource.PlayOneShot(yanlisRafaKoymamalisin, 1);
                    dummy.transform.parent = null;
                    dummy.GetComponent<Objects>().placeBack();
                    UstRaf.GetComponent<Collider>().enabled = false;
                    AltRaf.GetComponent<Collider>().enabled = false;
                    dummy = null;
                }
            }
            else if (dummy.GetComponent<Objects>().collideWith == -1)
            {
                if (dummy.GetComponent<Objects>().isBreakable == true)
                {
                    dummy.transform.parent = null;
                    placeToDown();
                    dummy.GetComponent<Objects>().gameObject.tag = "PlacedObject";
                    successCounter++;
                    UstRaf.GetComponent<Collider>().enabled = false;
                    AltRaf.GetComponent<Collider>().enabled = false;
                     if (successCounter == 6) { Invoke("enableTeleport", 2); }
                }
                else if (dummy.GetComponent<Objects>().isBreakable == false)
                {
                    mySource.PlayOneShot(yanlisAsagiKoymamalisin, 1);
                    dummy.transform.parent = null;
                    dummy.GetComponent<Objects>().placeBack();
                    UstRaf.GetComponent<Collider>().enabled = false;
                    AltRaf.GetComponent<Collider>().enabled = false;
                    dummy = null;
                }
            }

        }*/
    }

    public void enableTeleport()
    {
        objYerlestir.SetActive(false); 
        teleportObj.GetComponent<Teleport>().enabled = true;
        teleportObj.GetComponent<Teleport>().StartMoveAndRotation();
        teleportObj.GetComponent<Teleport>().ShowTebriklerObjeYerlestirme();
        teleportObj.GetComponent<Teleport>().SetPosition(new Vector3(-52.66f, 1.42f, -8.74f));
    }

    void TeleportToPosition(Vector3 teleportPos)
    {
        gameObject.transform.position = teleportPos + Vector3.up * 0.5f;
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
}