using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Player
{

    bool hasDoubleJump;

    public override void Jump()
    {
        base.Jump();

        if (Input.GetButtonDown("Jump") && !isGrounded && hasDoubleJump)
            DoubleJump();
    }

    void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        hasDoubleJump = false;
    }

    public override void CollisionsCheck()
    {
        base.CollisionsCheck();
        if (isGrounded && !isJumping)
            hasDoubleJump = true;
    }
}
