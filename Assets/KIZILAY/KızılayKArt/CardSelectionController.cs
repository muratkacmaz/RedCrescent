using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectionController : MonoBehaviour
{
    //public GameObject myObj, controllerObj;
    //firattan once
    public bool TeleportEnabled
    {
        get { return teleportEnabled; }
    }


    // public Text touchDeger;
    // public Bezier bezier;
    public GameObject teleportSprite;


    public GameObject selectedObject;

    public GameObject selectBox, cancelBox;


    private bool teleportEnabled;

    //private bool firstClick;
    //private float firstClickTime;
    private float doubleClickTimeLimit = 0.5f;


    // Vector3 angVel;
    //Vector3 positionBeforGrab;
    //Quaternion rotationBeforGrab;
    public GameObject hittedGameObj;

    GameObject dummy = null;

    void Start()
    {
        teleportEnabled = true;
        //firstClick = false;
        //firstClickTime = 0f;
        teleportSprite.SetActive(false);
        //  bezier.ToggleDraw(teleportEnabled);
    }

    void Update()
    {
        //UpdateTeleportEnabled();
        //OVRInput.Controller activeController = OVRInput.GetActiveController();
        // angVel = OVRInput.GetLocalControllerAngularVelocity(activeController);


        // hittedGameObj = bezier.getHitObject();
        // Debug.Log(hittedGameObj.gameObject.tag);
        //HandleTeleport();
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


    //public void deneme()
    //{
    //    gameLogic.floorPlaces[hittedGameObj.GetComponent<Guest>().waitingFloorPlace - 1].free = true;
    //    hittedGameObj.transform.parent = null;
    //    hittedGameObj.GetComponent<Guest>().placeInElevator();
    //    hittedGameObj.GetComponent<Guest>().inElevator = true;
    //    hittedGameObj.GetComponent<Guest>().wantedFloorText.text = "Thnx!";
    //    hittedGameObj.GetComponent<Guest>().canvas.SetActive(true);
    //}
    public void selectCard()
    {
        if (hittedGameObj.gameObject.tag == "Card")
        {
            hittedGameObj.transform.position = selectedObject.transform.position;
            selectedObject = hittedGameObj;
            selectBox.SetActive(true);
            cancelBox.SetActive(true);
        }
    }

    public void SelectedCard(GameObject selectedCard)
    {
        hittedGameObj = selectedCard;

        GetComponent<AudioSource>().Play();
        if (hittedGameObj.gameObject.CompareTag("Card"))
        {
            if (dummy != null)
            {
                dummy.GetComponent<Cards>().isRotating = false;
                dummy.GetComponent<Cards>().placeBack();
            }

            hittedGameObj.transform.position = selectedObject.transform.position;
            dummy = hittedGameObj;
            dummy.GetComponent<Cards>().isRotating = true;
            selectBox.SetActive(true);
            cancelBox.SetActive(true);
        }

        else if (hittedGameObj.gameObject.tag == "SelectionBox")
        {
            if (hittedGameObj.GetComponent<SelectionButtons>().isSelectionBox == true)
            {
                dummy.GetComponent<Cards>().placeSelections();
                dummy.GetComponent<Cards>().isRotating = false;
                dummy.GetComponent<Cards>().playSound();
                dummy = null;
            }
            else if (hittedGameObj.GetComponent<SelectionButtons>().isSelectionBox == false)
            {
                dummy.GetComponent<Cards>().placeBack();
                dummy.GetComponent<Cards>().isRotating = false;
                dummy = null;
            }

            selectBox.SetActive(false);
            cancelBox.SetActive(false);
        }

        // GetComponent<AudioSource>().Play();
        //         if (hittedGameObj.gameObject.tag == "Card")
        //         {
        //             if (dummy != null)
        //             {
        //                 dummy.GetComponent<Cards>().isRotating = false;
        //                 dummy.GetComponent<Cards>().placeBack();
        //             }
        //
        //             hittedGameObj.transform.position = selectedObject.transform.position;
        //             dummy = hittedGameObj;
        //             dummy.GetComponent<Cards>().isRotating = true;
        //             selectBox.SetActive(true);
        //             cancelBox.SetActive(true);
        //         }
        //
        //         else if (hittedGameObj.gameObject.tag == "SelectionBox")
        //         {
        //             if (hittedGameObj.GetComponent<SelectionButtons>().isSelectionBox == true)
        //             {
        //                 dummy.GetComponent<Cards>().placeSelections();
        //                 dummy.GetComponent<Cards>().isRotating = false;
        //                 dummy.GetComponent<Cards>().playSound();
        //                 dummy = null;
        //
        //             }
        //             else if (hittedGameObj.GetComponent<SelectionButtons>().isSelectionBox == false)
        //             {
        //                 dummy.GetComponent<Cards>().placeBack();
        //                 dummy.GetComponent<Cards>().isRotating = false;
        //                 dummy = null;
        //             }
        //
        //             selectBox.SetActive(false);
        //             cancelBox.SetActive(false);
        //
        //
        //         }
    }

    void TeleportToPosition(Vector3 teleportPos)
    {
        gameObject.transform.position = teleportPos + Vector3.up * 0.5f;
    }

    // Optional: use the touchpad to move the teleport point closer or further.

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