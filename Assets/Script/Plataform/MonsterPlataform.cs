using UnityEngine;

public class MonsterPlataform : MonoBehaviour 
{
    protected bool _playerInZone = false;
    protected int _state;
    private float _damage;
    public bool PlayerInZone { get => _playerInZone; set => _playerInZone = value; }
    public int State { get => _state; set => _state = value; }
    public float Damage { get => _damage; set => _damage = value; }

    protected virtual void Attack() { }
    protected virtual void Move() { }
}
