using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    LOCOMOTION,
    SPRINT,
    ROLL
}

public class PlayerStateMachine : MonoBehaviour
{
    #region Exposed

    [Header("Timer")]
    [SerializeField] float _rollDuration = 0.5f;

    [Header("Moving Parameter")]
    [SerializeField] float _runSpeed;

    [SerializeField] float _sprintSpeed;

    #endregion

    #region Unity Lifecycle
    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rb2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        TransitionToState(PlayerState.LOCOMOTION);
    }
    void Update()
    {
        OnStateUpdate();
        SetInput();
    }

    void FixedUpdate()
    {
        _rb2D.velocity = _direction.normalized * _currentSpeed * Time.fixedDeltaTime;
    }
    #endregion

    #region Methods

    void OnStateEnter()
    {
        switch (_currentState)
        {
            case PlayerState.LOCOMOTION:
                break;
            case PlayerState.ROLL:

                _animator.SetBool("isRolling", true);
                _endRollTime = Time.timeSinceLevelLoad + _rollDuration;

                break;
            case PlayerState.SPRINT:
                _animator.SetBool("isSprinting", true);
                break;
        }
    }

    void OnStateExit()
    {
        switch (_currentState)
        {
            case PlayerState.LOCOMOTION:
                break;
            case PlayerState.ROLL:

                _animator.SetBool("isRolling", false);

                break;
            case PlayerState.SPRINT:
                _animator.SetBool("isSprinting", false);
                break;
        }
    }

    void OnStateUpdate()
    {
        switch (_currentState)
        {
            case PlayerState.LOCOMOTION:
                _currentSpeed = _runSpeed;
                _animator.SetFloat("Direction_X", _direction.x);
                _animator.SetFloat("Direction_Y", _direction.y);

                if (Input.GetButtonDown("Fire3"))
                {
                    TransitionToState(PlayerState.ROLL);
                }
                break;

            case PlayerState.ROLL:
                _currentSpeed = _runSpeed;
                if (Time.timeSinceLevelLoad > _endRollTime)
                {
                    if (Input.GetButton("Fire3"))
                    {
                        TransitionToState(PlayerState.SPRINT);
                    }
                    else
                    {
                        TransitionToState(PlayerState.LOCOMOTION);
                    }
                }
                break;

            case PlayerState.SPRINT:
                _currentSpeed = _runSpeed * _sprintSpeed;
                _animator.SetFloat("Direction_X", _direction.x);
                _animator.SetFloat("Direction_Y", _direction.y);
                if (Input.GetButtonUp("Fire3"))
                {
                    TransitionToState(PlayerState.LOCOMOTION);
                }
                break;
            default:
                break;
        }
    }

    void TransitionToState(PlayerState toState)
    {
        OnStateExit();
        _currentState = toState;
        OnStateEnter();
    }

    void SetInput()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");
    }

    #endregion

    #region Private & Protected

    private PlayerState _currentState;
    private Animator _animator;
    private float _endRollTime;

    private Rigidbody2D _rb2D;
    private Vector2 _direction;
    private float _currentSpeed;

    #endregion
}

