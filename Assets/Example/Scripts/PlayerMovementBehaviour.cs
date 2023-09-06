using System;
using System.Collections;
using System.Collections.Generic;
using Example;
using UnityEngine;

namespace Example
{

    public class PlayerMovementBehaviour : MonoBehaviour
    {

        [Header("Component References")] public Rigidbody playerRigidbody;

        [Header("Movement Settings")] public float movementSpeed = 3f;
        public float turnSpeed = 0.1f;

        //Stored Values
        private Camera m_Camera;
        private Vector3 m_MovementDirection;

        private void Awake()
        {
            m_Camera = Camera.main;
        }

        public void UpdateMovementData(Vector3 newMovementDirection)
        {
            m_MovementDirection = newMovementDirection;
        }

        void FixedUpdate()
        {
            MoveThePlayer();
            TurnThePlayer();
        }

        void MoveThePlayer()
        {
            Vector3 movement = CameraDirection(m_MovementDirection) * movementSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(transform.position + movement);
        }

        void TurnThePlayer()
        {
            if (m_MovementDirection.sqrMagnitude > 0.01f)
            {

                Quaternion rotation = Quaternion.Slerp(playerRigidbody.rotation,
                    Quaternion.LookRotation(CameraDirection(m_MovementDirection)),
                    turnSpeed);

                playerRigidbody.MoveRotation(rotation);

            }
        }


        Vector3 CameraDirection(Vector3 movementDirection)
        {
            var cameraForward = m_Camera.transform.forward;
            var cameraRight = m_Camera.transform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            return cameraForward * movementDirection.z + cameraRight * movementDirection.x;

        }

    }
}
