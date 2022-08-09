using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _topLimit;
    [SerializeField] private float _bottomLimit;
    [SerializeField] private float _followSpeed;

    private void LateUpdate()
    {
        if (_target != null)
        {
            Vector3 newPosition = this.transform.position;
            newPosition.y = Mathf.Lerp(newPosition.y, _target.position.y, _followSpeed);

            newPosition.y = Mathf.Min(newPosition.y, _topLimit);
            newPosition.y = Mathf.Max(newPosition.y, _bottomLimit);
            transform.position = newPosition;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 topPoint = new Vector3(this.transform.position.x, _topLimit, this.transform.position.z);
        Vector3 bottomPoint = new Vector3(this.transform.position.x, _bottomLimit, this.transform.position.z);
        Gizmos.DrawLine(topPoint, bottomPoint);
    }
}
