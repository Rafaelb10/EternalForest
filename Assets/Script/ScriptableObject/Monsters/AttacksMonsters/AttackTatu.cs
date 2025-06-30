using UnityEngine;

public class AttackTatu : AttackLogic
{
    private Rigidbody2D _rb;
    private Vector2 _velocity = Vector2.zero;
    private Vector2 _randomDirection = Vector2.zero;
    private float _randomMoveTime = 0f;
    private bool _isPaused = false;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private Vector2 _lastDirection = Vector2.down; 

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void AttackOne()
    {
        if (_isPaused)
        {
            UpdateAnimator(Vector2.zero);
            return;
        }

        Vector2 playerPos = FindAnyObjectByType<Player>().transform.position;
        Vector2 directionToPlayer = (playerPos - (Vector2)transform.position).normalized;

        _velocity = Vector2.Lerp(_velocity, directionToPlayer * 8f, Time.deltaTime * 4f);
        _rb.linearVelocity = _velocity;

        UpdateAnimator(_velocity);
    }

    protected override void AttackTwo()
    {
        if (_isPaused)
        {
            UpdateAnimator(Vector2.zero);
            return;
        }

        if (_randomMoveTime <= 0f)
        {
            _randomDirection = Random.insideUnitCircle.normalized;
            _randomMoveTime = Random.Range(1f, 3f);
        }

        _rb.linearVelocity = _randomDirection * 20f;
        _randomMoveTime -= Time.deltaTime;

        UpdateAnimator(_rb.linearVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(FindAnyObjectByType<Monster>().Strenght);
            }

            StartCoroutine(PauseAfterHit());
        }
        else
        {
            _randomDirection = Random.insideUnitCircle.normalized;
            _randomMoveTime = Random.Range(1f, 2f);
        }
    }

    private System.Collections.IEnumerator PauseAfterHit()
    {
        _isPaused = true;
        _rb.linearVelocity = Vector2.zero;

        UpdateAnimator(Vector2.zero); 
        yield return new WaitForSeconds(0.5f);

        _isPaused = false;

        UpdateAnimator(_lastDirection * 0.1f);
    }

    private void UpdateAnimator(Vector2 movement)
    {
        float absX = Mathf.Abs(movement.x);
        float absY = Mathf.Abs(movement.y);

        if (movement.magnitude < 0.05f)
        {
            _animator.SetInteger("Direcao", 0); 
            return;
        }

        _lastDirection = movement.normalized;

        if (absX > absY)
        {
            _animator.SetInteger("Direcao", 3); 
            _spriteRenderer.flipX = movement.x < 0;
        }
        else
        {
            _animator.SetInteger("Direcao", movement.y > 0 ? 2 : 1); 
        }
    }
}