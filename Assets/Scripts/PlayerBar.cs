using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BarType
{
    healthBar,
    manaBar
}

public class PlayerBar : MonoBehaviour
{
    public BarType type;

    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();

        switch (this.type)
        {
            case BarType.healthBar:
                slider.maxValue = PlayerController.MAX_HEALTH;
                break;
            case BarType.manaBar:
                slider.maxValue = PlayerController.MAX_MANA;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.type)
        {
            case BarType.healthBar:
                slider.value = GameObject.Find("Player").GetComponent<PlayerController>().GetHealth();  
                break;
            case BarType.manaBar:
                slider.value = GameObject.Find("Player").GetComponent<PlayerController>().GetMana();
                break;
        }
         
    }
}
