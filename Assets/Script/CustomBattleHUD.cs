using System;
using UnityEngine;
using UnityEngine.UI;

public class CustomBattleHUD : MonoBehaviour
{
    [SerializeField] private Text characterName;
    [SerializeField] private Text levelText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text defenseText;

    public void setHUD(CharacterProperties charProps)
    {
        characterName.text = charProps.CharacterName;
        levelText.text = "Lv." + charProps.CharacterLevel;
        healthSlider.maxValue = charProps.MaxHealth;
        healthSlider.value = charProps.CurrentHealth;
        defenseText.text = charProps.Defense.ToString();
    }

    void SetHp(float hp)
    {
        healthSlider.value =Mathf.RoundToInt(hp);
    }
}
