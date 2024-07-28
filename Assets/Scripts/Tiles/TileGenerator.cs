using System.Collections.Generic;
using UnityEngine;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using UnityEngine.UIElements;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private List<TileStateController> _tiles;
    [SerializeField] private List<GameObject> _rocksAndTrees;
    [Space]
    [SerializeField] private int _spawnRadius = 10;
    [SerializeField] private int _stepsAfterLastTree = 20;
    [SerializeField, Range(0, 99)] private int _seed;

    private float _scalePerlin = 20f;

    private void Start()
    {
        SpawnTiles();
        SpawnRockAndTrees();
    }

    private void SpawnTiles()
    {
        Vector3Int position = new Vector3Int(0, 1, 0);
        System.Random rand = new System.Random(_seed);
        int tileIndex = 0;
        float perlinX = 0;
        float perlinY = 0;

        for(int x = -_spawnRadius;  x < _spawnRadius; x++) 
        {
            for (int z = -_spawnRadius; z < _spawnRadius; z++)
            {
                position.x = x;
                position.z = z;

                if (Vector3.Distance(transform.position, position) >= _spawnRadius) continue;

                perlinX = (x / _scalePerlin) + _spawnRadius + _seed;
                perlinY = (z / _scalePerlin) + _spawnRadius + _seed;

                if (Mathf.PerlinNoise(perlinX, perlinY) > .5f) tileIndex++;
                if (Mathf.PerlinNoise(perlinX, perlinY) > .75f) tileIndex++;

                Instantiate(_tiles[tileIndex], position, Quaternion.identity, transform).GenerateGrass(rand.Next());
                tileIndex = 0;
            }
        }
    }

    private void SpawnRockAndTrees()
    {
        System.Random rand = new System.Random(_seed);
        Instantiate(_rocksAndTrees[rand.Next(_rocksAndTrees.Count)], Vector3.zero, Quaternion.identity, transform);
    }
}
