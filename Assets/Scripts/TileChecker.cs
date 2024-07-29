using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChecker : MonoBehaviour, ITileChecker
{
    [SerializeField] private Transform _tileChecker;
    [SerializeField] private LayerMask _layer;
    private Vector3 _cube = new Vector3(0.125f,0.125f,0.125f);

    private Collider[] tiles;
    public void Action()
    {
        tiles = Physics.OverlapBox(_tileChecker.position, _cube, Quaternion.identity, _layer);
        foreach (Collider tile in tiles)
        {
            if(tile.gameObject.TryGetComponent<TileStateController>(out TileStateController _tilecontroller))
            if(tile.gameObject.TryGetComponent<TileView>(out TileView _tileview))
            {
                var fsm = _tilecontroller.fsm;
                switch(fsm.CurrentState)
                {
                    case TileStateController.State.Grass:
                        fsm.Trigger(TileStateController.Event.Cut);
                    break;
                    case TileStateController.State.Bald:
                        fsm.Trigger(TileStateController.Event.Plowed);
                    break;
                    case TileStateController.State.Plow:
                        fsm.Trigger(TileStateController.Event.Watering);
                    break;
                    case TileStateController.State.WateredPlow:
                        fsm.Trigger(TileStateController.Event.Seed);
                    break;
                    case TileStateController.State.IsGrowing:
                        fsm.Trigger(TileStateController.Event.FullyGrow);
                    break;
                    case TileStateController.State.IsGrowed:
                        fsm.Trigger(TileStateController.Event.Collect);
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_tileChecker.position, 2*_cube);
    }
}
