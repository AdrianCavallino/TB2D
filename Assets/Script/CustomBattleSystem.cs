using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public enum CombatState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Win,
    Lose
}
public class CustomBattleSystem : MonoBehaviour
{
    private CombatState _state;

    [SerializeField] private Transform playerLocation;
    [SerializeField] private Transform enemyLocation;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private CustomBattleHUD playerHUD;
    [SerializeField] private CustomBattleHUD enemyHUD;

    [SerializeField] private GameObject actionButtons;

    private CharacterProperties _playerInfo;
    private CharacterProperties _enemyInfo;
    
    [SerializeField] Text dialogueText;

    public CombatState State
    {
        get => _state;
        set => _state = value;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _state = CombatState.Start;
        StartCoroutine(Setup());
    }

    IEnumerator Setup()
    {
        GameObject playerObject = Instantiate(playerPrefab, playerLocation);
        _playerInfo = playerObject.GetComponent<CharacterProperties>();
        
        GameObject enemyObject = Instantiate(enemyPrefab, enemyLocation);
        _enemyInfo = enemyObject.GetComponent<CharacterProperties>();

        dialogueText.text = "Encountered Angry " + _enemyInfo.CharacterName;
        
        playerHUD.setHUD(_playerInfo);
        enemyHUD.setHUD(_enemyInfo);
        
        yield return new WaitForSeconds(3f);

        _state = (_playerInfo.Defense > _enemyInfo.Defense) ? CombatState.PlayerTurn : CombatState.EnemyTurn;
        if (_state == CombatState.PlayerTurn)
        {
            PlayerTurn();
        }
        else if (_state == CombatState.EnemyTurn)
        {
            EnemyTurn();
        }
    }

    void PlayerTurn()
    {
        actionButtons.SetActive(true);
        dialogueText.text = "Choose action: ";
    }

    public void OnAttack()
    {
        if (_state == CombatState.PlayerTurn)
        {
            StartCoroutine(PlayerAttack());
        }
        
        actionButtons.SetActive(false);
    }

    public void OnGuard()
    {
        if (_state == CombatState.PlayerTurn)
        {
            StartCoroutine(PlayerGuard());
        }
    }

    IEnumerator PlayerGuard()
    {
        _playerInfo.IsGuarding = true;
        dialogueText.text = _playerInfo.CharacterName + " is guarding!";
        actionButtons.SetActive(false);

        yield return new WaitForSeconds(2f);

        _state = CombatState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerAttack()
    {
        _playerInfo.IsGuarding = false;
        bool isDead = _enemyInfo.TakeDamage(_playerInfo.Damage);
        
        enemyHUD.SetHp(_enemyInfo.CurrentHealth);
        dialogueText.text = (!_enemyInfo.IsGuarding) ? "DIRECT HIT INTO " + _enemyInfo.CharacterName : "Attack guarded by " + _enemyInfo.CharacterName;

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            _state = CombatState.Win;
            EndBattle();
        }
        else
        {
            _state = CombatState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
        
        yield return new WaitForSeconds(2f);
    }

    private void EndBattle()
    {
        if (_state == CombatState.Win)
        {
            actionButtons.SetActive(false);
            dialogueText.text = "Victory is yours!";
        } else if (_state == CombatState.Lose)
        {
            actionButtons.SetActive(false);
            dialogueText.text = "Ugh, we'll get him next time";
        }
    }

    IEnumerator EnemyTurn()
    {
        _enemyInfo.IsGuarding = false;
        actionButtons.SetActive(false);
        dialogueText.text = "Enemy Action";

        yield return new WaitForSeconds(1f);

        float randomValue = UnityEngine.Random.value;

        if (_enemyInfo.CurrentHealth > _enemyInfo.MaxHealth * 0.4f)
        {
            if (randomValue <= 0.7f)
            {
                // Enemy attacks
                dialogueText.text = _enemyInfo.CharacterName + " attacks!";
                bool isDead = _playerInfo.TakeDamage(_enemyInfo.Damage);
                playerHUD.SetHp(_playerInfo.CurrentHealth);

                if (isDead)
                {
                    _state = CombatState.Lose;
                    EndBattle();
                    yield break;
                }
            }
            else
            {
                // Enemy blocks
                dialogueText.text = _enemyInfo.CharacterName + " is guarding!";
                _enemyInfo.IsGuarding = true;
            }
        }
        else
        {
            if (randomValue <= 0.3f)
            {
                // Enemy attacks
                dialogueText.text = _enemyInfo.CharacterName + " attacks!";
                bool isDead = _playerInfo.TakeDamage(_enemyInfo.Damage);
                playerHUD.SetHp(_playerInfo.CurrentHealth);

                if (isDead)
                {
                    _state = CombatState.Lose;
                    EndBattle();
                    yield break;
                }
            }
            else
            {
                // Enemy blocks
                dialogueText.text = _enemyInfo.CharacterName + " is guarding!";
                _enemyInfo.IsGuarding = true;
            }
        }

        yield return new WaitForSeconds(1f);

        _state = CombatState.PlayerTurn;
        PlayerTurn();
    }
}
