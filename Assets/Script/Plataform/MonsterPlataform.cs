using UnityEngine;

public class MonsterPlataform : MonoBehaviour 
{
    protected bool _playerInZone = false;
    public bool PlayerInZone { get => _playerInZone; set => _playerInZone = value; }

    protected virtual void Attack() { }
    protected virtual void Move() { }
}
