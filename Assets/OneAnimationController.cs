using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneAnimationController : MonoBehaviour

{
    private const string ANIMATION_IDLE = "IdleSit";
    private const string ANIMATION_WALK = "Walk";
    private const string ANIMATION_SLEEP = "Sleep";
    private const string ANIMATION_RUN = "Run";
    private const string ANIMATION_IDLE2 = "Idle2";
    private const string ANIMATION_STRECH = "Strech";
    private const string ANIMATION_SNIFF = "Sniff";
    private const string ANIMATION_TALK = "Talk";


    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    //TODO cuando se llame al metodo reproducir la animacion

    public void Walk() {

        anim.SetBool(ANIMATION_WALK, true);
    }

    public void NoWalk() {

        anim.SetBool(ANIMATION_WALK, false);
    }

    public void Sleep() {

        anim.SetBool(ANIMATION_SLEEP, true);
    }

    public void NoSleep() {

        anim.SetBool(ANIMATION_SLEEP, false);
    }

    public void Run() {

        anim.SetBool(ANIMATION_RUN, true);
    }

    public void NoRun() {

        anim.SetBool(ANIMATION_RUN, false);
    }

    public void Idle2() {

        anim.SetBool(ANIMATION_IDLE2, true);
    }

    public void NoIdle2() {

        anim.SetBool(ANIMATION_IDLE2, false);
    }

    public void Sit() {

        anim.SetBool(ANIMATION_IDLE, true);
    }

    public void StandUp() {

        anim.SetBool(ANIMATION_IDLE, false);
    }

    public void Strech() {

        anim.SetTrigger(ANIMATION_STRECH);
    }

    public void Sniff() {

        anim.SetTrigger(ANIMATION_SNIFF);
    }

    public void Talk() {

        anim.SetBool(ANIMATION_TALK, true);
    }

    public void NoTalk() {

        anim.SetBool(ANIMATION_TALK, false);
    }
}
