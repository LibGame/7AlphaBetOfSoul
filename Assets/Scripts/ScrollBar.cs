using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBar : MonoBehaviour
{

    private Vector3 _offset;
    private Vector3 _startPosition; // начальна позиция

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void OnMouseDown()
    {
        _offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, transform.position.y));
    }

    private void OnMouseDrag()
    {
        Vector3 curentScreenPoint = new Vector3(Input.mousePosition.x, 0, 0);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curentScreenPoint) + _offset;
        transform.position = new Vector3(Mathf.Clamp(curPosition.x, -19.5f, 1.2f), curPosition.y, 0);
    }
}
