using System;
using System.Collections.Generic;
using BzKovSoft.ObjectSlicer.Samples;
using Deform;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //TODO SINGLETONE
    public static GameManager Instance { get; private set; }

    public GameState GameState
    {
        get => _gameState;
        private set
        {
            _gameState = value;
            OnGameStateChange?.Invoke(_gameState);
            
            if(_gameState == GameState.Finished)
                FinishLevel();
        }
    }

    public event Action<GameState> OnGameStateChange;
    
    private GameState _gameState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GameState = GameState.Started;
    }
    

    public void ChangeGameState(GameState newGameState)
    {
        if(newGameState == GameState.NotStarted) return;

        GameState = newGameState;
    }
    
    public void FinishLevel()
    {
        Invoke(nameof(LoadNextLevel), 1f);
    }
    
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;

        SceneManager.LoadScene(nextSceneIndex);
    }
}
