using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnoffallBaget : MonoBehaviour {
    public GameObject baget1, baget2, baget3, baget4, baget5, baget6, baget7, baget8;
    public void turnoffAllBaget() {
        baget1.SetActive(false);
        baget2.SetActive(false);
        baget3.SetActive(false);
        baget4.SetActive(false);
        baget5.SetActive(false);
        baget6.SetActive(false);
        baget7.SetActive(false);
        baget8.SetActive(false);
    }
}
