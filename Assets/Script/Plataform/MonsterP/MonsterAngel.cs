using UnityEngine;

public class MonsterAngel : MonsterPlataform
{
    private float moveSpeed = 2f;
    [SerializeField] private Transform groundCheckRight;
    private float groundCheckDistance = 0.7f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private int moveDirection = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveDirection = Random.value < 0.5f ? -1 : 1;
    }

    private void Update()
    {
        Move();
    }

    protected override void Move()
    {
        bool groundAheadRight = Physics2D.Raycast(groundCheckRight.position, Vector2.down, groundCheckDistance, groundLayer);

        Debug.DrawRay(groundCheckRight.position, Vector2.down * groundCheckDistance, Color.blue);

        if ((moveDirection == -1 && !groundAheadRight) || (moveDirection == 1 && !groundAheadRight))
        {
            FlipDirection();
        }

        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);
    }

    private void FlipDirection()
    {
        moveDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * moveDirection;
        transform.localScale = scale;
    }

    protected override void Attack()
    {
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckRight != null)
            Gizmos.DrawLine(groundCheckRight.position, groundCheckRight.position + Vector3.down * groundCheckDistance);
    }
}

