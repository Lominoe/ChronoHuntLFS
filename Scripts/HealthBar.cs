using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth (float Health)
    {
        slider.maxValue = Health;
    }
    
    public void SetHealth (float Health)
    {
        slider.value = Health;
    }


    
}
