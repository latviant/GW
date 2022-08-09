using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    [SerializeField] private GameObject _ropeSegmentPrefab;
    [SerializeField] private Rigidbody2D _connectedObject;
    [SerializeField] private float _maxRopeSegments;
    [SerializeField] private float _ropeSpeed;
    private List<GameObject> _ropeSegments = new List<GameObject>();
    private LineRenderer _lineRenderer;

    public bool IsIncreasing { get; set; }
    public bool IsDecreasing { get; set; }

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        ResetLeght();
    }

    public void ResetLeght()
    {
        foreach (GameObject sepment in _ropeSegments)
            Destroy(sepment);

        _ropeSegments = new List<GameObject>();

        IsDecreasing = false;
        IsIncreasing = false;

        CreateRopeSegment();
    }

    private void CreateRopeSegment()
    {
        GameObject segment = (GameObject)Instantiate(_ropeSegmentPrefab, this.transform.position, Quaternion.identity);
        segment.transform.SetParent(this.transform, true);

        Rigidbody2D segmentBody = segment.GetComponent<Rigidbody2D>();
        SpringJoint2D segmentJoint = segment.GetComponent<SpringJoint2D>();

        if (segmentBody == null || segmentJoint == null)
        {
            Debug.LogError("Rope segment error");
            return;
        }

        _ropeSegments.Insert(0, segment);

        if (_ropeSegments.Count == 1)
        {
            SpringJoint2D connectedObjectJoint = _connectedObject.GetComponent<SpringJoint2D>();
            connectedObjectJoint.connectedBody = segmentBody;
            connectedObjectJoint.distance = 0.1f;
            segmentJoint.distance = _maxRopeSegments;
        }
        else
        {
            GameObject nextSegment = _ropeSegments[1];
            SpringJoint2D nextSegmentJoint = nextSegment.GetComponent<SpringJoint2D>();
            nextSegmentJoint.connectedBody = segmentBody;
            segmentJoint.distance = 0.0f;
        }

        segmentJoint.connectedBody = this.GetComponent<Rigidbody2D>();
    }

    private void RemoveRopeSegment()
    {
        if (_ropeSegments.Count < 2)
        {
            return;
        }

        GameObject topSegment = _ropeSegments[0];
        GameObject nextSegment = _ropeSegments[1];
        SpringJoint2D nextSegmentJoint = nextSegment.GetComponent<SpringJoint2D>();
        nextSegmentJoint.connectedBody = this.GetComponent<Rigidbody2D>();
        _ropeSegments.RemoveAt(0);
        Destroy(topSegment);
    }

    private void Update()
    {
        GameObject topSegment = _ropeSegments[0];
        SpringJoint2D topSegmentJoint = topSegment.GetComponent<SpringJoint2D>();

        if (IsIncreasing)
        {
            if (topSegmentJoint.distance >= _maxRopeSegments)
                CreateRopeSegment();
            else
                topSegmentJoint.distance += _ropeSpeed * Time.deltaTime;
        }

        if (IsDecreasing)
        {
            if (topSegmentJoint.distance <= 0.005f)
                RemoveRopeSegment();
            else
                topSegmentJoint.distance -= _ropeSpeed * Time.deltaTime;
        }

        _lineRenderer.positionCount = _ropeSegments.Count + 2;
        _lineRenderer.SetPosition(0, this.transform.position);

        for (int i = 0; i < _ropeSegments.Count; i++)
        {
            _lineRenderer.SetPosition(i + 1, _ropeSegments[i].transform.position);
        }

        SpringJoint2D connectedObjectJoint = _connectedObject.GetComponent<SpringJoint2D>();
        _lineRenderer.SetPosition(_ropeSegments.Count + 1, _connectedObject.transform.TransformPoint(connectedObjectJoint.anchor));
    }

}
