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



    #endregion

    #region Unity Lifecycle

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        TransitionToState(PlayerState.LOCOMOTION);
    }

    private void Update()
    {
        OnStateUpdate();
    }

    #endregion

    #region Methods

    private void OnStateEnter()
    {
        switch (_currentState)
        {
            case PlayerState.LOCOMOTION:
                break;
            case PlayerState.SPRINT:
                break;
            case PlayerState.ROLL:

                _animator.SetBool("isRolling", true);

                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch (_currentState)
        {
            case PlayerState.LOCOMOTION:

                _animator.SetFloat("MoveSpeedX", Input.GetAxis("Horizontal"));
                _animator.SetFloat("MoveSpeedY", Input.GetAxis("Vertical"));

                if(Input.GetButtonDown("Fire3"))
                {
                    TransitionToState(PlayerState.ROLL);
                }

                break;
            case PlayerState.SPRINT:
                break;
            case PlayerState.ROLL:
                break;
            default:
                break;
        }
    }

    private void OnStateExit()
    {
        switch (_currentState)
        {
            case PlayerState.LOCOMOTION:
                break;
            case PlayerState.SPRINT:
                break;
            case PlayerState.ROLL:

                _animator.SetBool("isRolling", false);

                break;
            default:
                break;
        }
    }

    private void TransitionToState(PlayerState ToState)
    {
        OnStateExit();
        _currentState = ToState;
        OnStateEnter();
    }

    #endregion

    #region Private & Protected

    private PlayerState _currentState;
    private Animator _animator;

    #endregion
}