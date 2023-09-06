using System;
using System.Collections;
using System.Collections.Generic;
using Example;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Example
{
    public class PlayerController : MonoBehaviour
    {

        public float movementSmoothingSpeed = 1f;
        private PlayerMovementBehaviour m_PlayerMovementBehaviour;
        private PlayerAnimationBehaviour m_PlayerAnimationBehaviour;
        private Vector3 m_RawInputMovement;
        private Vector3 m_SmoothInputMovement;
        private RPGUnit m_Unit;

        private void Awake()
        {
            m_Unit = GetComponent<RPGUnit>();
            m_PlayerMovementBehaviour = GetComponent<PlayerMovementBehaviour>();
            m_PlayerAnimationBehaviour = GetComponent<PlayerAnimationBehaviour>();
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            Vector2 inputMovement = value.ReadValue<Vector2>();
            m_RawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
        }

        //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Attack' action
        public void OnAttack(InputAction.CallbackContext value)
        {
        }

        public void OnAbility1(InputAction.CallbackContext value)
        {
            if (value.started)
                m_Unit.UseAbility(0);
        }

        public void OnAbility2(InputAction.CallbackContext value)
        {
            if (value.started)
                m_Unit.UseAbility(1);
        }

        public void OnAbility3(InputAction.CallbackContext value)
        {
            if (value.started)
                m_Unit.UseAbility(2);
        }


        //Update Loop - Used for calculating frame-based data
        void Update()
        {
            CalculateMovementInputSmoothing();
            UpdatePlayerMovement();
            UpdatePlayerAnimationMovement();
        }

        //Input's Axes values are raw


        void CalculateMovementInputSmoothing()
        {

            m_SmoothInputMovement = Vector3.Lerp(m_SmoothInputMovement, m_RawInputMovement,
                Time.deltaTime * movementSmoothingSpeed);

        }

        void UpdatePlayerMovement()
        {
            if (!m_Unit.StateContainer.HasState(UnitState.Stun))
            {
                m_PlayerMovementBehaviour.UpdateMovementData(m_SmoothInputMovement);
            }
            else
            {
                m_PlayerMovementBehaviour.UpdateMovementData(Vector3.zero);
            }
        }

        void UpdatePlayerAnimationMovement()
        {
            m_PlayerAnimationBehaviour.UpdateMovementAnimation(m_SmoothInputMovement.magnitude);
        }
    }
}
