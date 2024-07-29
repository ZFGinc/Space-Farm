using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITileUsager))]
//[RequireComponent(typeof(IInventory))]
//[RequireComponent(typeof(IShootingController))]
public class ActionController : MonoBehaviour
{
    [SerializeField] private ITileUsager _tileUsager;

    private void Start()
    {
        _tileUsager = GetComponent<ITileUsager>();
    }

    public void GrabOrRelese()
    {

    }

    public void Usage()
    {
        _tileUsager.Usage();
    }

    public void Shooting(bool value)
    {

    }
}
