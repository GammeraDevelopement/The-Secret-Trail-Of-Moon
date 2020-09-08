using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoviAnimationController : MonoBehaviour
{
    private const string ANIMATION_SIT = "Sit";
    private const string ANIMATION_SLEEP = "Sleep";
    private const string ANIMATION_WALK = "Walk";
    private const string ANIMATION_RUN = "Run";
    private const string ANIMATION_TALK = "Talk";
    private const string ANIMATION_THINK = "Think";

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void Sit() {

        anim.SetBool(ANIMATION_SIT, true);
    }

    public void StandUp() {

        anim.SetBool(ANIMATION_SIT, false);
    }

    public void Sleep() {

        anim.SetBool(ANIMATION_SLEEP, true);
    }

    public void NoSleep() {

        anim.SetBool(ANIMATION_SLEEP, false);
    }

    public void Walk() {

        anim.SetBool(ANIMATION_WALK, true);
    }

    public void NoWalk() {

        anim.SetBool(ANIMATION_WALK, false);
    }

    public void Run() {

        anim.SetBool(ANIMATION_RUN, true);
    }

    public void NoRun() {

        anim.SetBool(ANIMATION_RUN, false);
    }

    public void Talk() {

        anim.SetBool(ANIMATION_TALK, true);
    }

    public void NoTalk() {
        anim.SetBool(ANIMATION_TALK, false);
    }

    public void Think() {

        anim.SetBool(ANIMATION_THINK, true);
    }

    public void NoThink() {

        anim.SetBool(ANIMATION_THINK, false);
    }
}
