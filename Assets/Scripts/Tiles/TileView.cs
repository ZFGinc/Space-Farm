using UnityEngine;

[RequireComponent(typeof(TileStateController))]
public class TileView : MonoBehaviour
{
    [SerializeField] private GameObject _groundObject;
    [SerializeField] private GameObject _grassObject;
    [SerializeField] private GameObject _arableLandObject;

    private TileStateController _tileStateController;

    private void Awake()
    {
        _tileStateController = GetComponent<TileStateController>();
    }

    private void OnEnable()
    {
        _tileStateController.Default += OnDefault;
        _tileStateController.ArableLand += OnArableLand;
        _tileStateController.Grass += OnGrass;
    }

    private void OnDisable()
    {
        _tileStateController.Default -= OnDefault;
        _tileStateController.ArableLand -= OnArableLand;
        _tileStateController.Grass -= OnGrass;
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
}
