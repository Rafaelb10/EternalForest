using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public Vector3 _playerPosition;

    public float _level;
    public float _xp;
    public float _strenght;
    public float _def;
    public float _hp;
    public float _speed;
    public float _money;

    public float _pointsXp;

    public string _player;
    public string _enemy;

    public List<string> _itensInventoryname = new List<string>();
    public List<int> _itensInventorycount = new List<int>();
    public List<string> _itensInventoryEquiped = new List<string>();

    public List<int> _enemysInGame = new List<int>();
    public List<float> _enemysStateGame = new List<float>();

    public List<int> _enemysInPlataform = new List<int>();
    public List<float> _enemysStatePlataform = new List<float>();


    //https://www.youtube.com/watch?v=rDZztBWGMIs
}
