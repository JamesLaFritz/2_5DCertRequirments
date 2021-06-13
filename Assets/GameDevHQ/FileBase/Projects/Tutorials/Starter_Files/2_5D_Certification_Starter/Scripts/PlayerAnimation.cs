using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator m_animator;

    [SerializeField] private FloatReference m_speed;
    private static readonly int Speed = Animator.StringToHash("Speed");

    [SerializeField] private BoolReference m_isJumping;
    private static readonly int Jump = Animator.StringToHash("IsJumping");

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // ReSharper disable PossibleNullReferenceException
        m_animator.SetFloat(Speed, m_speed.Value);
        m_animator.SetBool(Jump, m_isJumping.Value);

        // ReSharper restore PossibleNullReferenceException
    }
}
