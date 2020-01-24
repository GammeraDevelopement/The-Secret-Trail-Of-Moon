using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess_Piece : MonoBehaviour
{
    public string position;
    public GameObject piezaDestructora;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision) {
        position = collision.gameObject.name;
    }

    private void OnTriggerEnter(Collider other) {
        position = other.name;
    }

    public string getPosition() {
        return position;
    }
}
