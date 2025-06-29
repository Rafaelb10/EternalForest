using System.Collections;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Monster>(out var monster))
        {
            monster.TakeDamage(FindAnyObjectByType<Player>().Strenght);
        }
        else if (collision.gameObject.TryGetComponent<MonsterPlataform>(out var monsterPlataform))
        {
            monsterPlataform.TakeDamage(FindAnyObjectByType<Player>().Strenght);
        }
        else if (collision.gameObject.TryGetComponent<BossFinal>(out var BossFinal))
        {
            BossFinal.TakeDamage(FindAnyObjectByType<Player>().Strenght);
        }
        else if (collision.gameObject.TryGetComponent<CruzTarget>(out var CruzTarget))
        {
            CruzTarget.TakeDamage(FindAnyObjectByType<Player>().Strenght);
        }
    }
}
