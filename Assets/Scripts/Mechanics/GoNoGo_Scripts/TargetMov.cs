using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMov : MonoBehaviour
{
    GoNoGo gng;
    /// <summary>
    /// Misma variable que en GoNoGo
    /// </summary>
    float speed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(1 * -speed, 0, 0) * Time.deltaTime;
    }
}
