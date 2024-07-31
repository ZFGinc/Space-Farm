using UnityEngine;

public abstract class Follower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothing = 1f;
    [Space]
    [SerializeField] private bool _unparentOnStart = true;

    protected void Start()
    {
        if (_unparentOnStart)
            transform.parent = null;
    }

    protected void Move(float deltaTime)
    {
        if (_target == null) Destroy(gameObject);
        
        var nextPosition = Vector3.Lerp(transform.position, _target.position + _offset, deltaTime * _smoothing);

        transform.position = nextPosition;
    }

    public Transform GetTarget() { return _target; }
}
