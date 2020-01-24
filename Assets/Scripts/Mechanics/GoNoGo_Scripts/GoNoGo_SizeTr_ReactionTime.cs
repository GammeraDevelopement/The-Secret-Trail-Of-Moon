using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoNoGo_SizeTr_ReactionTime : MonoBehaviour
{

    /// <summary>
    /// Referencia del script GoNoGo_SetData
    /// </summary>
    GoNoGo gng;

    BoxCollider C_Box;

    //[SerializeField] string ejeColliderCambia;

    // Start is called before the first frame update
    //Estos calculos los hacemos en el start del GoNoGo y luego le pasamos directamente la posición del centro y el tamaño en Z que debe tener.
    void Start()
    {
        gng = FindObjectOfType<GoNoGo>();

        C_Box = GetComponent<BoxCollider>();

        C_Box.size = new Vector3(C_Box.size.x, C_Box.size.y, gng.sizeZ);
        C_Box.center = new Vector3(C_Box.center.x, C_Box.center.y, -gng.sizeZ / 2);

        /*
        if(ejeColliderCambia == "z")
        {
            C_Box.size = new Vector3(C_Box.size.x, C_Box.size.y, gng.sizeZ);
            C_Box.center = new Vector3(C_Box.center.x, C_Box.center.y, -gng.sizeZ / 2);
        }
        else if (ejeColliderCambia == "y")
        {
            C_Box.size = new Vector3(C_Box.size.x, C_Box.size.y, gng.sizeZ);
            C_Box.center = new Vector3(C_Box.center.x, C_Box.center.y, -gng.sizeZ / 2);
        }
        else if (ejeColliderCambia == "x")
        {
            C_Box.size = new Vector3(C_Box.size.x, C_Box.size.y, gng.sizeZ);
            C_Box.center = new Vector3(C_Box.center.x, C_Box.center.y, -gng.sizeZ / 2);
        }
        */
    }

}
