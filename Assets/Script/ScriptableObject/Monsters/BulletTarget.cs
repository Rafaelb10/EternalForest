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
    }
}
