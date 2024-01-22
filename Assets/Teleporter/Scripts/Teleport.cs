using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public GameObject canvasImage,
        dusecekEsyalar,
        depremDurduSound,
        depremFizikAnimator,
        portalParent,
        depremCanvas,
        oncesiTamamlandi,
        guvenliYerPortal,
        guvenliYerSecme,
        gardenSound,
        CantaYapma,
        CantaYapmaPortal,
        cantaYapmaUI,
        kartOyunuBaslangic,
        masayaYerlestirmeGiris,
        potaCivi,
        tabloCivi,
        finishSound,
        odanageldin,
        objYerlestir,
        kartOyunu;

    public GameObject baget,
        drumSet,
        teleportParticleAnim,
        PepePortalEv,
        PepePortalBahce,
        PepePortalCocukOdasi,
        kidHouse,
        ObjYerlestirme,
        kartOyunuPortal,
        duvaraCak1,
        duvaraCak2,
        ladder1,
        ladder2;

    int interactionCounter = 0;

    //firattan once
    public bool TeleportEnabled
    {
        get { return teleportEnabled; }
    }

    // public Text touchDeger;
    public GameObject objeYerlestirmePanel,
        duvaraCak1Panel,
        duvaraCak2Panel,
        cantaYapmaPanel,
        guvenliYerSecmePanel,
        kartlarPanel,
        evdencikisPanel,
        tekrarBaslatPanel;


    public GameObject depremHazirligiPanel;
    public GameObject TebriklerObjeYerlestirme;
    public GameObject TebriklerDuvaraCak;
    public GameObject TebriklerCantaYapma;
    public GameObject TebriklerGuvenliYerSecme;
    public GameObject Bag;
    public GameObject Sneakers;
    public AudioSource PanelShow;
    public AudioClip Win;
    public AudioClip PopupOpen;



    private bool teleportEnabled;
    private bool firstClick;
    private float firstClickTime;
    private float doubleClickTimeLimit = 0.5f;
    private FPSController _fpsController;

    private int _objectsBeforeLeave = 0;

    void OnEnable()
    {
        portalParent.SetActive(true);
        cantaYapmaUI.SetActive(false);
        _fpsController = GetComponent<FPSController>();

        if (interactionCounter == 5)
        {
            oncesiTamamlandi.SetActive(true);
            kartOyunuPortal.SetActive(true);
            Invoke("depremBasla", 6);
        }
    }

    public void Baslat()
    {
        HideAll();
        Invoke("odayaGit", 1f);
    }

    public void TekrarBaslat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Start()
    {
        teleportEnabled = true;
        firstClick = false;
        firstClickTime = 0f;
        StopMoveAndRotation();
        SetRotation(new Vector3(0, 110, 0));
        //Invoke("odayaGit", 25);
        Invoke("ShowInitPopup", 3f);
    }

    public void ShowInitPopup()
    {
        depremHazirligiPanel.SetActive(true);
        PanelShow.PlayOneShot(PopupOpen);
    }

    void Update()
    {
        //UpdateTeleportEnabled();

        HandleBezier();
        //HandleTeleport();
    }

    // On double-click, toggle teleport mode on and off.
    void UpdateTeleportEnabled()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // The trigger is pressed.
            if (!firstClick)
            {
                // The first click is detected.
                firstClick = true;
                firstClickTime = Time.unscaledTime;
            }
            else
            {
                // The second click detected, so toggle teleport mode.
                firstClick = false;
                ToggleTeleportMode();
            }
        }

        if (Time.unscaledTime - firstClickTime > doubleClickTimeLimit)
        {
            // Time for the double click has run out.
            firstClick = false;
        }
    }

    void CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            PlayerPrefs.SetString("bakilanObj", hit.transform.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // There is a point to teleport to.
        string mybakilanObj = other.name;
        // touchDeger.text = mybakilanObj;
        // Display the teleport point.

        teleportParticleAnim.SetActive(false);
        switch (mybakilanObj)
        {
            case "PepePortalEv":
                Invoke("odayaGit", 1.5f);
                canvasImage.GetComponent<UISpriteAnimation>().enabled = true;


                break;
            case "PepePortalCocukOdasi":
                finishedDeprem();


                break;
            case "PepePortalBahce":
                camTransform(PepePortalBahce);
                baget.SetActive(true);
                drumSet.SetActive(true);
                ToggleTeleportMode();

                break;

            case "zemin":
                // hepsiniAc();
                break;

            case "ObjYerlestirme":
                objeYerlestirmePanel.SetActive(true);
                PanelShow.PlayOneShot(PopupOpen);

                StopMoveAndRotation();
                //ToggleTeleportMode();
                break;
            case "CantaYapma":
                //todoSes
                // bezier.bezierIsCurve();
                cantaYapmaPanel.SetActive(true);
                PanelShow.PlayOneShot(PopupOpen);

                StopMoveAndRotation();
                break;


            case "kartOyunuPortal":
                kartlarPanel.SetActive(true);
                PanelShow.PlayOneShot(PopupOpen);

                StopMoveAndRotation();

                break;
            case "duvaraCak1":
                duvaraCak1Panel.SetActive(true);
                PanelShow.PlayOneShot(PopupOpen);

                StopMoveAndRotation();
                break;
            case "duvaraCak2":
                duvaraCak2Panel.SetActive(true);
                PanelShow.PlayOneShot(PopupOpen);

                StopMoveAndRotation();
                break;
            case "guvenliYerSec":
                guvenliYerSecmePanel.SetActive(true);
                PanelShow.PlayOneShot(PopupOpen);

                StopMoveAndRotation();
                break;
        }
    }

    void TeleportToPosition(Vector3 teleportPos)
    {
        gameObject.transform.position = teleportPos + Vector3.up;
        teleportParticleAnim.SetActive(true);
    }

    // Optional: use the touchpad to move the teleport point closer or further.
    void HandleBezier()
    {
        Vector2 touchCoords = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

        if (Mathf.Abs(touchCoords.y) > 0.8f)
        {
            //  bezier.ExtensionFactor = touchCoords.y > 0f ? 1f : -1f;
        }
        else
        {
        }
    }

    public void ToggleTeleportMode()
    {
        // SceneManager.LoadScene("TeleporterScene", LoadSceneMode.Single);
        teleportEnabled = !teleportEnabled;
    }

    void hepsiniAc()
    {
        PepePortalEv.SetActive(true);
        PepePortalBahce.SetActive(true);
        PepePortalCocukOdasi.SetActive(true);
    }

    public void camTransform(GameObject portalObj)
    {
        portalObj.SetActive(false);
        transform.position = portalObj.transform.position;
        //transform.rotation = portalObj.transform.rotation;
        return;
        teleportParticleAnim.SetActive(true);
        portalParent.SetActive(false);
    }

    public void zemintelefport()
    {
    }

    public void afterBaget()
    {
        portalParent.SetActive(true);
        baget.SetActive(false);
        drumSet.SetActive(false);
        ladder1.SetActive(false);
        ladder2.SetActive(false);
        cantaYapmaUI.SetActive(false);
        if (interactionCounter == 5)
        {
            oncesiTamamlandi.SetActive(true);
            Invoke("depremBasla", 6);
        }
    }

    public void portalAc()
    {
        portalParent.SetActive(true);
    }

    public void odayaGit()
    {
        StartMoveAndRotation();
        gardenSound.SetActive(false);
        odanageldin.SetActive(true);
        kidHouse.SetActive(true);
        gardenSound.SetActive(false);
        camTransform(PepePortalCocukOdasi);
        Invoke("portalAc", 2f);
    }

    public void finishedDeprem()
    {
        gardenSound.SetActive(true);
        // kidHouse.SetActive(false);
        camTransform(PepePortalEv);
        finishSound.SetActive(true);
        tekrarBaslatPanel.SetActive(true);
        StopMoveAndRotation();
    }

    void ladderAc()
    {
        ladder1.SetActive(true);
        ladder2.SetActive(true);
    }

    void depremBasla()
    {
        //depremCanvas.SetActive(true);
        //depremFizikAnimator.GetComponent<Animator>().enabled = true;
        kartOyunuPortal.SetActive(true);
    }

    public void depremBasla2()
    {
        //depremCanvas.SetActive(true);
        depremFizikAnimator.GetComponent<Camera>().fieldOfView = 60f;
        GetComponent<Rigidbody>().mass = 0;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        dusecekEsyalar.GetComponent<OpenAnimationsOfChild>().enabled = true;
        ToggleTeleportMode();
        StartRotation();
        depremFizikAnimator.GetComponent<Animator>().enabled = true;
        //teleporttoMasaAlti
        SetPosition(new Vector3(-51.467f, 0.88f, -9.117f));
        SetRotation(new Vector3(0, 3.6f, 0));

        SetHeight(0);
        Invoke("depremDurdu", 10f);
    }

    void depremDurdu()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<Rigidbody>().mass = 1;
        GetComponent<CharacterController>().enabled = true;
        evdencikisPanel.SetActive(true);
        camTransform(kartOyunuPortal);
        ToggleTeleportMode();
        depremFizikAnimator.GetComponent<Animator>().enabled = false;
        // PepePortalCocukOdasi.SetActive(true);
        depremFizikAnimator.transform.localEulerAngles = new Vector3(0, 0, 0);
        depremDurduSound.SetActive(true);
        PepePortalCocukOdasi.SetActive(true);
        portalAc();
        ObjYerlestirme.gameObject.SetActive(false);
        kartOyunuPortal.gameObject.SetActive(false);
        duvaraCak1.gameObject.SetActive(false);
        duvaraCak2.gameObject.SetActive(false);
        guvenliYerPortal.gameObject.SetActive(false);
        CantaYapmaPortal.gameObject.SetActive(false);
        Bag.GetComponent<BoxCollider>().enabled = true;
        Sneakers.GetComponent<BoxCollider>().enabled = true;
    }

    private void StopMoveAndRotation()
    {
        _fpsController.walkSpeed = 0;
        _fpsController.lookSpeed = 0;
    }

    public void StartRotation(float value = 1)
    {
        _fpsController.lookSpeed = value;
    }

    public void StartMoveAndRotation()
    {
        SetRotation(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0));
        _fpsController.walkSpeed = 3;
        _fpsController.lookSpeed = 1;
        SetHeight(1.3f);
        depremFizikAnimator.GetComponent<Camera>().fieldOfView = 70f;
    }

    public void SetRotation(Vector3 newRotation)
    {
        transform.eulerAngles = newRotation;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(-55f, 1.76f, -8.85f);
        transform.eulerAngles = new Vector3(0, 55, 0);
    }

    public void StartObjeYerletirme()
    {
        HideAll();
        masayaYerlestirmeGiris.SetActive(true);
        camTransform(ObjYerlestirme);
        objYerlestir.SetActive(true);
        GetComponent<Teleport>().enabled = false;
        interactionCounter++;
        SetRotation(new Vector3(0, 143, 0));
        SetPosition(new Vector3(-52, 1.76f, -8.1f));
    }

    public void StartDuvaraCak1()
    {
        HideAll();
        Invoke("ladderAc", .5f);
        tabloCivi.SetActive(true);
        camTransform(duvaraCak1);
        SetPosition(new Vector3(-52.5f, 2.5f, -9.1f));
        SetRotation(new Vector3(-10, 180f, 0));
        SetHeight(2.7f);
        baget.SetActive(true);
        ladder1.SetActive(true);
        // drumSet.SetActive(true);
        ToggleTeleportMode();
        interactionCounter++;
    }

    public void StartDuvaraCak2()
    {
        HideAll();
        Invoke("ladderAc", .5f);
        potaCivi.SetActive(true);
        camTransform(duvaraCak2);
        SetPosition(new Vector3(-51.5f, 2.5f, -6f));
        SetRotation(new Vector3(-15, 0, 0));
        SetHeight(2.7f);
        baget.SetActive(true);
        ladder2.SetActive(true);
        // drumSet.SetActive(true);
        ToggleTeleportMode();
        interactionCounter++;
    }

    public void StartCantaYapma()
    {
        HideAll();
        camTransform(CantaYapmaPortal);
        portalParent.SetActive(false);
        CantaYapma.SetActive(true);
        SetRotation(new Vector3(0, 145, 0));
        this.GetComponent<Teleport>().enabled = false;
        cantaYapmaUI.SetActive(true);
        StartMoveAndRotation();
        interactionCounter++;
    }

    public void StartGuvenliYerSecme()
    {
        HideAll();
        guvenliYerSecme.SetActive(true);
        portalParent.SetActive(false);
        camTransform(guvenliYerPortal);
        this.GetComponent<Teleport>().enabled = false;
        SetRotation(new Vector3(35, 115, 0));
        SetHeight(4f);
        depremFizikAnimator.GetComponent<Camera>().fieldOfView = 100f;
        interactionCounter++;
    }

    public void StartKartOyunu()
    {
        HideAll();
        depremFizikAnimator.GetComponent<Animator>().enabled = false;
        depremFizikAnimator.transform.localEulerAngles = new Vector3(0, 0, 0);
        kartOyunuBaslangic.SetActive(true);
        camTransform(kartOyunuPortal);
        kartOyunu.SetActive(true);
        this.GetComponent<Teleport>().enabled = false;
        SetPosition(new Vector3(-51.05f, 1.5f, -7.81f));
        SetRotation(new Vector3(0, 127.1f, 0));
        StartRotation(0.3f);
        //ToggleTeleportMode();
    }

    public void HideAll()
    {
        depremHazirligiPanel.GetComponent<Animator>().Play("Hide");
        guvenliYerSecmePanel.GetComponent<Animator>().Play("Hide");
        cantaYapmaPanel.GetComponent<Animator>().Play("Hide");
        duvaraCak2Panel.GetComponent<Animator>().Play("Hide");
        duvaraCak1Panel.GetComponent<Animator>().Play("Hide");
        objeYerlestirmePanel.GetComponent<Animator>().Play("Hide");

        TebriklerDuvaraCak.GetComponent<Animator>().Play("Hide");
        TebriklerObjeYerlestirme.GetComponent<Animator>().Play("Hide");
        TebriklerGuvenliYerSecme.GetComponent<Animator>().Play("Hide");
        TebriklerCantaYapma.GetComponent<Animator>().Play("Hide");
        kartlarPanel.GetComponent<Animator>().Play("Hide");
        evdencikisPanel.GetComponent<Animator>().Play("Hide");

        Invoke("HideGOs", 1f);
    }


    public void HideGOs()
    {
        objeYerlestirmePanel.gameObject.SetActive(false);
        duvaraCak2Panel.gameObject.SetActive(false);
        cantaYapmaPanel.gameObject.SetActive(false);
        guvenliYerSecmePanel.gameObject.SetActive(false);
        depremHazirligiPanel.gameObject.SetActive(false);
        duvaraCak1Panel.gameObject.SetActive(false);

        TebriklerObjeYerlestirme.SetActive(false);
        TebriklerDuvaraCak.SetActive(false);
        TebriklerCantaYapma.SetActive(false);
        TebriklerGuvenliYerSecme.SetActive(false);

        kartlarPanel.gameObject.SetActive(false);
        evdencikisPanel.gameObject.SetActive(false);
    }

    public void ShowTebriklerObjeYerlestirme()
    {
        StopMoveAndRotation();
        TebriklerObjeYerlestirme.SetActive(true);
        PanelShow.PlayOneShot(Win);

    }

    public void ShowTebriklerDuvaraCak()
    {
        StopMoveAndRotation();
        SetPosition(new Vector3(transform.position.x, 1.41f, transform.position.z));
        SetHeight(1.3f);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        TebriklerDuvaraCak.SetActive(true);
        PanelShow.PlayOneShot(Win);

    }

    public void ShowTebriklerCantaYapma()
    {
        StopMoveAndRotation();
        TebriklerCantaYapma.SetActive(true);
        PanelShow.PlayOneShot(Win);

    }

    public void ShowTebriklerGuvenliYerSecme()
    {
        StopMoveAndRotation();
        TebriklerGuvenliYerSecme.SetActive(true);
        PanelShow.PlayOneShot(Win);

    }

    public void SetHeight(float height)
    {
        GetComponent<CharacterController>().height = height;
    }

    public void LateRotation()
    {
        SetRotation(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0));
    }

    public void AddLeaveObject()
    {
        _objectsBeforeLeave += 1;
        if (_objectsBeforeLeave >= 2)
        {
            var child = PepePortalCocukOdasi.transform.GetChild(0);
            child.GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}