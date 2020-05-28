using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneAnimationController : MonoBehaviour

{
    private const string ANIMATION_IDLE = "IdleSit";
    private const string ANIMATION_WALK = "Walk";


    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        bool walk = false;

        
        
    }

    //TODO cuando se llame al metodo reproducir la animacion

    public void Caminar() {

        anim.SetBool(ANIMATION_WALK, true);
    }
}
