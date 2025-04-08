using UnityEngine;

public class MouseFollowCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;

    void Update()
    {
        if (PauseManager.instance.currentState == PauseManager.GameState.Playing && Time.timeScale != 0f)
        {
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
    }
}
