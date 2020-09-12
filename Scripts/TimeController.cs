using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeController : MonoBehaviour
{
    public Image greyScreen;
    public Transform greyScreenPoint;
    public float slowDownFactor = 0.01f;
    public float speedUpLength = 1f;
    public float greyLength;
    public float timeUntilGrey;
    public Animator animator;
    public Animator anim;
    public Animator hourGlass;
    public bool isGreyScreen;
    public Button dodgeRoll;
    bool functionCalled;
    float functionTimer;
    public SpriteRenderer sr;
    public Material matBlue;
    private Material matDefault;
    public UiController ui;

    void Start()
    {
        greyLength = 0f;
        StartCoroutine("SlowMotion");
        animator.GetComponent<Animator>();
        matDefault = sr.material;
    }

    private void Update()
    {
        animator.SetBool("IsGreyScreen", isGreyScreen);
        anim.SetFloat("GreyLength", greyLength);
        hourGlass.SetFloat("Blue", timeUntilGrey);
        hourGlass.SetFloat("Orange", greyLength);
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        if (functionCalled == true)
        {
            functionTimer += Time.deltaTime;
            if (functionTimer >= .1)
            {
                functionCalled = false;
                functionTimer = 0f;
            }
        }

        //TIMERS
        if (timeUntilGrey <= 30)
        {
            timeUntilGrey += Time.unscaledDeltaTime;
        }

        if (timeUntilGrey >= 30)
        {
            timeUntilGrey = 30f;
        }

        if(greyLength <= 10)
        {
            greyLength += Time.unscaledDeltaTime;
        }

        if (greyLength >= 10)
        {
            hourGlass.SetTrigger("OrangeFlip");
            greyLength = 10f;
        }
        //END TIMERS


        //CALL FUNCTIONS
        SlowDown();
        SpeedUp();
    }

    public void SlowDown ()
    { 
        if (!functionCalled && timeUntilGrey >= 30 && Input.GetKey(KeyCode.E))
        {
            hourGlass.SetTrigger("BlueFlip");
            StartCoroutine("SlowMotion");
            timeUntilGrey = 2f;
            functionCalled = true;
        }
    }

    public void SpeedUp ()
    {
        if (!functionCalled && greyLength >= 10 && timeUntilGrey >= 14f)
        {
            Time.timeScale += 1f / speedUpLength;
            isGreyScreen = false;
            functionCalled = true;
        }
    }

    IEnumerator SlowMotion ()
    {
        hourGlass.SetTrigger("Start");
        isGreyScreen = true;
        yield return new WaitForSeconds(.5f);
        Time.timeScale = slowDownFactor;
        greyLength = 0f;
        timeUntilGrey = 5f;
    }

}
