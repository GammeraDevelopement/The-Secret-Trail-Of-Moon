using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kuburi_FaceDetector : MonoBehaviour {

    
    public float interactionDistance = 3;

    private string actualSolution;

    private void Update() {

        Debug.DrawRay(transform.position, -transform.forward * interactionDistance);
        if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit, interactionDistance)) {
            //Debug.Log(hit.collider.name);
            if (hit.collider.name.Contains("Cara")) {
                GameObject colision = hit.collider.gameObject;
                actualSolution = colision.name.Substring(4);
            }
        }
     
    }

    public string getCara() {
        return actualSolution;
    }
}
    

