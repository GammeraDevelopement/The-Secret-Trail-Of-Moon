using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enigma_Notch : MonoBehaviour
{
    private string nombre = "9";
    private GameObject objetivo;

    public int getTargetName() {
        nombre = nombre.Substring(nombre.Length - 1);
        //Debug.Log("nombre:" + nombre);
        return int.Parse(nombre);
    }

    public GameObject getTarget() {
        return objetivo;
    }

    private void OnCollisionEnter(Collision collision) {
        objetivo = collision.gameObject;
        for (int i = 0; i < 8; i++) {
            if (collision.collider.name == "Target" + i) {
                nombre = collision.collider.name;
            }
        }
        
    }


}
