using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CotN
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool f_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool jump_Input;
        public bool inventory_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;


        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool inventoryFlag;
        public float rollInputTimer;



        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        UIManager uiManager;
        CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            uiManager = FindObjectOfType<UIManager>();
        }


        public void OnEnable() 
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();              
            }

            inputActions.Enable();
        }
    
        public void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta) 
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotsInput();
            HandleInteractingButtonInput();
            HandleJumpInput();
            HandleInventoryInput();
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;

        }

        private void HandleRollInput(float delta)
        {
           b_Input = inputActions.PlayerActions.Roll.IsPressed();

            if (b_Input)
            {
                rollInputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }
                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            inputActions.PlayerActions.RT.performed += i => rt_Input = true;

            //RB Input handles the RIGHT hand weapon's light attack
            if(rb_Input)
            {
                if(playerManager.canDoCombo) 
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;
                         
                    if (playerManager.canDoCombo)
                        return;
                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }


                }

            if(rt_Input)
            {
                playerAttacker.HandelHeavyAttack(playerInventory.rightWeapon);
            }
        }

        private void HandleQuickSlotsInput()
        {
            inputActions.QuickSlots.DPadRight.performed += i => d_Pad_Right = true;
            inputActions.QuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
            if(d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();   
            }
            else if (d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
            
            
        }

        private void HandleInteractingButtonInput()
        {
            inputActions.PlayerActions.f.performed += i => f_Input = true;
        }

        private void HandleJumpInput()
        {
            inputActions.PlayerActions.jump.performed += i => jump_Input = true;
        }

        private void HandleInventoryInput()
        {
           inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;

           if (inventory_Input)
           {
                inventoryFlag =! inventoryFlag;
                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                }
                else 
                {
                    uiManager.CloseSelectWindow();
                }
           }

        }
    }
}
