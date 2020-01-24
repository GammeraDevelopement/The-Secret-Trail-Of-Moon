﻿using UnityEngine.UI;
using UnityEngine;

public class SokobanBlock : MonoBehaviour {

    public bool isColliding;

    private void OnTriggerEnter(Collider other) {

        if(other.name == "winningPlace") {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Sokoban>().sokobanWin();
        }

        if(other.tag == "block") {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        isColliding = false;
    }

    private void OnTriggerStay(Collider other) {
        isColliding = true;
    }

    private void OnCollisionEnter(Collision collision) {
        isColliding = true;
    }

    private void OnCollisionExit(Collision collision) {
        isColliding = false;
    }

}
