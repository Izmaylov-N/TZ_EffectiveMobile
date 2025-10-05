using System;
using UnityEngine;

namespace Project.Scripts
{
    public interface IPlayerController
    {
        float MoveDirection { get; }
        bool IsGrounded { get; }
        bool IsFacingRight { get; }
        Rigidbody2D Rigidbody2D { get; }
        public void FlipSprite();
    }
}