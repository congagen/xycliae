using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisMgmt : MonoBehaviour
{

    public GameObject cursorObject;

    public bool showTrail = false;


    void toggleTrails() {
        showTrail = !showTrail;

        foreach (Transform t in cursorObject.transform) {
            t.GetComponent<TrailRenderer>().enabled = showTrail;
        }

    }


    void Update() {
        if (Input.GetKeyUp(KeyCode.T)) {
            toggleTrails();
        }
    }
}
