using System.Collections.Generic;
using MHLab.InfectionsBlaster.Extensions;
using MHLab.SlayTheOrc.Decks.Cards;
using MHLab.SlayTheOrc.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MHLab.SlayTheOrc
{
    public sealed class PlayerInBattle : MonoBehaviour
    {
        private AudioSource _audioSource;
        public List<AudioClip> HitClips;
        public List<AudioClip> ReflectClips;
        public List<AudioClip> ShieldClips;

        public GameObject HelpPanel;
        
        public int MaxHP;
        public int CurrentHP;
        
        public Battle Battle;
        
        public Tooltip DamageTooltip;
        public Resourcebar HPBar;
        public Text PotionsValue;

        public ParticleSystem HealParticleSystem;
        
        public Tooltip ShieldTooltip;
        public Tooltip ReflectTooltip;
        
        private Transform _playerTransform;
        private Vector3 _playerInitialPosition;
        
        private bool _isPerforming;
        private Monster _target;
        private float _speed = 1;
        private BattleAction _currentBattleAction;

        private int _shieldValue;
        private int _reflectValue;
        
        private bool _isAttacking;
        private bool _isReturningFromAttack;

        private bool _isDead = false;

        private float _deathTimer;
        private const float DeathTime = 2f;
        
        private void Awake()
        {
            _playerTransform = transform;
            _playerInitialPosition = _playerTransform.position;
            _audioSource = gameObject.GetComponentNoAlloc<AudioSource>();
            
            _isPerforming = false;
        }

        private void Start()
        {
            MaxHP = PlayerManager.MaxHP;
            CurrentHP = PlayerManager.MaxHP;
            PotionsValue.text = LootManager.CurrentBag.SmallPotions.ToString();
            
            HPBar.SetValue(CurrentHP, MaxHP);
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

        private void UsePotion()
        {
            if (LootManager.CurrentBag.SmallPotions <= 0) return;
            if (CurrentHP >= MaxHP) return;
            
            LootManager.CurrentBag.SmallPotions -= 1;
            CurrentHP += 5;
            if (CurrentHP > MaxHP) CurrentHP = MaxHP;
            
            HPBar.SetValue(CurrentHP, MaxHP);
            
            PotionsValue.text = LootManager.CurrentBag.SmallPotions.ToString();
            
            HealParticleSystem.gameObject.SetActive(true);
            HealParticleSystem.Play();
        }

        public void Attack(Monster target, int damage)
        {
            _currentBattleAction = new BattleAction() {Type = ActionType.Attack, Value = damage};
            _isPerforming = true;
            _isAttacking = true;
            _isReturningFromAttack = false;
            _target = target;
        }
        
        public void Shield(int damage)
        {
            _currentBattleAction = new BattleAction() {Type = ActionType.Shield, Value = damage};
            _isPerforming = true;
        }
        
        public void Reflect(int damage)
        {
            _currentBattleAction = new BattleAction() {Type = ActionType.Reflect, Value = damage};
            _isPerforming = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                HelpPanel.SetActive(true);
            }
            else if (Input.GetKeyUp(KeyCode.H))
            {
                HelpPanel.SetActive(false);
            }

            if (Battle.IsPlayerTurn() && Input.GetKeyUp(KeyCode.P))
            {
                UsePotion();
            }
            
            if (_isDead)
            {
                HandleDeath();
                _deathTimer += Time.deltaTime;

                if (_deathTimer >= DeathTime)
                {
                    SceneManager.LoadScene("GameOver");
                }
                
                return;
            }
            
            if (_isPerforming && _currentBattleAction != null)
            {
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
            var targetPosition = _target.transform.position;
            var playerPosition = _playerTransform.position;
            
            var direction = targetPosition - playerPosition;

            var velocity = playerPosition + (direction.normalized * (Time.deltaTime * _speed));
            
            _playerTransform.position = velocity;
            
            _speed += 0.3f;

            if (direction.magnitude <= 0.1f)
            {
                _speed = 1;
                _isAttacking = false;
                _isReturningFromAttack = true;
                _audioSource.PlayOneShot(HitClips[Random.Range(0, HitClips.Count)]);
                
                var damage = _currentBattleAction.Value;
                var shieldValue = _target.GetShieldValue();
                var reflectValue = _target.GetReflectValue();

                if (shieldValue > 0)
                {
                    var actualDamage = damage - Mathf.Min(shieldValue, damage);
                    var newShieldValue = shieldValue - Mathf.Min(damage, shieldValue);
                    _target.ShieldTooltip.Activate(newShieldValue);
                    _target.ApplyDamage(actualDamage);
                    ShowDamageTooltip(actualDamage);
                }
                else if (reflectValue > 0)
                {
                    if (Random.value < 0.5f)
                    {
                        var actualDamage = damage * 2;
                        ApplyDamage(actualDamage);
                        _target.ShowDamageTooltip(actualDamage);
                    }
                    else
                    {
                        var actualDamage = damage * 2;
                        _target.ApplyDamage(actualDamage);
                        ShowDamageTooltip(actualDamage);
                    }
                }
                else
                {
                    _target.ApplyDamage(damage);
                    ShowDamageTooltip(damage);
                }
            }
        }
        
        private void HandleReturnFromAttack()
        {
            var targetPosition = _playerInitialPosition;
            var playerPosition = _playerTransform.position;
            
            var direction = targetPosition - playerPosition;

            var velocity = playerPosition + (direction.normalized * (Time.deltaTime * _speed));
            
            _playerTransform.position = velocity;
            
            _speed += 0.3f;

            if (direction.magnitude <= 0.1f)
            {
                _playerTransform.position = _playerInitialPosition;
                _speed = 1;
                _isAttacking = false;
                _isReturningFromAttack = false;
                _currentBattleAction = null;
                _isPerforming = false;
                
                HandleAttackCompleted();
            }
        }

        private void HandleAttackCompleted()
        {
            Battle.IsCompletingTurn = false;
            Battle.ChangeTurn();
        }

        public void ShowDamageTooltip(int damage)
        {
            DamageTooltip.Activate(damage);
        }

        private void HandleDeath()
        {
            var playerPosition = _playerTransform.position;
            
            _playerTransform.position = new Vector3(playerPosition.x, playerPosition.y + _speed, playerPosition.z);

            _speed -= 0.01f;
        }

        public void ClearBuffs()
        {
            ShieldTooltip.Deactivate();
            ReflectTooltip.Deactivate();

            _shieldValue = 0;
            _reflectValue = 0;
        }
        
        private void HandleShield()
        {
            _audioSource.PlayOneShot(ShieldClips[Random.Range(0, ShieldClips.Count)]);
            ShieldTooltip.Activate(_currentBattleAction.Value);

            _shieldValue = _currentBattleAction.Value;
            
            _currentBattleAction = null;
            _isPerforming = false;
            Battle.IsCompletingTurn = false;
            Battle.ChangeTurn();
        }
        
        private void HandleReflect()
        {
            _audioSource.PlayOneShot(ReflectClips[Random.Range(0, ReflectClips.Count)]);
            ReflectTooltip.Activate(_currentBattleAction.Value);

            _reflectValue = _currentBattleAction.Value;
            
            _currentBattleAction = null;
            _isPerforming = false;
            Battle.IsCompletingTurn = false;
            Battle.ChangeTurn();
        }
    }
}