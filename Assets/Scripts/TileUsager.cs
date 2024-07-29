using System.Collections.Generic;
using UnityEngine;

public class TileUsager : MonoBehaviour, ITileUsager
{
    [SerializeField] private Transform _tileChecker;
    [SerializeField] private LayerMask _layer;
    [Space] 
    [SerializeField] private int _maxTiles = 1; //Далее будет контролироваться извне

    private Vector3 _cube = Vector3.one;

    public void Usage()
    {
        List<TileStateController> tileStateControllers = GetSortedTiles();

        for (int i = 0; i < _maxTiles; i++)
        {
            tileStateControllers[i].ChangeState();
        }
    }

    private List<TileStateController> GetSortedTiles()
    {
        Collider[] tiles = Physics.OverlapBox(_tileChecker.position, _cube, Quaternion.identity, _layer);
        List<TileStateController> tileStateControllers = new List<TileStateController>();
        foreach (Collider tile in tiles)
        {
            if (tile.gameObject.TryGetComponent(out TileStateController tileStateController))
            {
                tileStateControllers.Add(tileStateController);
            }
        }

        tileStateControllers.Sort((x, y) => GetDistance(x.transform).CompareTo(GetDistance(y.transform)));

        return tileStateControllers;
    }

    private float GetDistance(Transform obj)
    {
        return Vector3.Distance(transform.position, obj.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_tileChecker.position, _cube);
    }
}
