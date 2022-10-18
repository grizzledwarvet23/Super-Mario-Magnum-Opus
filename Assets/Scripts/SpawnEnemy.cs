using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [System.Serializable]
    public struct enemyParentPair
    {
        public GameObject enemy;
        public GameObject parent;
    }
    public enemyParentPair[] pair;

    public Transform launchPos;
    public Vector2 launchSpeed;
    // Start is called before the first frame update


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.gameObject.CompareTag("platform") && collision.gameObject.name.Equals("sand_boat"))
        {
            
            StartCoroutine(spawnEnemies());
            
        }
        else if(collision.gameObject.name.Equals("JBplatform"))
        {
            
            StartCoroutine(launchEnemies());
        }
        
        
        
    }

    IEnumerator spawnEnemies()
    {
        gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        foreach (enemyParentPair p in pair)
        {
            GameObject enemy;
            if (p.parent == null)
            {
                enemy = Instantiate(p.enemy, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0), gameObject.transform.rotation);
            }
            else
            {
                enemy = Instantiate(p.enemy, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0), gameObject.transform.rotation, p.parent.transform);
            }
            if (enemy.GetComponent<Patrol>() != null)
            {
                enemy.GetComponent<Patrol>().waitForMovingPlat = true;
            }
            yield return new WaitForSeconds(1f);
        }
        gameObject.SetActive(false);
    }

    IEnumerator launchEnemies()
    {
        gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        foreach(enemyParentPair p in pair)
        {
            if (p.enemy.GetComponent<Magikoopa>() != null) //level 4. done so it is consistent with color changes
            {
                p.enemy.SetActive(true);
                if (p.enemy.name.Contains("red"))
                {
                    p.enemy.GetComponent<Magikoopa>().setVulnerability(PanelSwitcher.redOn);
                }
                else if (p.enemy.name.Contains("green"))
                {
                    p.enemy.GetComponent<Magikoopa>().setVulnerability(PanelSwitcher.greenOn);
                }
                else if (p.enemy.name.Contains("blue"))
                {
                    p.enemy.GetComponent<Magikoopa>().setVulnerability(PanelSwitcher.blueOn);
                }
                
                p.enemy.GetComponent<Magikoopa>().canFlip = false;
                p.enemy.GetComponent<Rigidbody2D>().velocity = launchSpeed;
                StartCoroutine(enablePatrol(p.enemy));
            }
            else
            {
                GameObject enemy = Instantiate(p.enemy, launchPos.position, launchPos.rotation, p.parent.transform);
                enemy.GetComponent<Patrol>().enabled = false;

                enemy.GetComponent<Rigidbody2D>().velocity = launchSpeed;
                StartCoroutine(enablePatrol(enemy));
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator enablePatrol(GameObject enemy)
    {
        yield return new WaitForSeconds(1f);
        if (enemy != null)
        {
            if (enemy.GetComponent<Magikoopa>() != null)
            {
                enemy.GetComponent<Magikoopa>().canFlip = true;
            }
            else
            {
                enemy.GetComponent<Patrol>().enabled = true;
            }
        }        
    }


}


