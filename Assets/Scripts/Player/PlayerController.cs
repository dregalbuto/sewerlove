using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public bool allowYMovement = false;
    [ReadOnly] public bool facingLeft = true;
    private Rigidbody2D body;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
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
            FlipPlayer(xInput);
            MovePlayer(xInput, yInput);
        }
    }

    bool IsMoving(float x)
    {
        return Mathf.Abs(x - 0) < float.Epsilon;
    }

    void FlipPlayer(float xInput)
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
