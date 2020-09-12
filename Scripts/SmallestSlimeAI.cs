using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SmallestSlimeAI : MonoBehaviour
{
    public Transform playerPrefab;
    public float speed = 200f;
    public float nextWaypointDistance = 2;
    public float jumpCD = 0f;
    public float enemyHP;
    public HitStop hitStop;
    public Material matWhite;
    private Material matDefault;
    SpriteRenderer sr;
    public int onHitDamage;
    AIDestinationSetter AI;
    bool dead;
    public float jumpWaitTime;
    public EnemyAI dad;


    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public Player player;


    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        AI = GetComponent<AIDestinationSetter>();
        playerPrefab = GameObject.Find("Player").transform;
        hitStop = FindObjectOfType<HitStop>();
        player = FindObjectOfType<Player>();
        AI.target = GameObject.Find("Player").transform;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        matDefault = sr.material;

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }


    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, playerPrefab.position, onPathComplete);
    }

    void onPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyHP <= 0 && dead == false)
        {
            StartCoroutine("Death");
            dead = true;
        }

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime;

        animator.SetFloat("JumpCD", jumpCD);

        jumpCD += Time.deltaTime;

        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Jumping"))
        {
            rb.AddForce(force);
            StartCoroutine("StopJump");
        }
       

        if (jumpCD >= 2.5f)
        {
            jumpCD = 0f;
        }



        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Arrow")
        {
            if (col.gameObject.tag == "Arrow")
            {
                enemyHP -= 20f;
                Invoke("ResetMaterial", .1f);
                hitStop.Stop(0.1f);
                if (player.yellowHealth > player.currentHealth)
                {
                    player.currentHealth += 3;
                }
                if (!dead)
                {
                    sr.material = matWhite;
                    Invoke("ResetMaterial", .3f);
                }
            }
        }
        if (col.gameObject.tag == "FireArrow")
        {
            for (int i = 0; i < 4; i++)
            {
                enemyHP -= 10f;
                hitStop.Stop(0.001f);
                if (player.yellowHealth > player.currentHealth)
                {
                    player.currentHealth += 3;
                }
                if (!dead)
                {
                    sr.material = matWhite;
                    Invoke("ResetMaterial", .3f);
                }
            }
        }
        if (col.gameObject.tag == "Player" && !dead)
        {
            player.DamagePlayer(onHitDamage);
            hitStop.Stop(.01f);
            dad.hits += 1;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && dead)
        {
            player.DamagePlayer(1);
        }
    }

    void ResetMaterial()
    {
        sr.material = matDefault;
    }


    IEnumerator StopJump()
    {
        yield return new WaitForSeconds(jumpWaitTime);
        rb.velocity = Vector2.zero;
    }

    IEnumerator Death()
    {
        animator.SetTrigger("LargeSlime");
        speed = 0f;
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Bubbling");
        yield return new WaitForSeconds(.1f);
        transform.localScale += new Vector3(7,7,7);
        yield return new WaitForSeconds(1f);
        gameObject.tag = "Puddle";
    }
}
