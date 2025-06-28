using UnityEngine;

public class BulletTarget : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float _speed = 4f;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    void Update()
    {
        if (_target == null) return;

        Vector3 direction = (_target.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
