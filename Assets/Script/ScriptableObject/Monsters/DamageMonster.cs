using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageMonster : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            if (SceneManager.GetActiveScene().name == "BattleScena")
            {
                FindAnyObjectByType<Player>().TakeDamage(FindAnyObjectByType<Monster>().Strenght);
                Destroy(gameObject);
            }
            else if (SceneManager.GetActiveScene().name == "PlataformGame")
            {
                FindAnyObjectByType<Player>().TakeDamage(FindAnyObjectByType<MonsterPlataform>().Damage);
                Destroy(gameObject);
            }
            else if (SceneManager.GetActiveScene().name == "TheFinalBattle")
            {
                FindAnyObjectByType<Player>().TakeDamage(FindAnyObjectByType<BossFinal>().Damage);
                Destroy(gameObject);
            }
        }
        Destroy(gameObject);
    }
}
