using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{
    public Vector2 velocity;
    public GameObject archer;
    public Vector2 offset = new Vector2(0f, 0f);
    public Rigidbody2D rb;
    public CapsuleCollider2D col;
    public Vector2 currentPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
    }

    void fixedUpdate()
    {
        print("arrow");
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPosition = currentPosition + velocity * Time.deltaTime;

        RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition + offset, newPosition + offset);

        foreach (RaycastHit2D hit in hits)
        {
            GameObject other = hit.collider.gameObject;

                if (other.CompareTag("Interactables"))
                {
                    Destroy(gameObject);
                    break;
                }
                if (other.CompareTag("Enemy"))
                {
                    rb.isKinematic = true;
                    transform.parent = other.gameObject.transform;
                    velocity = Vector2.zero;
                    StartCoroutine("DestroyArrow");
                }

        }

        currentPosition = newPosition;
    }

    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
    }
    }