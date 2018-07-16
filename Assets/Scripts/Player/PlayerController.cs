using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public bool allowYMovement = false;
    private Rigidbody2D body;
    //private SpriteRenderer spriteRenderer;
    [ShowOnly] public bool facingLeft = true;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per physics tick
    void FixedUpdate()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        UpdatePlayer(xInput, yInput);
    }

    void UpdatePlayer(float xInput, float yInput)
    {
        if (IsMoving(xInput) || IsMoving(yInput))
        {
            flipPlayer(xInput);
            MovePlayer(xInput, yInput);
        }
    }

    bool IsMoving(float x)
    {
        return Mathf.Abs(x - 0) < float.Epsilon;
    }

    void flipPlayer(float xInput)
    {
        if (xInput > 0 && facingLeft || xInput < 0 && !facingLeft)
        {
            facingLeft = !facingLeft;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void MovePlayer(float xInput, float yInput)
    {
        body.MovePosition(new Vector2(NewX(xInput), NewY(yInput)));
    }

    float NewX(float x)
    {
        return transform.position.x + x * moveSpeed * Time.deltaTime;
    }

    float NewY(float y)
    {
        if (allowYMovement)
        {
            return transform.position.y + y * moveSpeed * Time.deltaTime;
        }
        else
        {
            return transform.position.y;
        }
    }
}
