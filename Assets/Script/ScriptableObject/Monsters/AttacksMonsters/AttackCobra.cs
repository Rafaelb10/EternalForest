using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class AttackCobra : AttackLogic
{
    [SerializeField] private GameObject _bulletNormal;
    [SerializeField] private GameObject _bulletPersegue;
    [SerializeField] private Transform _bulletSpaw;
    private bool _attackingCoowldown;

    protected override void AttackOne()
    {
        if (_attackingCoowldown == false)
        {
            Vector3 playerPos = FindAnyObjectByType<Player>().transform.position;
            Vector3 direction = (playerPos - transform.position).normalized;

            GameObject bullet = Instantiate(_bulletNormal, transform.position, Quaternion.identity);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * 10f;
            }
            StartCoroutine(AttackDuranting());
        }
    }

    protected override void AttackTwo()
    {
        if (_attackingCoowldown == false)
        {
            int bulletCount = Random.Range(4, 7);

            Vector2 spawnCenter = _bulletSpaw.position;
            Vector2 areaSize = Vector2.one;

            BoxCollider2D box = _bulletSpaw.GetComponent<BoxCollider2D>();
            if (box != null)
            {
                areaSize = box.size;
            }

            for (int i = 0; i < bulletCount; i++)
            {
                float offsetX = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
                float offsetY = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
                Vector3 spawnPos = spawnCenter + new Vector2(offsetX, offsetY);

                GameObject bullet = Instantiate(_bulletPersegue, spawnPos, Quaternion.identity);

                Transform target = FindAnyObjectByType<Player>().transform;
                Vector2 dir = (target.position - bullet.transform.position).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

                BulletTarget bt = bullet.GetComponent<BulletTarget>();
                if (bt != null)
                {
                    bt.SetTarget(target);
                }
            }
            StartCoroutine(AttackDuranting());
        }
    }
    IEnumerator AttackDuranting()
    {
        _attackingCoowldown = true;
        yield return new WaitForSeconds(5);
        _attackingCoowldown = false;
    }
}
