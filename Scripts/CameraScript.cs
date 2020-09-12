using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraScript : MonoBehaviour
{
    //Camera Movement
    public Transform cam;
    public Transform player;
    public Transform Slime;
    public EnemyAI slime;
    Vector3 target, mousePos, shakeOffset;
    Vector3 refVel = Vector3.zero;
    float cameraDistance = 3.5f;
    public float smoothTime = 0.3f, zStart;

    //Camera Shake
    float shakeMag, shakeTimeEnd;
    Vector3 shakeVector;
    bool shaking;

    //Cam Border
    public Vector2 minPos;
    public Vector2 maxPos;

    public GameObject timeCont;

    public Animator anim;

    bool isFight;

    void Start()
    {
        target = player.position;
        zStart = transform.position.z;
    }


    void LateUpdate()
     {
        mousePos = CaptureMousePos();
        shakeOffset = UpdateShake();
        target = UpdateTargetPos();
        UpdateCameraPosition();

        transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, minPos.x, maxPos.x),
        Mathf.Clamp(transform.position.y, minPos.y, maxPos.y),
        Mathf.Clamp(transform.position.z, -10.0f, -10.0f));

        if (transform.position.y >= -1 && isFight == false)
        {
            DirShake(Slime.position - player.position, 1f, .7f);
            StartCoroutine("Fight");
        }
       
     }

    void FollowSlime()
    {
        DirShake(Slime.position, 10f, .01f);
    }

    Vector3 CaptureMousePos()
    {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        ret *= 2;
        ret -= Vector2.one;
        float max = 0.7f;
        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
        {
            ret = ret.normalized;
        }
        return ret;
    }

    Vector3 UpdateTargetPos ()
    {
        Vector3 mouseOffset = mousePos * cameraDistance;
        Vector3 ret = mouseOffset + player.position;
        ret += shakeOffset;
        ret.z = zStart;
        return ret;
    }

    void UpdateCameraPosition ()
    {   
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
        transform.position = tempPos;
    }

    public void DirShake (Vector3 direction, float magnitude, float length)
    {
        shaking = true;
        shakeVector = direction;
        shakeMag = magnitude;
        shakeTimeEnd = Time.time + length;
    }

    Vector4 UpdateShake()
    {
        if (!shaking || Time.time > shakeTimeEnd)
        {
            shaking = false;
            return Vector3.zero;
        }
        Vector3 tempOffset = shakeVector;
        tempOffset *= shakeMag;
        return tempOffset;
    }

    IEnumerator Fight()
    {
        yield return new WaitForSeconds(.2f);
        slime.target = player;
        anim.SetTrigger("SlideTrigger");
        timeCont.SetActive(true);
        minPos.y = -2f;
        isFight = true;
    }
}

