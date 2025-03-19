using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnObjects : MonoBehaviour
{   
    [SerializeField]
    List<GameObject> prefabs = new List<GameObject>();
    
    [Space]
    [SerializeField] private Delays delays;
    private Camera _mainCamera;

    private float _minX;
    private float _maxX;
    
    private float _maxY;
    private float _minY;
    

    private List<GameObject> _spawnedObjects = new List<GameObject>();
    private void Awake()
    {
        BusSystem.Register(Topics.Game.START_GAME, OnStartGame);
        BusSystem.Register(Topics.Game.END_GAME, OnEndGame);
    }
    
    
    
    void Start()
    {
        _mainCamera = Camera.main;

        _minX = _mainCamera.ScreenToWorldPoint(new Vector2(0, Screen.height)).x;
        _maxX = _mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        
        
        _maxY = _mainCamera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
        _minY = _mainCamera.ScreenToWorldPoint(new Vector2(0, 0)).y;
        
    }

    void OnStartGame(BusObject busObject)
    {
        StartCoroutine(Spawn());
    }
    
    void OnEndGame(BusObject busObject)
    {
        StopAllCoroutines();

        for (int i = 0; i < _spawnedObjects.Count; i++)
        {   
            Destroy(_spawnedObjects[i]);
        }

        _spawnedObjects.Clear();
    }

    private void Update()
    {
        for (int i = 0; i < _spawnedObjects.Count; i++)
        {
            _spawnedObjects[i].transform.Translate(Vector3.down * Time.deltaTime * 10f, Space.World);

            if (_spawnedObjects[i].transform.position.y < _minY)
            {
                _spawnedObjects[i].SetActive(false);
            }
        }
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(delays.GetAsteroidBeginDelay());
        
        while (true)
        {
            yield return new WaitForSeconds(delays.GetAsteroidSpawnDelay());

            Vector3 newPos = new Vector3(Random.Range(_minX, _maxX), _maxY, 3);


            bool pooledObject = false;
            for(int i = 0; i < _spawnedObjects.Count ; i++)
            {
                if (_spawnedObjects[i].activeSelf == false)
                {
                    _spawnedObjects[i].SetActive(true);
                    _spawnedObjects[i].transform.position = newPos;
                    pooledObject = true;
                    break;
                }
            }

            if (pooledObject == false)
            {
                GameObject obj = Instantiate(prefabs[0], newPos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                _spawnedObjects.Add(obj);
            }
        }
    }
}
