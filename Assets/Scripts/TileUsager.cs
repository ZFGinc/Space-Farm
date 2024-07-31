using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;

public class TileUsager : MonoBehaviour, ITileUsager
{
    public event Action<int> Using;

    [SerializeField] private Transform _tileChecker;
    [SerializeField] private LayerMask _layer;
    [Space]
    [SerializeField] private PlayerAnimator _playerAnimator;
    [Space] 
    [SerializeField] private int _maxTiles = 5; //Далее будет контролироваться извне

    private void OnEnable()
    {
        _playerAnimator.SubsribeUsager(this);
    }

    public void Usage(int idObject)
    {
        List<TileStateController> tileStateControllers = GetSortedTiles();

        for (int i = 0; i < _maxTiles && i < tileStateControllers.Count; i++)
        {
            tileStateControllers[i].ChangeState();
        }

        Using?.Invoke(idObject);
    }

    private List<TileStateController> GetSortedTiles()
    {
        List<TileStateController> tileStateControllers = new List<TileStateController>();

        Ray ray = new Ray(_tileChecker.position, transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent(out TileStateController tileStateController))
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
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(new Ray(_tileChecker.position, transform.forward));
    }
}
