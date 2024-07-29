using System;
using UnityEngine;

public class TileStateController : MonoBehaviour
{
    private enum TileState
    {
        None = 0,
        GrassOrFlower = 1,
        ArableLand = 2,
        ArableLandWatered = 3,
        Planted = 4
    }

    public event Action Default, ArableLand, ArableLandWatered;
    public event Action<bool> Grass, Flower;
    //public event Action<Plant> Planting;

    [SerializeField, Range(0f,1f)] private float _targetPerlin = 0.7f;

    private TileState _currentState;
    //private Plant _currentPlant;
    //private Vector2 _coordinates;

    //public Plant GetCurrentPlant => _currentPlant

    //private void Start()
    //{
    //    _coordinates = new Vector2(transform.position.x, transform.position.z);
    //}

    private void RemoveGrassOrFlower()
    {
        Grass?.Invoke(false);
        Flower?.Invoke(false);
        _currentState = TileState.None;
    }

    private void Plow()
    {
        ArableLand?.Invoke();
        _currentState = TileState.ArableLand;
    }

    private void Watered()
    {
        ArableLandWatered?.Invoke();
        _currentState = TileState.ArableLandWatered;
    }

    private void Planting()
    {
        //_currentPlant = Hotbar.GetCurrentItem();
        //Planting(_currentPlant);

        _currentState = TileState.Planted;
    }

    public void ChangeState()
    {
        switch(_currentState)
        {
            case TileState.None: Plow(); break;
            case TileState.GrassOrFlower: RemoveGrassOrFlower(); break;
            case TileState.ArableLand: Watered(); break;
            case TileState.ArableLandWatered: Planting(); break;
            default: break;
        }
    }

    public void GenerateGrassOrFlower(float perlinValue)
    {
        Default?.Invoke();
        _currentState = TileState.None;

        if (perlinValue >= _targetPerlin)
        {
            Grass?.Invoke(true);
            _currentState = TileState.GrassOrFlower;
            return;
        }

        if (perlinValue / 2 >= .2)
        {
            Flower?.Invoke(true);
            _currentState = TileState.GrassOrFlower;
            return;
        }
    }
}
