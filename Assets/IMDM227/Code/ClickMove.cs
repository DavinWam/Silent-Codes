using UnityEngine;

public class ClickMove : MonoBehaviour
{
    private void OnMouseDown()
    {

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        pos.z = transform.position.z;
        Debug.Log(pos);
        transform.position = pos;
    }
}
