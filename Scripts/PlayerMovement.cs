using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed;
    public float drawArrowTime = 0f;
    public KeyCode drawArrow;
    public float coolDown = 2f;
    public float slideTime = 2f;
    public Transform player;
    public float dashSpeed;
    public float dsMult;
    public Transform dashEffect;
    public Transform slideEffect;
    public Transform walkPoint;
    public Transform dodgePoint;
    public Player stats;
    private Rigidbody2D rb2D;
    public bool isMoving;
    public ParticleSystem walking;
    public ParticleSystem sliding;
    public CameraScript cam;
    public float accel;
    public float maxSpeed;
    public Slider staminaBar;
    public float stamina = 60;

    public void Start()
    {
        walking.Play();
        sliding.Stop();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private Vector2 movement = Vector3.zero;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isMoving == true && stamina > 10)
        {
            StartCoroutine("DashMove");
            stamina = stamina - 20;
            SetStamina();
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            isMoving = true;
            if (moveSpeed < maxSpeed)
            {
                moveSpeed += accel * Time.deltaTime;
            }
        }
        else
        {
            isMoving = false;
            if (moveSpeed > maxSpeed)
            {
                moveSpeed -= accel * Time.deltaTime;
            }
        }
        if (stamina < 60)
        {
            stamina += 5f * Time.deltaTime;
            SetStamina();
        }
        if (moveSpeed > 6)
        {
            moveSpeed -= dsMult * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            dsMult = 5;
        }
        else
        {
            dsMult = 8;
        }

        stats = GetComponent<Player>();

        Vector3 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        animator = GetComponent<Animator>();


        if (isMoving == false)
        {
            walking.Play();
        }

        if (slideTime >= 2)
        {
            sliding.Stop();
        }

        //animation
        if (movement != Vector3.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Magnitude", movement.sqrMagnitude);
        animator.SetFloat("AimHorizontal", position.x - player.position.x);
        animator.SetFloat("AimVertical", position.y - player.position.y);
        animator.SetFloat("AimMagnitude", position.sqrMagnitude);
        animator.SetBool("AimDown", Input.GetMouseButton(0));
        animator.SetFloat("AimTime", drawArrowTime);
        // End of Animation Stuff

        if (Input.GetKey(drawArrow))
        {
            drawArrowTime += Time.deltaTime;
        }

        if ((Input.GetKeyUp(drawArrow)) && (drawArrowTime > 1))
        {
            drawArrowTime = 0;
        }

        // End of CoolDown Stuff

        // Slide stuff
        if (Input.GetKey(drawArrow))
        {
            slideTime -= Time.deltaTime;
            if (slideTime < 2)
            {
                if (isMoving)
                {
                    sliding.Play();
                    moveSpeed -= 10 * Time.deltaTime;
                }
            }

            if (slideTime < 1.3f)
            {
                sliding.Stop();
                moveSpeed = 0f;
            }
        }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
        {
            sliding.Stop();
            slideTime = 2f;
            moveSpeed = 5;
        }
        // End of Slide Stuff


        if (stats.currentHealth <= 0)
        {
            rb2D.velocity = Vector2.zero;
            return;
        }

        rb2D.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);


    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Dirt")
        {
            maxSpeed = 7f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        maxSpeed = 5f;
    }


    IEnumerator DashMove()
    {
        animator.SetTrigger("OnDodgeDown");
        yield return new WaitForSeconds(.05f);
        moveSpeed *= dashSpeed * dsMult * Time.fixedDeltaTime;
        yield return new WaitForSeconds(.1f);
        cam.DirShake(Vector3.down, 20f, .1f);
        yield return new WaitForSeconds(.2f);
        moveSpeed = 7;
    }

    void SetStamina()
    {
        staminaBar.value = stamina;
    }


}
