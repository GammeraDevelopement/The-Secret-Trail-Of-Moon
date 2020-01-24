using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAction : MonoBehaviour
{
    private float speed = 2.0F;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Action() {
        if (Input.GetButton("Fire1")) {
            transform.Rotate(Vector3.down * Time.deltaTime * speed);
        }else if (Input.GetButton("Fire2")) {
            transform.Rotate(Vector3.up * Time.deltaTime * speed);
        }
    }
}
