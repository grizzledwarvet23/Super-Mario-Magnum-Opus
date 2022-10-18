using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    private GameObject player;
    private ParticleSystem ps;

    private void Start()
    {
        player = GameObject.Find("player1");
        ps = GetComponent<ParticleSystem>();
        ps.trigger.SetCollider(0, player.GetComponent<BoxCollider2D>());
    }
    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> list = new List<ParticleSystem.Particle>();
        ParticlePhysicsExtensions.GetTriggerParticles(ps, ParticleSystemTriggerEventType.Inside, list);
        if (list.Count > 0)
        {
            player.GetComponent<PlayerHealth>().takeDamage(10);
        }
    }
}
