using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    
    private bool timeStarted = false;
    private bool pause = false;

    private float runningStartTime = 0;
    private float totalElapsedPausedTime = 0;
    private float pauseStartTime = 0;
    private float elapsedPausedTime = 0;
    private float elapsedRunningTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeStarted) {
            elapsedRunningTime = Time.time - runningStartTime - totalElapsedPausedTime;
        } else if (pause) {
            elapsedPausedTime = Time.time - pauseStartTime;
        }
    }

    public void startTimer() {
        if (!timeStarted && !pause) {
            runningStartTime = Time.time;
            timeStarted = true;
        }
    }

    public void stopTimer() {
        timeStarted = false;
    }

    public void pauseTimer() {
        if (timeStarted && !pause) {
            timeStarted = false;
            pauseStartTime = Time.time;
            pause = true;
        }
    }

    public void unpauseTimer() {
        if (!timeStarted && pause) {
            totalElapsedPausedTime += elapsedPausedTime;
            timeStarted = true;
            pause = false;
        }
    }

    public void resetTimer() {
        elapsedRunningTime = 0f;
        runningStartTime = 0f;
        pauseStartTime = 0f;
        elapsedPausedTime = 0f;
        totalElapsedPausedTime = 0f;
        pause = false;
        timeStarted = false;
    }

    public int GetMinutes() {
        return (int)(elapsedRunningTime / 60f);
    }

    public int GetSeconds() {
        return (int)(elapsedRunningTime);
    }

    public float GetMilliseconds() {
        return (float)(elapsedRunningTime - System.Math.Truncate(elapsedRunningTime));
    }

    public float GetRawElapsedTime() {
        return elapsedRunningTime;
    }
}
