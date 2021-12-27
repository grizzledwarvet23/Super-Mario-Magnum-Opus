using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    public AudioSource jump;
    public AudioSource fire;
    public AudioSource kill;
    public AudioSource coin;
    public bool disabled;
    // Start is called before the first frame update
 
    public void PlayJump()
    {
        jump.Play();
    }
    public void FirePlay()
    {
        fire.Play();
    }
    public void KillPlay()
    {
        kill.Play();
    }
    public void CoinPlay()
    {
        coin.Play();
       
    }
    /**
     *  First track is disabled, second one in array is enabled
     */
    public static void trackShift(AudioSource[] tracks) {
        if (tracks[0] != null && tracks[1] != null)
        {
            tracks[0].enabled = false;
            tracks[1].enabled = true;
        }
    
    }

    //to fade music
    public static IEnumerator startFade(AudioSource source, float duration, float targetVol)
    {
        float currentTime = 0;
        float start = source.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            source.volume = Mathf.Lerp(start, targetVol, currentTime / duration);
            yield return null;
        }
        yield break;
    }

}
