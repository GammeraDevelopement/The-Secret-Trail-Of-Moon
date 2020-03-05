using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GNG_TargetMove: MonoBehaviour
{
    GNG_GameController gng;
    float speed = 10f;

    // Update is called once per frame

    private void Start() {
        gng = GameObject.FindGameObjectWithTag("GameController").GetComponent<GNG_GameController>();
        speed = gng.speed;
    }
    void Update()
    {
        transform.Translate(new Vector3(1 * -speed / 2  * Time.deltaTime * 2, 0, 0), Space.World);
    }
}
