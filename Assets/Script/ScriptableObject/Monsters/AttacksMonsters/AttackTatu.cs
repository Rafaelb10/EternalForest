using UnityEngine;

public class AttackTatu : AttackLogic
{
    private Vector2 _velocity = Vector2.zero;
    private Vector2 _randomDirection = Vector2.zero;
    private float _randomMoveTime = 0f;

    protected override void AttackOne()
    {
        Vector2 playerPos = FindAnyObjectByType<Player>().transform.position;
        Vector2 directionToPlayer = (playerPos - (Vector2)transform.position).normalized;

        _velocity = Vector2.Lerp(_velocity, directionToPlayer * 8f, Time.deltaTime * 4f);
        transform.position += (Vector3)(_velocity * Time.deltaTime);
    }
    protected override void AttackTwo()
    {
        if (_randomMoveTime <= 0f)
        {
            _randomDirection = Random.insideUnitCircle.normalized;
            _randomMoveTime = Random.Range(1f, 3f);
        }

        transform.position += (Vector3)(_randomDirection * 20f * Time.deltaTime);
        _randomMoveTime -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            _randomDirection = Random.insideUnitCircle.normalized;
            _randomMoveTime = Random.Range(1f, 2f);
        }
    }
}
