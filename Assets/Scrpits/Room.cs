using UnityEngine;

public class Room : MonoBehaviour
{

    [Header("Doors")]
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject bottomDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    [Header("Doors Colliders")]

    [SerializeField] private BoxCollider2D topDoorCollider;
    [SerializeField] private BoxCollider2D bottomDoorCollider;
    [SerializeField] private BoxCollider2D leftDoorCollider;
    [SerializeField] private BoxCollider2D rightDoorCollider;

    [Header("Doors SpirteRenderer")]
    [SerializeField] private SpriteRenderer topDoorRenderer;
    [SerializeField] private SpriteRenderer bottomDoorRenderer;
    [SerializeField] private SpriteRenderer leftDoorRenderer;
    [SerializeField] private SpriteRenderer rightDoorRenderer;

    public Vector2Int RoomIndex { get; set; }

    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2.up)
        {
            topDoorRenderer.color = Color.black;
            topDoorCollider.enabled = false;
        }

        if (direction == Vector2.down)
        {
            bottomDoorRenderer.color = Color.black;
            bottomDoorCollider.enabled = false;
        }

        if (direction == Vector2.right)
        {
            rightDoorRenderer.color = Color.black;
            rightDoorCollider.enabled = false;
        }

        if (direction == Vector2.left)
        {
            leftDoorRenderer.color = Color.black;
            leftDoorCollider.enabled = false;
        }
    }

    public void CloseDoor(Vector2Int direction)
    {
        if (direction == Vector2.up)
        {
            topDoorRenderer.color = Color.red;
            topDoorCollider.enabled = true;
        }

        if (direction == Vector2.down)
        {
            bottomDoorRenderer.color = Color.red;
            bottomDoorCollider.enabled = true;
        }

        if (direction == Vector2.right)
        {
            rightDoorRenderer.color = Color.red;
            rightDoorCollider.enabled = true;
        }

        if (direction == Vector2.left)
        {
            leftDoorRenderer.color = Color.red;
            leftDoorCollider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Coś dotknęło pokoju: " + collision.gameObject.name);
        if (collision.CompareTag("Player"))
        {
            CameraController.instance.MoveCameraToRoom(transform.position);
        }
    }
}
