using System.Collections;
using Unity.VisualScripting;
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

    public bool IsGuarding
    {
        get => _isGuarding;
        set => _isGuarding = value;
    }
    
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
    
    public IEnumerator CharacterGuard()
    {
        // Guard Logic


        yield return new WaitForSeconds(3f);
    }

    public bool TakeDamage(int inDamage)
    {
        int calcDamage = inDamage;

        if (_isGuarding)
        {
            calcDamage -= defense;
            if (calcDamage < 0)
            {
                calcDamage = 0;
            }
        }

        currentHealth -= calcDamage;

        EventBus.Broadcast("GetHit");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            return true; // Character is dead
        }

        return false; // Character is still alive
    }
}
