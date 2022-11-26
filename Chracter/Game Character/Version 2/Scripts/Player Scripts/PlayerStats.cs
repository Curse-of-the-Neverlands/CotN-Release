using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CotN
{
    public class PlayerStats : MonoBehaviour
    {

        float counter = 0;
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public int maxStamina;
        public int currentStamina;

        public HealthBar healthbar;
        public StaminaBar staminabar;
        AnimatorHandler animatorHandler;

        private void Awake()
        {
            healthbar = FindObjectOfType<HealthBar>();
            staminabar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth; 
            healthbar.SetMaxHealth(maxHealth);
            healthbar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminabar.SetMaxStamina(maxStamina);
            staminabar.SetCurrentStamina(currentStamina);
        }

        void Update()
        {
            counter++;
            if (counter == 70)
            {
                counter = 0;
                IncreaseStaminaPerTick();
            }
        
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            healthbar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("dead1", true);
            }
        }
        
        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminabar.SetCurrentStamina(currentStamina);
            //Set Bar

        }

        public void IncreaseStaminaPerTick()
        {
            currentStamina += 1;
            staminabar.SetCurrentStamina(currentStamina);
        }

    }
}