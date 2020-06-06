using System;
using MHLab.SlayTheOrc.Maps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MHLab.SlayTheOrc.Shop
{
    public sealed class ShopManager : MonoBehaviour
    {
        public Text GoldText;
        public Text PotionsText;
        public Text HealthText;

        public Text HealthCostText;
        public Text PotionCostText;

        public Text HealthDescription;
        public Text PotionDescription;

        public RectTransform Selector;

        public RectTransform HealthTarget;
        public RectTransform PotionTarget;
        
        private bool _healthSelected;
        private bool _potionSelected;
        
        private int _healthCost;
        private int _healthValue;
        
        private int _potionCost;
        private int _potionValue;
        
        private void Awake()
        {
            SetBagValues();

            _healthCost = Random.Range(28, 48);
            _potionCost = Random.Range(6, 16);

            var healthRandom = Random.value;
            if (healthRandom <= 0.7f)
            {
                _healthValue = 1;
            }
            else if (healthRandom <= 0.9f)
            {
                _healthValue = 2;
            }
            else
            {
                _healthValue = 3;
            }
            
            var potionRandom = Random.value;
            if (potionRandom <= 0.7f)
            {
                _potionValue = 1;
            }
            else if (potionRandom <= 0.9f)
            {
                _potionValue = 2;
            }
            else
            {
                _potionValue = 3;
            }

            HealthCostText.text = _healthCost.ToString();
            PotionCostText.text = _potionCost.ToString();
            HealthDescription.text = $"Obtain +{_healthValue}HP permanently.";
            PotionDescription.text = $"Obtain +{_potionValue} potions.";

            _healthSelected = true;
            _potionSelected = false;
            Selector.position = HealthTarget.position;
        }

        private void SetBagValues()
        {
            GoldText.text = LootManager.CurrentBag.Gold.ToString();
            PotionsText.text = LootManager.CurrentBag.SmallPotions.ToString();
            HealthText.text = PlayerManager.MaxHP.ToString();
        }

        private void MoveSelection()
        {
            if (_healthSelected)
            {
                _healthSelected = false;
                _potionSelected = true;
                Selector.position = PotionTarget.position;
            }
            else
            {
                _healthSelected = true;
                _potionSelected = false;
                Selector.position = HealthTarget.position;
            }
        }

        private void Buy()
        {
            if (_healthSelected)
            {
                if (LootManager.CurrentBag.Gold >= _healthCost)
                {
                    PlayerManager.MaxHP += _healthValue;
                    LootManager.CurrentBag.Gold -= _healthCost;
                }
            }
            else
            {
                if (LootManager.CurrentBag.Gold >= _potionCost)
                {
                    LootManager.CurrentBag.SmallPotions += _potionValue;
                    LootManager.CurrentBag.Gold -= _potionCost;
                }
            }
            
            SetBagValues();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow) || 
                Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                MoveSelection();
            }

            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
            {
                Buy();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var node = MapNodesManager.GetMapNode(PlayerManager.NodeIndexInMap);
                node.MarkAsSolved();

                MapNodesManager.Instance.gameObject.SetActive(true);
            
                SceneManager.LoadScene("LevelSelection");
            }
        }
    }
}