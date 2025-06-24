using System.Collections;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(AttackFinish());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Monster>(out var monster))
        {
            monster.TakeDamage(FindAnyObjectByType<Player>().Strenght);
            Destroy(gameObject);
        }
        else if (collision.gameObject.TryGetComponent<MonsterPlataform>(out var monsterPlataform))
        {
            monsterPlataform.TakeDamage(FindAnyObjectByType<Player>().Strenght);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
    IEnumerator AttackFinish()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
