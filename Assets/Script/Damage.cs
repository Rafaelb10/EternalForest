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
        if (collision.TryGetComponent<Monster>(out var player))
        {
            FindAnyObjectByType<Monster>().TakeDamage(FindAnyObjectByType<Player>().Strenght);
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
