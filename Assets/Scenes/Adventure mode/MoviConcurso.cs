using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoviConcurso : MonoBehaviour
{
    public MoviAnimationController mac;

    // Start is called before the first frame update
    void Start()
    {
        mac.Sit();
        mac.Sleep();
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
