using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Timer", menuName = "CVeinStudio/Game System/Timer")]
public class SO_Timer : ScriptableObject
{
    private float _elapsedTime = 0;
    private float seconds;

    internal string Tick()
    {
        _elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(_elapsedTime / 60F);
        seconds += Time.deltaTime;

        if (seconds >= 60)
        {
            seconds = 0;
        }
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
