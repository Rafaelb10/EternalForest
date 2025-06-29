using UnityEngine;

public class AutoDestruction : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 4f); // Destroi este GameObject após 4 segundos
    }
}
