using System.Collections;
using System.Collections.Generic;
using Project.Scripts;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private enum MovementState { idle, running, jumping, falling, WallJump }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (_playerController.MoveDirection != 0f)
        {
            state = MovementState.running;
            FlipSprite();
        }
        else
        {
            state = MovementState.idle;
        }

        if (_playerController.Rigidbody2D.linearVelocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (_playerController.Rigidbody2D.linearVelocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("State", (int)state);
        Debug.Log(state);
    }
    
    private void FlipSprite()
    {
        if (_playerController.MoveDirection  > 0f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_playerController.MoveDirection  < 0f)
        {
            _spriteRenderer.flipX = true;
        }
    }
}
