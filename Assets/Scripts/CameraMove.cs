using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x+3f,
            player.transform.position.y-0.4f, -10f);
    }
}
