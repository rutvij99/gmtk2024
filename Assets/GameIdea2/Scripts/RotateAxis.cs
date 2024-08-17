using UnityEngine;

public class RotateAxis : MonoBehaviour
{
    [SerializeField] private Vector3 axis;

    [SerializeField] private float speed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis.normalized * (speed * Time.deltaTime));
    }
}
