using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TamerUIController : MonoBehaviour
{
    public TimeController timeCont;
    public Button dodge;
    public TMP_Dropdown skill;
    public Player health;
    public int skillTwo = 2;
    public int skillOne = 1;
    public int skillZero = 0;
    public Animator anim;
    public HealthBar healthBar;
    bool healed;

    private void Start()
    {
        healed = false;
        skill.value = 0;
        anim.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        SkillHealth();
    }


    public void OnValueChanged()
    {
        if (skill.value == skillOne || skill.value == skillTwo)
        {
            timeCont.timeUntilGrey = 15f;
            timeCont.greyLength += Time.unscaledDeltaTime * 10000;
        }
    }

    public void SkillHealth()
    {
        if (skill.value == skillOne && healed == false)
        {
            health.currentHealth += 1;
            healthBar.SetHealth(health.currentHealth);
            StartCoroutine("waitForHealChange");
        }
    }


    IEnumerator waitForHealChange()
    {
        yield return new WaitForSeconds(.7f);
        skill.value = skillZero;
        yield return new WaitForSeconds(3f);
        changeDDItemText("Heal                              x0", 1);
        healed = true;

    }

    public void OnAttackPress()
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

