using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JBColored : MonoBehaviour, BossInterface
{
    public JBBoss JBMain;
    public Material defaultMaterial;
    public void onDamageTaken()
    {
        if (GetComponent<AIDamage>().health == 0)
        {
            GetComponent<SpriteRenderer>().material = defaultMaterial;
            transform.parent.gameObject.SetActive(false);
            JBMain.coloredJBCount--;
            if (JBMain.coloredJBCount <= 0)
            {
                JBMain.StopAllCoroutines();
                JBMain.reappear();      
            }
        }
    }
}
