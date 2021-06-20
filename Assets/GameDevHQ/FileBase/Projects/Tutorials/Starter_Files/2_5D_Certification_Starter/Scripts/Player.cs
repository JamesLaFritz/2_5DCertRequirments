using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController m_controller;

    [Header("Movement")]
    [Range(1, 30)]
    [SerializeField]
    private float m_speed = 5;

    private Vector3 m_moveDirection = Vector3.zero;
    private Vector3 m_moveVelocity = Vector3.zero;

    [Header("Gravity")] [SerializeField] private float m_gravityScale = 1;
    [SerializeField] private float m_fallMultiplier = 2.5f;
    private float m_currentGravityScale;

    [Header("Ledge Grab")] private bool m_grabActivated;

    [SerializeField] private Vector3Reference m_playerLedgePosition;

    [Header("Jumping")] [SerializeField] private float m_jumpHeight = 6.5f;
    [SerializeField] private float m_lowJumpMultiplier = 2.0f;

    [Header("Animation")]
    [SerializeField]
    private FloatReference m_speedFloatReference;

    [SerializeField] private Vector3Reference m_playerAnimationPosition;

    [SerializeField] private BoolReference m_isJumping;

    [SerializeField] private BoolReference m_isLedgeGrabbing;

    [SerializeField] private BoolReference m_ClimbUpComplete;

    [SerializeField] private BoolReference m_roll;
    private bool m_isRolling;
    [SerializeField] private BoolReference m_rollAnimationComplete;
    [SerializeField] private Transform m_physicsCollider;
    private bool m_hasPhysicsCollider;

    // Start is called before the first frame update
    private void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_isLedgeGrabbing.Value = false;
        m_rollAnimationComplete.Value = false;
        m_hasPhysicsCollider = m_physicsCollider != null;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_isLedgeGrabbing)
        {
            GrabLedge();

            if (Input.GetKeyDown(KeyCode.E))
            {
                m_isLedgeGrabbing.Value = false;
            }
        }
        else if (m_grabActivated && m_ClimbUpComplete.Value)
        {
            PullUpToLedge();
        }
        else if (m_isRolling && m_rollAnimationComplete.Value)
        {
            m_rollAnimationComplete.Value = false;
            transform.position = m_playerAnimationPosition.Value;
            if (m_hasPhysicsCollider)
            {
                m_physicsCollider.gameObject.SetActive(false);
                m_physicsCollider.localPosition = Vector3.zero;
            }

            m_controller.enabled = true;
            m_isRolling = false;
        }
        else if (m_isRolling && m_hasPhysicsCollider)
        {
            m_physicsCollider.position = m_playerAnimationPosition.Value;
        }
        else if (!m_grabActivated && !m_isRolling)
        {
            ControllerMovement();
        }
    }

    private void PullUpToLedge()
    {
        if (!m_grabActivated && !m_ClimbUpComplete.Value) return;

        m_ClimbUpComplete.Value = false;

        transform.position = m_playerAnimationPosition.Value - m_controller.center;
        if (m_hasPhysicsCollider)
        {
            m_physicsCollider.gameObject.SetActive(false);
        }

        m_controller.enabled = true;
        m_grabActivated = false;
    }

    private void GrabLedge()
    {
        if (m_grabActivated) return;

        m_grabActivated = true;
        m_controller.enabled = false;
        transform.position = m_playerLedgePosition.Value;

        m_isJumping.Value = false;
        m_moveVelocity = Vector3.zero;
        m_speedFloatReference.Value = 0;
        if (m_hasPhysicsCollider)
        {
            m_physicsCollider.gameObject.SetActive(true);
        }
    }

    private void ControllerMovement()
    {
        // If Grounded
        Debug.Assert(m_controller != null, nameof(m_controller) + " != null");
        if (m_controller.isGrounded)
        {
            m_isJumping.Value = false;
            // Calculate move direction based inputs
            m_moveDirection.z = Input.GetAxisRaw("Horizontal");
            Debug.Assert(m_speedFloatReference != null, nameof(m_speedFloatReference) + " != null");
            m_speedFloatReference.Value = Mathf.Abs(m_moveDirection.z);
            m_moveVelocity.z = m_moveDirection.z * m_speed;

            // Face the correct direction
            if (m_moveDirection.z != 0)
                transform.forward = m_moveDirection;

            // If Jump is pressed
            if (Input.GetButtonDown("Jump"))
            {
                // Set Velocity.Y to Jump Height
                m_moveVelocity.y = m_jumpHeight;
                m_isJumping.Value = true;
            }

            // If left shift is pressed roll
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                m_controller.enabled = false;
                if (m_hasPhysicsCollider)
                {
                    m_physicsCollider.gameObject.SetActive(true);
                }

                m_roll.Value = true;
                m_isRolling = true;
                return;
            }
        }
        // if in the air
        else
        {
            ApplyGravity();
        }

        // Move based on the velocity
        m_controller.Move(m_moveVelocity * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        // If the player is on the downward portion of its jump / falling
        if (m_moveVelocity.y < 0)
        {
            // set the gravity scale to the fall multiplier
            m_currentGravityScale = m_gravityScale * m_fallMultiplier;
        }
        //control jump height by length of time jump button held
        // apply more gravity if the player is not holding the jump button
        // and is moving upward.
        else if (m_moveVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            // apply low jump multiplier
            m_currentGravityScale = m_gravityScale * m_lowJumpMultiplier;
        }
        // else set to the normal gravity scale
        else
        {
            m_currentGravityScale = m_gravityScale;
        }

        m_moveVelocity.y += Physics.gravity.y * m_currentGravityScale * Time.deltaTime;
    }
}
