using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _AttackPrefab;
    private float _spawnDistance = 1.5f;
    private int _state = 0;
    private bool _active = false;

    public int State { get => _state; set => _state = value; }

    void Update()
    {
        if (State == 1)
        {
            if (_active == false) 
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SpawnCubeAtMouseDirection();
                }
            }
        }
    }

    void SpawnCubeAtMouseDirection()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - FindAnyObjectByType<Player>().transform.position).normalized;

        Vector3 spawnPos = FindAnyObjectByType<Player>().transform.position + direction * _spawnDistance;

        Instantiate(_AttackPrefab, spawnPos, Quaternion.identity);
        StartCoroutine(AttackCoowldown());
    }

    IEnumerator AttackCoowldown()
    {
        _active = true;
        yield return new WaitForSeconds(1f);
        _active = false;
    }
}
