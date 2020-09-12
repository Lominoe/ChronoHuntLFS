using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 5f;


    void Update()
    {
        Vector3 movement = new Vector3 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"),0.0f);

        if (movement != Vector3.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            
        }
        animator.SetFloat("magnitude", movement.sqrMagnitude);

        transform.position = transform.position + movement * moveSpeed * Time.deltaTime;

    }


}
