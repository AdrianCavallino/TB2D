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
        if(_state != CombatState.PlayerTurn) return;

        StartCoroutine(_playerInfo.CharacterAttack(_enemyInfo));
    }

    void EnemyTurn()
    {
        actionButtons.SetActive(false);
        dialogueText.text = "Enemy Action";
    }
}
