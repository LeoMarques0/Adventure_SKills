using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Player
{

    public PlayerProjectile bow;
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

    public void CallShot()
    {
        if (online && photonView.IsMine)
            photonView.RPC("CallShotRPC", RpcTarget.AllViaServer);
        else if (!online)
            bow.Shoot();
    }

    [PunRPC]
    public void CallShotRPC()
    {
        bow.Shoot();
    }

    public override void CollisionsCheck()
    {
        base.CollisionsCheck();
        if (isGrounded && !isJumping)
            hasDoubleJump = true;
    }
}
