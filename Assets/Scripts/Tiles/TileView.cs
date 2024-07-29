using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileStateController))]
public class TileView : MonoBehaviour
{
    [SerializeField] private GameObject _groundObject;
    [SerializeField] private GameObject _grassObject;
    [SerializeField] private GameObject _arableLandObject;
    [SerializeField] private GameObject _flowerObject;
    [SerializeField] private GameObject _arableLandWateredObject;
    //[Space]
    //[Serializable, SerializeField] private List<Plant> _plants;

    private TileStateController _tileStateController;

    private void Awake()
    {
        _tileStateController = GetComponent<TileStateController>();
    }

    private void OnEnable()
    {
        _tileStateController.Default += OnDefault;
        _tileStateController.ArableLand += OnArableLand;
        _tileStateController.ArableLandWatered += OnWatered;
        _tileStateController.Grass += OnGrass;
        _tileStateController.Flower += OnFlower;
    }

    private void OnDisable()
    {
        _tileStateController.Default -= OnDefault;
        _tileStateController.ArableLand -= OnArableLand;
        _tileStateController.ArableLandWatered -= OnWatered;
        _tileStateController.Grass -= OnGrass;
        _tileStateController.Flower -= OnFlower;
    }

    private void OnDefault()
    {
        _groundObject.SetActive(true);
        _grassObject.SetActive(false);
        _arableLandObject.SetActive(false);
    }

    private void OnArableLand()
    {
        _groundObject.SetActive(false);
        _grassObject.SetActive(false);
        _arableLandObject.SetActive(true);
    }

    private void OnGrass(bool active)
    {
        _grassObject.SetActive(active);
    }

    private void OnFlower(bool active)
    {
        _flowerObject.SetActive(active);
    }

    private void OnWatered()
    {
        _arableLandObject.SetActive(false);
        _arableLandWateredObject.SetActive(true);
    }
}
