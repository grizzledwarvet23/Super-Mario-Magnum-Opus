using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AIDamage : MonoBehaviour
{

    public Slider healthbar;

    public int health = 100;
    private int maxHealth;
    public GameObject obj; //THE ONE TO DESTROY. AKA THE ENEMY

    public bool nonPrefabDeathEffect;
    public GameObject deathEffect;
    public GameObject deathEffect2;

    public Transform deathEffectPos;

    public AudioSource deathSound;
    public AudioSource soundToStop;
    public string soundToStopIndex;
    public GameObject victoryMario;

    public GameObject oldCamera;  //ONLY USE THESE VARIABLES IN CONJUCTION WITH GameCutsceneManager
    public GameObject bossCamera;

    public SoundControl PlayerSound;
    private SpriteRenderer renderer;
    Color originalColor;
    [SerializeField]


    private Material matWhite;
    private Material matDefault;

    public float flashSpeed;

    public GameObject damagedPrefab;

    public GameObject hitEffect;

    public GameObject[] deadzoneToRemove;

    void Start()
    {
        maxHealth = health;
        if (GameObject.Find("player1") != null)
        {
            PlayerSound = GameObject.Find("player1").GetComponent<SoundControl>();
        }
        renderer = gameObject.GetComponent<SpriteRenderer>();
        originalColor = renderer.color;

        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = renderer.material;

        if (soundToStop == null && soundToStopIndex != null && soundToStopIndex.Length > 0)
        {
            try
            {
                soundToStop = GameObject.Find("Music").GetComponents<AudioSource>()[int.Parse(soundToStopIndex)];
            }
            catch { Debug.Log("Sound to stop assignment has failed");  }
        }

    }
    public void TakeDamage(int damage)
    {   
        health -= damage;
        if (healthbar != null)
        {
            healthbar.value = ( (health * 100) / maxHealth);
         //   healthbar.value -= (damage * (100) / maxHealth);
        }
        StartCoroutine(WhiteFlash(flashSpeed));
        renderer.material = matWhite;

        if (gameObject.GetComponent<BossInterface>() != null)
        {
            gameObject.GetComponent<BossInterface>().onDamageTaken();
        }

        if (health <= 0)
        {
            Die();
        }
    }


    // Update is called once per frame
    void Die()
    {
        if (PlayerSound != null && !PlayerSound.disabled)
        {
            PlayerSound.KillPlay();
        }

        if (deathEffect != null)
        {

            GameObject effect; // = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Vector3 pos;
            if (deathEffectPos != null)
            {
                pos = deathEffectPos.position;
            }
            else
            {
                pos = transform.position;
            }

            if (!nonPrefabDeathEffect)
            {
                effect = Instantiate(deathEffect, pos, Quaternion.identity);
            }
            else
            {
                effect = deathEffect;
                effect.transform.position = pos;
                effect.SetActive(true);
            }
            
            
            //if a boss
            if (effect.GetComponent<GameCutsceneManager>() != null && victoryMario != null) {
                effect.GetComponent<GameCutsceneManager>().victoryMario = victoryMario;
                foreach(GameObject obj in deadzoneToRemove)
                {
                    obj.SetActive(false);
                }
                if(bossCamera != null)
                {
                    effect.GetComponent<GameCutsceneManager>().bossCamera = bossCamera;
                    effect.GetComponent<GameCutsceneManager>().oldCamera = oldCamera;
                }
            }
            if (deathSound != null) {
                deathSound.Play();
            }
            if (soundToStop != null)
            {
                soundToStop.Stop();
            }


        }
        if (deathEffect2 != null)
        {
            Instantiate(deathEffect2, transform.position, Quaternion.identity);
        }
        Destroy(obj);
    }

    public void playHitEffect()
    {
        if (hitEffect != null)
        {
            StartCoroutine(hitEffectActivation());
        }
        else {
            Debug.Log(gameObject.name + " doesn't have hitEffect!");
        }




    }
    IEnumerator hitEffectActivation() {
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        hitEffect.SetActive(false);


    }

    private IEnumerator WhiteFlash(float flashSpeed)
    {
        renderer.material = matWhite;
        yield return new WaitForSeconds(flashSpeed);
        renderer.material = matDefault;

    }

}
