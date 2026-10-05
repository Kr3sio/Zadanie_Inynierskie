using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    private Vector3 targetPosition;

    public float transitionSpeed = 5f;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, transitionSpeed * Time.deltaTime);
    }

    public void MoveCameraToRoom(Vector3 roomPosition)
    {
        targetPosition = new Vector3(roomPosition.x, roomPosition.y, roomPosition.z);
    }
}
