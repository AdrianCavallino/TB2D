using System.Collections;
using UnityEngine;

public class CharacterProperties : MonoBehaviour
{
    [SerializeField] private string characterName;
    [SerializeField] private int characterLevel;

    // Offensive stats
    [SerializeField] private int damage;

    // Defensive stats
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int defense;
    
    // Actions
    private bool _isGuarding;

    public string CharacterName
    {
        get => characterName;
        set => characterName = value;
    }

    public int CharacterLevel
    {
        get => characterLevel;
        set => characterLevel = value;
    }

    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public int CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    public int Defense
    {
        get => defense;
        set => defense = value;
    }

    public IEnumerator CharacterAttack(CharacterProperties target)
    {
        // Damage Logic
        bool isDead = target.TakeDamage(damage);

        if (isDead)
        {
            
        }
        else
        {
            
        }
        
        yield return new WaitForSeconds(3f);    
    }

    public IEnumerator CharacterGuard()
    {
        // Guard Logic


        yield return new WaitForSeconds(3f);
    }

    public bool TakeDamage(float inDamage)
    {
        float calcDamage = inDamage;
        if (_isGuarding)
        {
            calcDamage = inDamage - defense;
        }

        currentHealth -= Mathf.RoundToInt(calcDamage);

        if (currentHealth <= 0)
        {
            return true;
        }

        return false;
    }
}
