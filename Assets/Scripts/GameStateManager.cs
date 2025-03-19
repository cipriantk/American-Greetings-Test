using System;
using UnityEngine;



public enum GameState
{   
    Demo,
    Game
}
public class GameStateManager : MonoBehaviour {   
    
    private GameState _currentGamestate = GameState.Demo;

    private void Awake()
    {
        BusSystem.Register(Topics.Game.START_GAME, OnStartGame);
        BusSystem.Register(Topics.Game.END_GAME, OnEndGame);
    }


    public GameState GetCurrentGameState()
    {
        return _currentGamestate;
    }

    void OnStartGame(BusObject busObject)
    {
        _currentGamestate = GameState.Game;
    }
    
    void OnEndGame(BusObject busObject)
    {
        _currentGamestate = GameState.Demo;
    }
    
}
