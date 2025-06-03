using System;
using UnityEngine;

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

    [SerializeField] private GameObject[] _locateToMove;
    private int _currentTargetIndex = 0;
    private bool _reverse = false;

    [SerializeField] private MonsterData Data;

    private bool _playerInZone = false;

    public bool PlayerInZone { get => _playerInZone; set => _playerInZone = value; }

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
                    Debug.Log("C");
                    break;

                case 1:
                    Debug.Log("A");
                    MoveTowards(_locateToMove[_currentTargetIndex].transform.position);

                    if (HasReachedTarget(_locateToMove[_currentTargetIndex].transform.position))
                    {
                        _currentTargetIndex = (_currentTargetIndex + 1) % _locateToMove.Length;
                    }
                    break;

                case 2:
                    Debug.Log("B");
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
            MoveTowards(FindObjectOfType<Player>().transform.position);
        }
    }

    public void Move()
    {

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

    public void ChageState()
    {

    }
}
