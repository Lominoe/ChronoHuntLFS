using UnityEngine;
using System.Collections;
using Pathfinding;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    public float jumpCD = 0f;
    public float enemyHP;
    public float sizeChange;
    public HitStop hitStop;
    public Material matWhite;
    private Material matDefault;
    SpriteRenderer sr;
    public int onHitDamage;
    AIDestinationSetter AI;
    public ParticleSystem fire;
    public GameObject smallSlimeOne;
    float slimeShake = 2;
    bool slimeOneSpawned;
    bool slimeTwooSpawned;
    bool hasDuped;
    public GameObject smallSlimeTwo;
    bool dead;
    public float jumpWaitTime;
    public float roamRadius;
    public Camera cam;
    public Rigidbody2D arrow;

    public Player player;
    public Animator animator;
    // What to chase?
    public Transform target;
    Seeker seeker;
    Rigidbody2D rb;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    public float nextWaypointDistance = 2;
    public float speed;
    public float LaserTime;
    public GameObject laser;
    bool laserFired;
    bool Lasered;
    public int hits;


    void Start()
    {
        laserFired = false;
        fire.Stop();
        sr = GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        UpdatePath();
        matDefault = sr.material;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, onPathComplete);
    }

    void onPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void FixedUpdate()
    {
        animator.SetBool("SlimeLaser", Lasered);
        if (cam.transform.position.y > -3)
        {
            LaserTime += Time.deltaTime;
        }
        if (LaserTime >= 15 && !dead)
        {
            StartCoroutine("FireLaser");
            LaserTime = 0f;
        }
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        Vector2 randomForce = randomDirection * speed * Time.deltaTime;
        if (cam.transform.position.y <= -5 && this.animator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Jumping"))
        {
            rb.AddForce(randomForce);
        }

        if (enemyHP <= 400)
        {
            if (slimeOneSpawned == false)
            {
                StartCoroutine("SlimeShake");
                slimeOneSpawned = true;
            }
            if (slimeTwooSpawned == false)
            {
                StartCoroutine("SpawnWait");
                slimeTwooSpawned = true;
                hasDuped = true;
            }
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


        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Jumping") && cam.transform.position.y > -3)
        {
            rb.AddForce(force);
            StartCoroutine("StopJump");
        }
        else
        {
            rb.velocity = Vector2.zero;
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

        if (enemyHP <= 0 && !dead)
        {
            StartCoroutine("Death");
            dead = true;
        }

        animator.SetFloat("JumpCD", jumpCD);


        if (jumpCD >= 2.5f)
        {
            jumpCD = 0f;
        }

    }

    IEnumerator FireLaser ()
    {
        Lasered = true;
        yield return new WaitForSeconds(1.1f);
        Lasered = false;
        yield return new WaitForSeconds(.7f);
        laser.SetActive(true);
        float angle = AngleBetweenTwoPoints(laser.transform.position, target.transform.position) - Random.Range(70, 100);
        if (!laserFired)
        {
            animator.SetFloat("Angle", angle);
            laser.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
        yield return new WaitForSeconds(.001f);
        laserFired = true;
        yield return new WaitForSeconds(.5f);
        laser.SetActive(false);
        LaserTime = 0f;
        yield return new WaitForSeconds(1f);
        laserFired = false;
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Arrow")
        {
            enemyHP -= 20f;
            transform.localScale -= new Vector3(sizeChange, sizeChange, 0);
            hitStop.Stop(0.1f);
            if (!dead)
            {
                sr.material = matWhite;
                Invoke("ResetMaterial", .3f);
            }
            if (player.yellowHealth > player.currentHealth)
            {
                player.currentHealth += 5;
            }
            col.transform.parent = gameObject.transform;
            arrow.isKinematic = true;
            arrow.velocity = Vector2.zero;
            col.enabled = !col.enabled;
            Destroy(col.gameObject, .1f);
        }
        if (col.gameObject.tag == "FireArrow")
        {
            for (int i = 0; i < 4; i++)
            {
                fire.Play();
                enemyHP -= 10f;
                transform.localScale -= new Vector3(sizeChange / 10, sizeChange / 10, 0);
                hitStop.Stop(0.001f);
                StartCoroutine("FireStop");
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
            hits += 1;
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
        transform.localScale += new Vector3(7, 7, 7);
        yield return new WaitForSeconds(1f);
        gameObject.tag = "Puddle";
    }

    IEnumerator SlimeShake()
    {
        slimeShake -= Time.deltaTime;
        if (slimeShake >= 2)
        {
            slimeShake = 0;
        }
        yield return new WaitForSeconds(2);
        smallSlimeOne.SetActive(true);
        smallSlimeOne.transform.parent = null;
    }

    IEnumerator SpawnWait()
    {
        yield return new WaitForSeconds(3f);
        smallSlimeTwo.SetActive(true);
        smallSlimeTwo.transform.parent = null;
    }

    IEnumerator FireStop()
    {
        yield return new WaitForSeconds(3f);
        fire.Stop();
    }
}
