using System;
using UnityEngine;

public class TileStateController : MonoBehaviour
{
    public event Action Default, ArableLand;
    public event Action<bool> Grass;

    [SerializeField, Range(0f,1f)] private float _targetPerlin = 0.7f;

    private Vector2 _coordinates;

    private void Start()
    {
        _coordinates = new Vector2(transform.position.x, transform.position.z);
    }

    public void GenerateGrass(float randomValue)
    {
        Default?.Invoke();
        if (UnityEngine.Random.Range(0f, 1f) >= _targetPerlin)
            Grass?.Invoke(true);
    }
}
