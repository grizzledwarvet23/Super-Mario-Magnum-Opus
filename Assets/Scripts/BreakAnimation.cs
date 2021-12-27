using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BreakAnimation : MonoBehaviour
{
    public AudioSource breakNoise;
    public GameObject brokenSprite;
    GridLayout grid;
    Vector3 hitPosition;
    Tilemap map;
    public GameObject particleSystem;

    private readonly int[] xDir = new int[]{0, 1, -1, 0, 0, 1, 1, -1, -1}; //readonly basically equals const
    private readonly int[] yDir = new int[] {0, 0, 0, 1, -1, 1, -1, 1, -1};
    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<GridLayout>();
        hitPosition = Vector3.zero;
        map = gameObject.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    

    public IEnumerator Hit(Vector3Int pos)
    {
        yield return new WaitForSeconds(0);
        gameObject.GetComponent<Tilemap>().SetTile(pos, null);
        breakNoise.Play();
        GameObject particles = Instantiate(particleSystem, new Vector3(grid.CellToWorld(pos).x + 1, grid.CellToWorld(pos).y, grid.CellToWorld(pos).z), particleSystem.transform.rotation);
        yield return new WaitForSeconds(1);
        Destroy(particles);


    }
    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.collider.gameObject.tag == "BlockDestroyer")
        {
            Animator animator = col.collider.transform.parent.gameObject.GetComponent<Move2D>().animator;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump_Animation") || !(col.collider.transform.parent.gameObject.GetComponent<Move2D>().isGrounded) && col.collider.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity.y >= 0)
            {
                //  Debug.Log("Yes");
                foreach (ContactPoint2D hit in col.contacts)
                {
                    hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                    hitPosition.y = hit.point.y - 0.01f * hit.normal.y;

                }
                if (gameObject.GetComponent<Tilemap>().GetTile(gameObject.GetComponent<Tilemap>().WorldToCell(hitPosition)) != null)
                    StartCoroutine(Hit(gameObject.GetComponent<Tilemap>().WorldToCell(hitPosition)));
                else
                {
                    Vector3Int cellCoords = gameObject.GetComponent<Tilemap>().WorldToCell(hitPosition);
                    if (map.GetTile(new Vector3Int(cellCoords.x, cellCoords.y + 1, cellCoords.z)) != null)
                    {
                        StartCoroutine(Hit(new Vector3Int(cellCoords.x, cellCoords.y + 1, cellCoords.z)));
                    }
                    else if (map.GetTile(new Vector3Int(cellCoords.x + 1, cellCoords.y + 1, cellCoords.z)) != null)
                    {
                        StartCoroutine(Hit(new Vector3Int(cellCoords.x + 1, cellCoords.y + 1, cellCoords.z)));
                    }
                    else if (map.GetTile(new Vector3Int(cellCoords.x - 1, cellCoords.y + 1, cellCoords.z)) != null)
                    {
                        StartCoroutine(Hit(new Vector3Int(cellCoords.x - 1, cellCoords.y + 1, cellCoords.z)));

                    }
                }
            }



        }
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
//        Debug.Log(col);
        if (col.name == "torpedo(Clone)" || col.name == "Explosion_Damaging(Clone)" || col.name == "Explosion_Damaging")
        {
            StartCoroutine(destroyMultiple(col));
        }
    }

    public IEnumerator destroyMultiple(Collider2D col)
    {
        yield return new WaitForSeconds(0);
        if (col != null)
        {
        //    Debug.Log(col.bounds.center);
            hitPosition.x = col.bounds.center.x;
            hitPosition.y = col.bounds.center.y;
            Vector3Int cellCoords = gameObject.GetComponent<Tilemap>().WorldToCell(hitPosition);
            List<Vector3Int> cells = new List<Vector3Int>();
            for (int x = 0; x < xDir.Length; x++)
            {
                if (map.GetTile(new Vector3Int(cellCoords.x + xDir[x], cellCoords.y + yDir[x], cellCoords.z)) != null)
                {
                    cells.Add(new Vector3Int(cellCoords.x + xDir[x], cellCoords.y + yDir[x], cellCoords.z));
                }
            }

            foreach (Vector3Int block in cells)
            {
                StartCoroutine(Hit(block));
            }

            if (col.name == "torpedo(Clone)")
            {
                yield return new WaitForSeconds(0.1f);
                if (col != null)
                {
                    col.GetComponent<Bullet>().instantiateEffect();
                    Destroy(col.gameObject);
                }
            }
        }

        
    }
}
