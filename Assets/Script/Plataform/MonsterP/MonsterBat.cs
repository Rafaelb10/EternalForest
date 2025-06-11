using System.Collections;
using UnityEngine;

public class MonsterBat : MonsterPlataform
{
    protected bool _playerInZoneBat;
    [SerializeField] private Transform _spawTransform;
    private bool _back;
    private Vector2 _velocity = Vector2.zero;

    private Vector2 _randomDirection = Vector2.zero;
    private float _randomMoveTime = 0f;

    private void Update()
    {
        Move();

        _playerInZoneBat = _playerInZone;
    }

    protected override void Attack() { }
    protected override void Move() 
    { 
        if (_playerInZoneBat == true)
        {
            Vector2 playerPos = FindAnyObjectByType<Player>().transform.position;
            Vector2 directionToPlayer = (playerPos - (Vector2)transform.position).normalized;

            _velocity = Vector2.Lerp(_velocity, directionToPlayer * 3f, Time.deltaTime * 2f);
            transform.position += (Vector3)(_velocity * Time.deltaTime);
        }
        else
        {
            if (_back == false)
            {
                if (_randomMoveTime <= 0f)
                {
                    _randomDirection = Random.insideUnitCircle.normalized;
                    _randomMoveTime = Random.Range(1f, 3f);
                }

                transform.position += (Vector3)(_randomDirection * 3f * Time.deltaTime);
                _randomMoveTime -= Time.deltaTime;

                StartCoroutine(GotoSpaw());
            }
            else
            {
                Vector2 spaw = _spawTransform.transform.position;
                Vector2 directionToSpaw = (spaw - (Vector2)transform.position).normalized;

                _velocity = Vector2.Lerp(_velocity, directionToSpaw * 3f, Time.deltaTime * 2f);
                transform.position += (Vector3)(_velocity * Time.deltaTime);
            }
        }
    }

    IEnumerator GotoSpaw() 
    {
        yield return new WaitForSeconds(10);
        _back = true;
        yield return new WaitForSeconds(10);
        _back = false;
    }
}
