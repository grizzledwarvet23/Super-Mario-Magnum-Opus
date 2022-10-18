using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class LightningEffect : MonoBehaviour
{
    public bool done;
    public Light2D[] lights;
    public AudioSource lightningSound;

    public ParticleSystem rainGenerator;
    public AudioSource ambientRain;

    public SpriteRenderer playerSprite;
    public Tilemap[] maps;
    public SpriteRenderer[] otherSprites;

    private float player_RGB = 0.7f;
    private float tilemap_RGB = 0.7f;


    public void Start()
    {
        done = false;
        playerSprite.gameObject.GetComponent<Weapon>().lightningOn = true;
        playerSprite.color = new Color(player_RGB, player_RGB, player_RGB);
        foreach (Tilemap map in maps)
        { 
            map.color = new Color(tilemap_RGB, tilemap_RGB, tilemap_RGB);
        }
        StartCoroutine(lightningStrike());
    }

    public IEnumerator lightningStrike()
    {
        float time = Random.Range(3f, 6.5f);
        yield return new WaitForSeconds(time);
        if (!done)
        {
            Light2D light = lights[Random.Range(0, lights.Length)];
            lightningSound.Stop(); //if already playing
            lightningSound.pitch = Random.RandomRange(0.6f, 1f);
            lightningSound.volume = Random.RandomRange(0.6f, 0.8f);

            light.intensity = 500;

            float p_color = Mathf.Min(player_RGB, playerSprite.color.r);

            playerSprite.color = Color.black;
            foreach (Tilemap map in maps)
            {
                map.color = Color.black;
            }
            foreach (SpriteRenderer renderer in otherSprites)
            {
                if (renderer != null)
                {
                    if (renderer.enabled)
                    {
                        renderer.color = Color.black;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
            lightningSound.Play();
            light.intensity = 30;

            for (int i = 0; i <= 30; i++)
            {
                light.intensity--;

                float playerColorVal = Mathf.Min(p_color, (i / 30.0f) * p_color);
                float tilemapColorVal = Mathf.Min(tilemap_RGB, (i / 30.0f) * tilemap_RGB);
                playerSprite.color = new Color(playerColorVal, playerColorVal, playerColorVal);
                foreach (Tilemap map in maps)
                {
                    map.color = new Color(tilemapColorVal, tilemapColorVal, tilemapColorVal);
                }
                foreach (SpriteRenderer renderer in otherSprites)
                {
                    if (renderer != null)
                    {
                        if (renderer.enabled)
                        {
                            renderer.color = new Color(playerColorVal, playerColorVal, playerColorVal);
                        }
                    }
                }
                yield return new WaitForSeconds(0.02f);
            }
            light.intensity = 0;
            StartCoroutine(lightningStrike());
        }
    }

    public void stopLightning()
    {
        done = true;
        rainGenerator.Stop();
        StartCoroutine(SoundControl.startFade(ambientRain, 2, 0));
    }

    private void OnEnable()
    {
        playerSprite.gameObject.GetComponent<Weapon>().lightningOn = true;
        playerSprite.color = new Color(player_RGB, player_RGB, player_RGB);

        foreach (Tilemap map in maps)
        { 
            map.color = new Color(tilemap_RGB, tilemap_RGB, tilemap_RGB);
        }
    }

    private void OnDisable()
    {
        if (playerSprite != null)
        {
            playerSprite.gameObject.GetComponent<Weapon>().lightningOn = false;
            playerSprite.color = Color.white;
        }
        if (maps != null)
        {
            foreach (Tilemap map in maps)
            {
                if (map != null)
                {
                    map.color = Color.white;
                }
            }
        }
    }




}
