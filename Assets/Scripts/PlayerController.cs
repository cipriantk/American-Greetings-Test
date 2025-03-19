using System;
using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera mainCamera;

    private bool _gameStarted = false;
    private bool _collided = false;
    private void Awake()
    {
        BusSystem.Register(Topics.Game.START_GAME, StartGame);
        BusSystem.Register(Topics.Game.COLLIDED, Collided);
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

   
    void Update()
    {
        if (_collided == true)
        {
            return;
        }
        
        
        if (_gameStarted == true)
        {
            if (Input.GetMouseButton(0) == true)
            {
                transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 3);
            }
        }
    }

    private void StartGame(BusObject busObject)
    {
        _gameStarted = true;
        _collided = false;
    }
    
    private void Collided(BusObject busObject)
    {
        _collided = true;
    }
}
