using System.Collections.Generic;
using MHLab.InfectionsBlaster.Extensions;
using MHLab.SlayTheOrc.Decks;
using MHLab.SlayTheOrc.Decks.Cards;
using MHLab.SlayTheOrc.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace MHLab.SlayTheOrc
{
    public sealed class Monster : MonoBehaviour
    {
        public List<Sprite> Enemies;
        private SpriteRenderer _spriteRenderer;
        
        private AudioSource _audioSource;
        public List<AudioClip> HitClips;
        public List<AudioClip> ReflectClips;
        public List<AudioClip> ShieldClips;
        
        public int MaxHP;
        public int CurrentHP;
        
        public Tooltip DamageTooltip;
        public Resourcebar HPBar;

        public Tooltip ShieldTooltip;
        public Tooltip ReflectTooltip;

        public EnemyTierSetter TierSetter;
        
        public MonsterNextMove NextMoveComponent;
        public Deck Deck;
        public Battle Battle;

        private Transform _monsterTransform;
        private Vector3 _monsterInitialPosition;
        private BattleAction _currentBattleAction;
        private bool _isPerforming;

        private bool _isAttacking = false;
        private bool _isReturningFromAttack = false;
        
        private float _speed = 1;

        private int _shieldValue;
        private int _reflectValue;
        
        private bool _isDead = false;

        private float _deathTimer;
        private const float DeathTime = 2f;

        private float _timerBeforeAction = 0f;
        private const float TimeBeforeAction = 1f;

        private void Awake()
        {
            _monsterTransform = transform;
            _monsterInitialPosition = _monsterTransform.position;
            _currentBattleAction = new BattleAction();
            _audioSource = gameObject.GetComponentNoAlloc<AudioSource>();

            _spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

            _isPerforming = false;

            GenerateMonsterType();
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name != "Boss")
            {
                MaxHP = MonsterManager.LastGeneratedMonster.MaxHP;
                CurrentHP = MaxHP;
                TierSetter.SetTier(MonsterManager.LastGeneratedMonster.Tier);
            }

            HPBar.SetValue(CurrentHP, MaxHP);
        }

        private void GenerateMonsterType()
        {
            if (SceneManager.GetActiveScene().name != "Boss")
            {
                _spriteRenderer.sprite = Enemies[Random.Range(0, Enemies.Count)];
                MonsterManager.GenerateMonsterInfo();
            }
        }

        public void ApplyDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP < 0) CurrentHP = 0;
            
            HPBar.SetValue(CurrentHP, MaxHP);
            
            if (damage > 0)
            {
                EZCameraShake.CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
            }
            
            if (CurrentHP == 0)
            {
                Battle.FinishBattle();
                _speed = 0.1f;
                _isDead = true;
                NextMoveComponent.gameObject.SetActive(false);
            }
        }

        public int GetShieldValue()
        {
            return _shieldValue;
        }

        public int GetReflectValue()
        {
            return _reflectValue;
        }

        public void SetNextMove()
        {
            var playerCards = Deck.GetCurrentCards();
            var selectedCard = playerCards[Random.Range(0, playerCards.Count)];

            switch (selectedCard)
            {
                case AttackCard attack:
                    ReactToAttackMove(attack);
                    break;
                case ShieldCard shield:
                    ReactToShieldMove(shield);
                    break;
                case ReflectCard reflect:
                    ReactToReflectMove(reflect);
                    break;
            }
            
            NextMoveComponent.SetMove(_currentBattleAction);
        }

        private void ReactToAttackMove(in AttackCard attack)
        {
            if (Random.value <= 0.5f)
            {
                _currentBattleAction.Type = ActionType.Shield;
            }
            else
            {
                _currentBattleAction.Type = ActionType.Reflect;
            }

            var newValue = Random.Range(attack.Value - 3, attack.Value + 2);
            if (newValue <= 0) newValue = 1;
            
            _currentBattleAction.Value = newValue;
        }

        private void ReactToShieldMove(in ShieldCard shield)
        {
            _currentBattleAction.Type = ActionType.Attack;
            var newValue = Random.Range(shield.Value - 3, shield.Value + 2);
            if (newValue <= 0) newValue = 1;
            
            _currentBattleAction.Value = newValue;
        }
        
        private void ReactToReflectMove(in ReflectCard attack)
        {
            if (Random.value <= 0.33f)
            {
                _currentBattleAction.Type = ActionType.Shield;
            }
            else if (Random.value <= 0.66f)
            {
                _currentBattleAction.Type = ActionType.Reflect;
            }
            else
            {
                _currentBattleAction.Type = ActionType.Attack;
            }

            var newValue = Random.Range(attack.Value - 3, attack.Value + 2);
            if (newValue <= 0) newValue = 1;
            
            _currentBattleAction.Value = newValue;
        }

        public void PerformBattleAction()
        {
            _isPerforming = true;

            switch (_currentBattleAction.Type)
            {
                case ActionType.Attack:
                    _isAttacking = true;
                    _isReturningFromAttack = false;
                    break;
            }
        }

        private void Update()
        {
            if (_isDead)
            {
                HandleDeath();
                _deathTimer += Time.deltaTime;

                if (_deathTimer >= DeathTime)
                {
                    if (SceneManager.GetActiveScene().name != "Boss")
                    {
                        SceneManager.LoadScene("Loot");
                    }
                    else
                    {
                        SceneManager.LoadScene("EndGame");
                    }
                }

                return;
            }
            
            if (_isPerforming)
            {
                if (_timerBeforeAction < TimeBeforeAction)
                {
                    _timerBeforeAction += Time.deltaTime;
                    return;
                }
                
                switch (_currentBattleAction.Type)
                {
                    case ActionType.Attack:
                        if (_isAttacking) HandleAttack();
                        if (_isReturningFromAttack) HandleReturnFromAttack();
                        break;
                    case ActionType.Shield:
                        HandleShield();
                        break;
                    case ActionType.Reflect:
                        HandleReflect();
                        break;
                }
            }
        }

        private void HandleAttack()
        {
            var monsterPosition = _monsterTransform.position;
            var targetPosition = new Vector3(Battle.Player.transform.position.x, monsterPosition.y, monsterPosition.z);

            var direction = targetPosition - monsterPosition;

            var velocity = monsterPosition + (direction.normalized * (Time.deltaTime * _speed));

            _monsterTransform.position = velocity;

            _speed += 0.3f;

            if (direction.magnitude <= 0.1f)
            {
                _speed = 1;
                _isAttacking = false;
                _isReturningFromAttack = true;
                _audioSource.PlayOneShot(HitClips[Random.Range(0, HitClips.Count)]);

                var damage = _currentBattleAction.Value;
                var shieldValue = Battle.Player.GetShieldValue();
                var reflectValue = Battle.Player.GetReflectValue();

                if (shieldValue > 0)
                {
                    var actualDamage = damage - Mathf.Min(shieldValue, damage);
                    var newShieldValue = shieldValue - Mathf.Min(damage, shieldValue);
                    Battle.Player.ShieldTooltip.Activate(newShieldValue);
                    Battle.Player.ApplyDamage(actualDamage);
                    ShowDamageTooltip(actualDamage);
                }
                else if (reflectValue > 0)
                {
                    if (Random.value < 0.5f)
                    {
                        var actualDamage = damage * 2;
                        ApplyDamage(actualDamage);
                        Battle.Player.ShowDamageTooltip(actualDamage);
                    }
                    else
                    {
                        var actualDamage = damage * 2;
                        Battle.Player.ApplyDamage(actualDamage);
                        ShowDamageTooltip(actualDamage);
                    }
                }
                else
                {
                    Battle.Player.ApplyDamage(damage);
                    ShowDamageTooltip(damage);
                }
            }
        }
        
        private void HandleReturnFromAttack()
        {
            var targetPosition = _monsterInitialPosition;
            var monsterPosition = _monsterTransform.position;
            
            var direction = targetPosition - monsterPosition;

            var velocity = monsterPosition + (direction.normalized * (Time.deltaTime * _speed));
            
            _monsterTransform.position = velocity;
            
            _speed += 0.3f;

            if (direction.magnitude <= 0.1f)
            {
                _monsterTransform.position = _monsterInitialPosition;
                _speed = 1;
                _isAttacking = false;
                _isReturningFromAttack = false;
                _isPerforming = false;
                
                HandleAttackCompleted();
            }
        }
        
        private void HandleAttackCompleted()
        {
            _timerBeforeAction = 0;
            
            Battle.IsCompletingTurn = false;
            Battle.ChangeTurn();
        }

        private void HandleShield()
        {
            _audioSource.PlayOneShot(ShieldClips[Random.Range(0, ShieldClips.Count)]);
            ShieldTooltip.Activate(_currentBattleAction.Value);
            _shieldValue = _currentBattleAction.Value;
            
            HandleShieldCompleted();
        }
        
        private void HandleShieldCompleted()
        {
            _timerBeforeAction = 0;
            _isPerforming = false;
            
            Battle.IsCompletingTurn = false;
            Battle.ChangeTurn();
        }

        private void HandleReflect()
        {
            _audioSource.PlayOneShot(ReflectClips[Random.Range(0, ReflectClips.Count)]);
            ReflectTooltip.Activate(_currentBattleAction.Value);
            _reflectValue = _currentBattleAction.Value;
            
            HandleReflectCompleted();
        }
        
        private void HandleReflectCompleted()
        {
            _timerBeforeAction = 0;
            _isPerforming = false;
            
            Battle.IsCompletingTurn = false;
            Battle.ChangeTurn();
        }
        
        public void ShowDamageTooltip(int damage)
        {
            DamageTooltip.Activate(damage);
        }
        
        private void HandleDeath()
        {
            var monsterPosition = _monsterTransform.position;
            
            _monsterTransform.position = new Vector3(monsterPosition.x, monsterPosition.y + _speed, monsterPosition.z);

            _speed -= 0.01f;
        }
        
        public void ClearBuffs()
        {
            ShieldTooltip.Deactivate();
            ReflectTooltip.Deactivate();

            _shieldValue = 0;
            _reflectValue = 0;
        }
    }
}