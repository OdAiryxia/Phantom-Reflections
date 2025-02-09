using UnityEngine;

public class MouseFollowCamera : MonoBehaviour
{
    void Update()
    {
        if (PauseManager.instance.currentState == PauseManager.GameState.Playing && Time.timeScale != 0f)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
    }
}
