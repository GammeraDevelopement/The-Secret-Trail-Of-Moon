using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{
    void Activate() {
        gameObject.SetActive(true);
    }

    void Deactivate() {
        gameObject.SetActive(false);
    }
}
