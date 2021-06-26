using Cinemachine;
using Unity.VisualScripting;
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

    [Header("Jumping")] [SerializeField] private float m_jumpHeight = 6.5f;
    [SerializeField] private float m_lowJumpMultiplier = 2.0f;

    [Header("Pushing")] [Range(0.1f, 10)] [SerializeField]
    private float m_pushPower = 2;

    [Header("Animation")]
    [SerializeField]
    private FloatReference m_speedFloatReference;

    [SerializeField] private Vector3Reference m_playerAnimationPosition;

    [SerializeField] private BoolReference m_isJumping;

    [SerializeField] private Vector3Reference m_playerLedgePosition;

    [SerializeField] private BoolReference m_isLedgeGrabbing;
    private bool m_grabActivated;

    [SerializeField] private BoolReference m_isOnLadder;
    private bool m_onLadder;
    private bool m_climbingOffLadder;

    [SerializeField] private BoolReference m_ClimbUpComplete;

    [SerializeField] private BoolReference m_roll;
    [SerializeField] private float m_rollDistance = 11.25f;
    private bool m_isRolling;
    [SerializeField] private BoolReference m_rollAnimationComplete;

    [SerializeField] private Transform m_physicsCollider;
    private bool m_hasPhysicsCollider;

    private void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_playerAnimationPosition.Value = m_playerLedgePosition.Value = transform.position;
        m_isLedgeGrabbing.Value = false;
        m_rollAnimationComplete.Value = false;
        m_ClimbUpComplete.Value = false;
        m_isOnLadder.Value = false;
        if (m_physicsCollider != null)
        {
            m_hasPhysicsCollider = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        OnControllerColliderPushRigidbody(hit);
    }

    private void OnControllerColliderPushRigidbody(ControllerColliderHit hit)
    {
        Rigidbody hitRigidbody = hit.rigidbody;
        // confirm it has a rigidbody and that the rigidbody can be pushed (the body is not kinematic)
        if (hitRigidbody == null || hitRigidbody.isKinematic) return;

        // make sure that the box is not below the player
        if (hit.moveDirection.y < -0.3f) return;

        //calculate move direction
        Vector3 pushDirection = hit.moveDirection;
        //push (using rigid body velocity
        float pushPower = m_pushPower / (hitRigidbody.mass > 0 ? hitRigidbody.mass : 0.1f);
        hitRigidbody.velocity = pushDirection * pushPower;
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
        else if (m_isOnLadder.Value && !m_onLadder)
        {
            m_onLadder = true;
            GrabLedge();
        }
        else if (m_onLadder)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, m_playerAnimationPosition.Value, 1 * Time.deltaTime);
            if (!m_isOnLadder.Value)
            {
                m_onLadder = false;
                transform.position = m_playerLedgePosition.Value;
            }
        }
        else if (m_grabActivated && m_ClimbUpComplete.Value)
        {
            PullUpToLedge();
        }
        else if (m_isRolling && m_rollAnimationComplete.Value)
        {
            m_rollAnimationComplete.Value = false;
            
            //Debug.Log(Vector3.Distance(transform.position, m_playerAnimationPosition.Value));
            
            transform.position = m_playerAnimationPosition.Value;
            if (m_hasPhysicsCollider)
            {
                m_physicsCollider.gameObject.SetActive(false);
            }

            m_controller.enabled = true;
            m_isRolling = false;
        }
        else if (!m_grabActivated && !m_isRolling)
        {
            ControllerMovement();
        }
    }

    private void PullUpToLedge()
    {
        m_ClimbUpComplete.Value = false;
        m_onLadder = false;

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

        m_moveVelocity = Vector3.zero;
        m_moveDirection = Vector3.zero;
        m_speedFloatReference.Value = 0;

        transform.position = m_playerAnimationPosition.Value = m_playerLedgePosition.Value;

        m_isJumping.Value = false;

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
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position + transform.TransformDirection(Vector3.up),
                                    transform.TransformDirection(Vector3.forward), m_rollDistance))
                {
                    return;
                }

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
