using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour {
    public GameObject teleportObj, kartOyunu,kartSoundFinished;
    public AudioClip classicSound, YanlisHareketSound;
    public float invokedSeconds;
    public string name;
    public Text nameTag;
    public bool trueSelection;

    public GameObject selectedObject;
    public Vector3 startingPlace;

    public GameObject[] selectionPlacements;

    public bool isRotating;

    
    

    // Use this for initialization
    void Start() {
        PlayerPrefs.SetInt("successCounter",0);
        nameTag.text = name;
        startingPlace = transform.position;
        isRotating = false;
    }

    // Update is called once per frame
    void Update() {

        if(isRotating == true)
        {
            RotationOfCards();
        }
        else if(isRotating == false)
        {
          transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);
        }

    }

    private void OnMouseDown()
    {
        kartOyunu.GetComponent<CardSelectionController>().SelectedCard(gameObject);
    }

    public void placeSelections(){
        if(name == "ÇÖK")
        {
            transform.position = selectionPlacements[0].transform.position;
            gameObject.tag = "TrueCard";
            PlayerPrefs.SetInt("successCounter", PlayerPrefs.GetInt("successCounter") + 1);
            
            if (PlayerPrefs.GetInt("successCounter") == 3) { kartSoundFinished.SetActive(true); Invoke("fenableTeleport", invokedSeconds); }
        }
        else if(name == "KAPAN")
        {
            transform.position = selectionPlacements[1].transform.position;
            gameObject.tag = "TrueCard";
            PlayerPrefs.SetInt("successCounter", PlayerPrefs.GetInt("successCounter") + 1);

            if (PlayerPrefs.GetInt("successCounter") == 3) { kartSoundFinished.SetActive(true); Invoke("enableTeleport", invokedSeconds); }

        }
        else if(name == "TUTUN")
        {
            transform.position = selectionPlacements[2].transform.position;
            gameObject.tag = "TrueCard";
            PlayerPrefs.SetInt("successCounter", PlayerPrefs.GetInt("successCounter") + 1);

            if (PlayerPrefs.GetInt("successCounter") == 3) { kartSoundFinished.SetActive(true); Invoke("enableTeleport", invokedSeconds); }
        }
        else
        {
            transform.position = startingPlace;
            GetComponent<AudioSource>().PlayOneShot(YanlisHareketSound, 1);
        }
        
    }
    public void placeBack()
    {
        transform.position = startingPlace; 
    }

    public void selectCard()
    {
       transform.position = selectedObject.transform.position;
       
    }

    public void RotationOfCards()
    {

        transform.localRotation *= Quaternion.AngleAxis(60f * Time.deltaTime, Vector3.up);
    }

    public void playSound()
    {
        GetComponent<AudioSource>().PlayOneShot(classicSound, 1);
    }
    public void enableTeleport()
    {
        kartOyunu.SetActive(false);
        teleportObj.GetComponent<Teleport>().enabled = true;
        teleportObj.GetComponent<Teleport>().depremBasla2();
    }
}
