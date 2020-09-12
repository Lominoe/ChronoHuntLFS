using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CrosshairAiming : MonoBehaviour
{
    public float arrowSpeed;
    public GameObject arrowPrefab;
    public GameObject firePrefab;
    public Transform firePoint;
    public Transform player;
    public float fixedDistance;
    public Transform fireSystemPoint;

    public KeyCode drawArrow;
    public float drawArrowTime = 0f;
    public float fireArrowTime;
    public CameraScript Cam;
    public Transform dashEffect;
    bool changed;

    public float fireAmmount = 100;

    public int quiverAmmount;
    public TMP_Text quiverCount;

    public Texture2D cursor;

    public UiController uI;

    public Material matWhite;
    private Material matDefault;
    public SpriteRenderer sr;
    public Animator anim;

    public Slider fireSlider;

    bool reloading;

    public void SetFire()
    {
        fireSlider.value = fireAmmount;
    }

    void Start()
    {
        SetFire();
        quiverCount = quiverCount.GetComponent<TMP_Text>();
        reloading = false;
        Cam = FindObjectOfType<CameraScript>();
        Vector2 cursorOffset = Vector2.zero;
        Cursor.SetCursor(cursor, cursorOffset, CursorMode.Auto);

        matDefault = sr.material;
    }

    private bool isMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }


    void Update()
    {
        {
            anim.SetBool("Reloading", reloading);
            if (quiverAmmount <= 0)
            {
                StartCoroutine("Reload");
            }

            if (quiverAmmount > 5)
            {
                quiverAmmount = 5;
            }

            Cursor.visible = true;
            quiverCount.text = quiverAmmount.ToString();

            if (Input.GetKey(drawArrow))
            {
                drawArrowTime += Time.deltaTime;
            }

            if (drawArrowTime >= 1 && quiverAmmount > 0 && !changed)
            {
                sr.material = matWhite;
                Invoke("ResetMaterial", .1f);
                changed = true;
            }
            if ((Input.GetKeyUp(drawArrow)) && (drawArrowTime > 1) && quiverAmmount >= 1 && uI.skill.value != uI.skillTwo)
            {
                NormalArrows();
                changed = false;
            }

            if ((Input.GetKeyUp(drawArrow)))
            {
                drawArrowTime = 0f;
            }
        }
    }


    public void FireArrows()
    {
        if (Input.GetKey(drawArrow))
        {
            fireArrowTime += Time.deltaTime;
        }

        if (drawArrowTime > 1 && quiverAmmount > 0)
        {
            sr.material = matWhite;
            Invoke("ResetMaterial", .1f);
        }

        if (fireAmmount >= 0 && Input.GetKeyUp(drawArrow) && fireArrowTime >= 1)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 1.0f;
            transform.position = position;
            GameObject fireArrow = Instantiate(firePrefab, firePoint.position, Quaternion.identity);
            ArrowScript arrowScript = fireArrow.GetComponent<ArrowScript>();
            arrowScript.rb.velocity = (position - player.position).normalized * arrowSpeed;
            fireArrow.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(position.y - player.position.y, position.x - player.position.x) * Mathf.Rad2Deg);
            Destroy(fireArrow, 3f);
            Transform DashEffect = Instantiate(dashEffect, firePoint.position, firePoint.rotation) as Transform;
            DashEffect.transform.parent = fireArrow.transform;
            Destroy(DashEffect.gameObject, 5f);
            fireArrowTime = 0;
            Cam.DirShake(-(position - player.position), 3f, 0.2f);
            drawArrowTime = 0f;
            quiverCount.text = quiverAmmount.ToString();
        }

        if ((Input.GetKeyUp(drawArrow)))
        {
            fireArrowTime = 0f;
        }

    }

    public void NormalArrows()
    {
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 1.0f;
            transform.position = position;
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            ArrowScript ArrowScript = arrow.GetComponent<ArrowScript>();
            ArrowScript.rb.velocity = (position - player.position).normalized * arrowSpeed;
            arrow.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(position.y - player.position.y, position.x - player.position.x) * Mathf.Rad2Deg);
            drawArrowTime = 0;
            Destroy(arrow, 2f);
            Cam.DirShake(-(position - player.position), 3f, .01f);
            ArrowShot(1);
            quiverCount.text = quiverAmmount.ToString();
        }

    }
    public void ArrowShot(int shot)
    {
        quiverAmmount -= shot;
    }
    void ResetMaterial()
    {
        sr.material = matDefault;
    }
    IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(3f);
        quiverAmmount = 5;
        reloading = false;
    }
    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
    }
}

