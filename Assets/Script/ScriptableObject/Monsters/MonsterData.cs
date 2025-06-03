using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public enum PatrolType
    {
        Stoped,
        Circulating,
        GoAndBack
    }

    public enum StateType
    {
        Patrol,
        Enemy
    }

    [SerializeField] private float _hp;
    [SerializeField] private float _strenght;
    [SerializeField] private float _speed;
    [SerializeField] private float _def;

    [SerializeField] private Sprite _sprite;
    [SerializeField] private Animator _animator;
    [SerializeField] private PatrolType _type;
    [SerializeField] private StateType _state;

    public float Hp { get => _hp; set => _hp = value; }
    public float Strenght { get => _strenght; set => _strenght = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float Def { get => _def; set => _def = value; }
    public Sprite Sprite { get => _sprite; set => _sprite = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public PatrolType Type { get => _type; set => _type = value; }
    public StateType State { get => _state; set => _state = value; }
}
