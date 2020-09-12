using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    public TimeController timeCont;
    public CrosshairAiming cross;
    public Button dodge;
    public TMP_Dropdown skill;
    public Player health;
    public int skillTwo = 2;
    public int skillOne = 1;
    public int skillZero = 0;
    public Transform firePrefab;
    public Transform firePoint;
    public float fireAmmountDecrease;
    bool isCreated;
    public Animator anim;
    public HealthBar healthBar;
    public int healNumber = 3;
    public int fireNumber = 1;
    public Animator animator;
    public Animator levelAnim;
    public GameObject fireMeter;
    public TMP_Text minutes;
    public TMP_Text hits;
    public EnemyAI Slime;
    public TMP_Text outcome;
    public TMP_Text gradeText;
    float timeCount;

    private void Start()
    {
        skill.value = 0;
        anim.GetComponent<Animator>();
    }

    private void Update()
    {
        if (health.currentHealth > 0)
        {
            timeCount += Time.unscaledDeltaTime;
        }
        if (skill.value == skillOne && healNumber >= 0)
        {
            SkillHealth();
        }
        if (skill.value == skillTwo && cross.fireAmmount > 0)
        {
            SkillFire();
        }
    }

    public void EndScreen (bool winLose)
    {
        if (winLose == true)
        {
            outcome.text = "You Won";
        }
        else
        {
            outcome.text = "You Lost";
        }
        hits.text = "You Were Hit " + Slime.hits + " Times";
        minutes.text = "You took " + Mathf.Round(timeCount / 60) + " Minutes";
        if (timeCount <= 2 && Slime.hits <= 2)
        {
            gradeText.text = "S";
        }
        else if (timeCount <= 3 && Slime.hits <= 5)
        {
            gradeText.text = "A";
        }
        else if (timeCount <= 4 && Slime.hits <= 10)
        {
            gradeText.text = "B";
        }
        else if (winLose == false)
        {
            gradeText.text = "D";
        }
        Time.timeScale = .001f;
    }

    public void SkillFire()
    {
        cross.quiverAmmount = 5;
        fireMeter.SetActive(true);
        cross.FireArrows();
        cross.SetFire();
        cross.fireAmmount -= fireAmmountDecrease;
        StartCoroutine("WaitForFireChange");
    }


    public void OnValueChanged()
    {
        if (skill.value == skillOne || skill.value == skillTwo)
        {
            StartCoroutine("TimeChange");
        }
    }

    public void SkillHealth()
    {
        health.currentHealth += 10 * Time.deltaTime;
        healthBar.SetHealth(health.currentHealth);
        StartCoroutine("waitForHealChange");
    }

    public void MinusHealNumber()
    {
        if (skill.value == skillOne)
        {
            healNumber = healNumber - 1;
        }
    }
    public void MinusFireNumber()
    {
        if (skill.value == skillTwo)
        {
            fireNumber = fireNumber - 1;
        }
    }

    public void MainMenu ()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    IEnumerator WaitForFireChange()
    {
        yield return new WaitForSeconds(10f);
        animator.SetTrigger("Trig");
        skill.value = skillZero;
        cross.fireAmmount = 0f;
        changeDDItemText("FireArrows                   x" + fireNumber, 2);
    }

    IEnumerator waitForHealChange()
    {
        yield return new WaitForSeconds(3f);
        skill.value = skillZero;
        yield return new WaitForSeconds(3f);
        changeDDItemText("Heal                              x" + healNumber, 1);
    }

    IEnumerator TimeChange()
    {
        yield return new WaitForSeconds(.003f);
        timeCont.timeUntilGrey = 15f;
        timeCont.greyLength += Time.unscaledDeltaTime * 10000;
    }

    public void OnAttackPress ()
    {
        timeCont.timeUntilGrey = 15f;
        timeCont.greyLength += Time.unscaledDeltaTime * 1000;
    }

    void changeDDItemText(string newText, int index)
    {
        if (index < skill.options.Count)
        {
            TMP_Dropdown.OptionData newItem = new TMP_Dropdown.OptionData(newText);
            skill.options.RemoveAt(index);
            skill.options.Insert(index, newItem);
        }
    }
}
