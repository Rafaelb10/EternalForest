using UnityEngine;

public class MouseActive : MonoBehaviour
{
    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
 
}
