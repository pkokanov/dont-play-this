using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text timerText;
    private float startTime;
    private bool timerRunning;
    private float pauseTime;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        timerRunning = true;
        pauseTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (timerRunning)
        {
            float t = Time.time - startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");
            timerText.text = minutes + ":" + seconds;
        }
	}

    public void ResetTimer()
    {
        startTime = Time.time;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
        pauseTime = Time.time;
    }

    public void ResumeTimer()
    {
        pauseTime = Time.time - pauseTime;
        startTime += pauseTime;
        timerRunning = true;
    }
 }
