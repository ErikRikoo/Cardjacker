using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    private Vector3 targetPos;
    public float speed = 2.0f;
    private float YStart = 0;

    public Vector3 offset;

    private void Start()
    {
        YStart = transform.position.y;
    }

    private void Update()
    {
        targetPos = target.position + offset;
        targetPos.y = YStart;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
    }
}
