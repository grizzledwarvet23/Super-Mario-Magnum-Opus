using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class PanelSwitcher : MonoBehaviour
{
    public GameObject redPanels;
    public GameObject greenPanels;
    public GameObject bluePanels;

    public GameObject redSpikes;
    public GameObject greenSpikes;
    public GameObject blueSpikes;

    private int count = 0;

    public Image colorwheel;

    public bool on = false;

    public LayerMask excludeCollisionMask;

    private LayerMask OG_red;
    private LayerMask OG_green;
    private LayerMask OG_blue;

    public GameObject redEnemies;
    public GameObject greenEnemies;
    public GameObject blueEnemies;
    private void Start()
    {
        OG_red = redPanels.GetComponent<PlatformEffector2D>().colliderMask;
        OG_green = greenPanels.GetComponent<PlatformEffector2D>().colliderMask;
        OG_blue = bluePanels.GetComponent<PlatformEffector2D>().colliderMask;
        changePanelMap(true, false, false);

        greenSpikes.GetComponent<PlatformEffector2D>().colliderMask = ~excludeCollisionMask;
        blueSpikes.GetComponent<PlatformEffector2D>().colliderMask = ~excludeCollisionMask;
    }

    public void Switch()
    {
        if (on)
        {
            count++;
            count %= 3;
            switch (count)
            {
                case 0:
                    changePanelMap(true, false, false);
                    colorwheel.rectTransform.anchoredPosition = new Vector2(341, 176);
                    StartCoroutine(rotate());
                    
                    break;
                case 1:
                    changePanelMap(false, true, false);
                    colorwheel.rectTransform.anchoredPosition = new Vector2(338.6f, 176);
                    StartCoroutine(rotate());
                    
                    break;
                case 2:
                    changePanelMap(false, false, true);
                    colorwheel.rectTransform.anchoredPosition = new Vector2(344.5f, 176);
                    StartCoroutine(rotate());
                    
                    break;
            }
        }
    }

    void changePanelMap(bool red, bool green, bool blue)
    {
        if (red)
        {
            redPanels.GetComponent<PlatformEffector2D>().colliderMask = OG_red;
            greenPanels.GetComponent<PlatformEffector2D>().colliderMask = ~excludeCollisionMask;
            bluePanels.GetComponent<PlatformEffector2D>().colliderMask = ~excludeCollisionMask;

            redPanels.layer = 8; //ground
            greenPanels.layer = 2;
            bluePanels.layer = 2;

            foreach(Magikoopa spr in redEnemies.GetComponentsInChildren<Magikoopa>())
            {
                spr.setVulnerability(true);
            }
            foreach (Magikoopa spr in greenEnemies.GetComponentsInChildren<Magikoopa>())
            {
                spr.setVulnerability(false);
            }
            foreach (Magikoopa spr in blueEnemies.GetComponentsInChildren<Magikoopa>())
            {
                spr.setVulnerability(false);
            }

            redSpikes.GetComponent<PlatformEffector2D>().colliderMask = OG_red;
            blueSpikes.GetComponent<PlatformEffector2D>().colliderMask = ~excludeCollisionMask;

            redSpikes.GetComponent<TilemapRenderer>().enabled = true;
            blueSpikes.GetComponent<TilemapRenderer>().enabled = false;

            redSpikes.layer = 4;
            blueSpikes.layer = 2;
        }
        else if(green)
        {
            redPanels.GetComponent<PlatformEffector2D>().colliderMask = ~excludeCollisionMask;
            greenPanels.GetComponent<PlatformEffector2D>().colliderMask = OG_green;
            bluePanels.GetComponent<PlatformEffector2D>().colliderMask = ~excludeCollisionMask;

            redPanels.layer = 2; //ground
            greenPanels.layer = 8;
            bluePanels.layer = 2;

            foreach (Magikoopa spr in greenEnemies.GetComponentsInChildren<Magikoopa>())
            {
                spr.setVulnerability(true);
            }
            foreach (Magikoopa spr in redEnemies.GetComponentsInChildren<Magikoopa>())
            {
                spr.setVulnerability(false);
            }
            foreach (Magikoopa spr in blueEnemies.GetComponentsInChildren<Magikoopa>())
            {
                spr.setVulnerability(false);
            }
            greenSpikes.GetComponent<PlatformEffector2D>().colliderMask = OG_green;
            redSpikes.GetComponent<PlatformEffector2D>().colliderMask = ~excludeCollisionMask;

            greenSpikes.GetComponent<TilemapRenderer>().enabled = true;
            redSpikes.GetComponent<TilemapRenderer>().enabled = false;

            greenSpikes.layer = 4;
            redSpikes.layer = 2;



        }
        else if(blue)
        {
            redPanels.GetComponent<PlatformEffector2D>().colliderMask = ~excludeCollisionMask;
            greenPanels.GetComponent<PlatformEffector2D>().colliderMask = ~excludeCollisionMask;
            bluePanels.GetComponent<PlatformEffector2D>().colliderMask = OG_blue;

            redPanels.layer = 2; //ground
            greenPanels.layer = 2;
            bluePanels.layer = 8;

            foreach (Magikoopa spr in blueEnemies.GetComponentsInChildren<Magikoopa>())
            {
                spr.setVulnerability(true);
            }
            foreach (Magikoopa spr in greenEnemies.GetComponentsInChildren<Magikoopa>())
            {
                spr.setVulnerability(false);
            }
            foreach (Magikoopa spr in redEnemies.GetComponentsInChildren<Magikoopa>())
            {
                spr.setVulnerability(false);
            }
            

            blueSpikes.GetComponent<PlatformEffector2D>().colliderMask = OG_blue;
            greenSpikes.GetComponent<PlatformEffector2D>().colliderMask = ~excludeCollisionMask;

            blueSpikes.GetComponent<TilemapRenderer>().enabled = true;
            greenSpikes.GetComponent<TilemapRenderer>().enabled = false;

            blueSpikes.layer = 4;
            greenSpikes.layer = 2;
        }
        redPanels.GetComponent<TilemapRenderer>().enabled = red;
        greenPanels.GetComponent<TilemapRenderer>().enabled = green;
        bluePanels.GetComponent<TilemapRenderer>().enabled = blue;
    }

    

    IEnumerator rotate()
    {
        for(int x = 0; x < 10; x++)
        {
            colorwheel.transform.eulerAngles = new Vector3(colorwheel.transform.eulerAngles.x, colorwheel.transform.eulerAngles.y, colorwheel.transform.eulerAngles.z + 12);
            yield return new WaitForSeconds(0.03f);
        }
    }

    
}
