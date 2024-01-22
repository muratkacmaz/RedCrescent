using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public bool isBreakable;
    public bool isRotating;

    public float rotationSpeed_up;
    public float rotationSpeed_left;

    Vector3 startPosition;

    public bool canPlace;

    public int collideWith = 0;
    public ObjectPlacement ObjectPlacement;
    public Camera Camera;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
        canPlace = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "FlowingObject")
        {
            isRotating = true;
        }
        else if (gameObject.tag == "PlacedObject")
        {
            isRotating = false;
        }

        if (isRotating == true)
        {
            RotationOfObjects();
        }
        else if (isRotating == false)
        {
            transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);
            transform.localRotation = Quaternion.AngleAxis(0, Vector3.left);
        }
    }

    public void RotationOfObjects()
    {
        transform.localRotation *= Quaternion.AngleAxis(rotationSpeed_up * Time.deltaTime, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationSpeed_left * Time.deltaTime, Vector3.left);
    }

    public void placeBack()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        transform.position = startPosition;
        Invoke("OpenCollider", 1f);
    }

    public void OpenCollider()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AltRaf")
        {
            collideWith = -1;
            ObjectPlacement.DropFlowingObject(gameObject);
        }
        else if (other.gameObject.tag == "UstRaf")
        {
            collideWith = 1;
            ObjectPlacement.DropFlowingObject(gameObject);
        }
        else
            collideWith = 0;
    }

    private Vector3 mOffset;


    private float mZCoord;


    void OnMouseDown()

    {
        if (gameObject.GetComponent<BoxCollider>().enabled)
        {
            mZCoord = Camera.WorldToScreenPoint(
                gameObject.transform.position).z;


            // Store offset = gameobject world pos - mouse world pos

            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
            ObjectPlacement.HoldFlowingObject(gameObject);
        }
    }


    private Vector3 GetMouseAsWorldPoint()

    {
        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;


        // z coordinate of game object on screen

        mousePoint.z = mZCoord;


        // Convert it to world points

        return Camera.ScreenToWorldPoint(mousePoint);
    }


    void OnMouseDrag()
    {
        if (gameObject.GetComponent<BoxCollider>().enabled)
        {
            transform.position = GetMouseAsWorldPoint() + mOffset;
        }
    }

    private void OnMouseUp()
    {
        if (!gameObject.CompareTag("PlacedObject"))
        {
            placeBack();
        }
    }
}