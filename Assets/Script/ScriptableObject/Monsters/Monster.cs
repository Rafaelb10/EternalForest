using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
public class Monster: MonoBehaviour, IDamageable
{
    private float _hp;
    private float _strenght;
    private float _speed;
    private float _def;

    private Sprite _sprite;
    private Animator _animator;
    private int _type;
    private int _state;
    private bool _changeState;

    [SerializeField] private GameObject[] _locateToMove;
    private int _currentTargetIndex = 0;
    private bool _reverse = false;

    [SerializeField] private MonsterData Data;

    private bool _playerInZone = false;
    private Vector2 _velocity = Vector2.zero;
    private float _circleAngle = 0f;
    private Vector2 _randomDirection = Vector2.zero;
    private float _randomMoveTime = 0f;
    private int _movetype = 0;
    private bool _changeMove = false;

    public bool PlayerInZone { get => _playerInZone; set => _playerInZone = value; }
    public bool ChangeState { get => _changeState; set => _changeState = value; }

    public void Start()
    {
        _hp = Data.Hp;
        _strenght = Data.Strenght;
        _speed = Data.Speed;
        _def = Data.Def;
        _sprite = Data.Sprite;
        _animator = Data.Animator;
        _type = (int)Data.Type;
        _state = (int)Data.State;
    }

    public void Update()
    {
        if (ChangeState == true)
        {
            _state = 1;
            ChangeState = false;
        }

        if (_state == 0)
        {
            Patrol();
        }
        else if (_state == 1) 
        { 
            Move();
        }
    }

    public void Patrol()
    {
        if (PlayerInZone == false)
        {
            if (_locateToMove.Length == 0) return;

            switch (_type)
            {
                case 0:
                    break;

                case 1:
                    MoveTowards(_locateToMove[_currentTargetIndex].transform.position);

                    if (HasReachedTarget(_locateToMove[_currentTargetIndex].transform.position))
                    {
                        _currentTargetIndex = (_currentTargetIndex + 1) % _locateToMove.Length;
                    }
                    break;

                case 2:
                    MoveTowards(_locateToMove[_currentTargetIndex].transform.position);

                    if (HasReachedTarget(_locateToMove[_currentTargetIndex].transform.position))
                    {
                        if (!_reverse)
                        {
                            _currentTargetIndex++;
                            if (_currentTargetIndex >= _locateToMove.Length)
                            {
                                _currentTargetIndex = _locateToMove.Length - 2;
                                _reverse = true;
                            }
                        }
                        else
                        {
                            _currentTargetIndex--;
                            if (_currentTargetIndex < 0)
                            {
                                _currentTargetIndex = 1;
                                _reverse = false;
                            }
                        }
                    }
                    break;
            }
        }
        else if (PlayerInZone == true)
        {
            MoveTowards(FindAnyObjectByType<Player>().transform.position);
        }
    }

    public void Move()
    {
        

        if (_changeMove == false)
        {
            _movetype = Random.Range(0, 3);
            StartCoroutine(MoveType());
        }

        switch (_movetype)
        {
            case 0:
                Vector2 playerPos = FindAnyObjectByType<Player>().transform.position;
                Vector2 directionToPlayer = (playerPos - (Vector2)transform.position).normalized;

                _velocity = Vector2.Lerp(_velocity, directionToPlayer * 3f, Time.deltaTime * 2f);
                transform.position += (Vector3)(_velocity * Time.deltaTime);
                break;

            case 1:
                playerPos = FindAnyObjectByType<Player>().transform.position;
                Vector2 targetOrbitPos = playerPos + new Vector2(Mathf.Cos(_circleAngle), Mathf.Sin(_circleAngle)) * 4f;

                if (Vector2.Distance(transform.position, targetOrbitPos) > 0.1f)
                {
                    Vector2 moveDir = (targetOrbitPos - (Vector2)transform.position).normalized;
                    transform.position += (Vector3)(moveDir * 3f * Time.deltaTime);
                }
                else
                {
                    _circleAngle += Time.deltaTime * 1f;
                    targetOrbitPos = playerPos + new Vector2(Mathf.Cos(_circleAngle), Mathf.Sin(_circleAngle)) * 4f;
                    transform.position = (Vector3)targetOrbitPos;
                }
                break;

            case 2:
                if (_randomMoveTime <= 0f)
                {
                    _randomDirection = Random.insideUnitCircle.normalized;
                    _randomMoveTime = Random.Range(1f, 3f);
                }

                transform.position += (Vector3)(_randomDirection * 3f * Time.deltaTime);
                _randomMoveTime -= Time.deltaTime;
                break;
        }
    }

    private void MoveTowards(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, 3 * Time.deltaTime);
    }

    private bool HasReachedTarget(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) < 0.1f;
    }

    public void TakeDamage(float damage)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_state == 0)
        {
            if (collision.TryGetComponent<Player>(out var player))
            {
                FindAnyObjectByType<UIStatusManager>().Enemy = this.gameObject.name;
                FindAnyObjectByType<SaveController>()?.SaveGame();
                
                SceneManager.LoadScene("BattleScena");
            }
        }
    }

    IEnumerator MoveType()
    {
        _changeMove = true;
        yield return new WaitForSeconds(10);
        _changeMove = false;
    }

}
