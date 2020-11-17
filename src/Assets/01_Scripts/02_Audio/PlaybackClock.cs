using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackClock : MonoBehaviour
{

    public decimal playbackPositionOffset = 0;

    public decimal playbackPosition = 0;
    public string playbackPositionDataKey = "playbackPosition";

    private bool flip = false;

    void Update()
    {
        if (Mathf.Abs((float)playbackPosition) > 999) {
            playbackPosition = 999;
        }


        if (PlayerPrefs.GetInt("play") == 1)
        {
            if (playbackPosition < 999 && !flip)
            {
                playbackPosition += (decimal)(Time.deltaTime * 0.01f);
                PlayerPrefs.SetFloat(playbackPositionDataKey, (float)playbackPosition);
            }
            else
            {
                flip = playbackPosition > 1;
                playbackPosition -= (decimal)(Time.deltaTime * 0.01f);
                PlayerPrefs.SetFloat(playbackPositionDataKey, (float)playbackPosition);
            }
        }
    }

}
