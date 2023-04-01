using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Gets singletone instance of GameManager
        /// </summary>
        public static GameManager Instance { get; private set; }
        
        /// <summary>
        /// Get current game state
        /// Set current game state from game manager and handles level finishing
        /// </summary>
        public GameState GameState
        {
            get => _gameState;
            private set
            {
                _gameState = value;
                if(_gameState == GameState.Finished)
                    GoToRestartPanel();
            }
        }

        [Header("UI Panels")]
        
        [Tooltip("Link to panel that appears on start of the level")]
        [SerializeField] private GameObject startCanvasPanel;
        
        [Tooltip("Link to panel that appears on pause")]
        [SerializeField] private GameObject pauseCanvasPanel;
        
        [Tooltip("Link to panel that appears on finish of the level")]
        [SerializeField] private GameObject finishCanvasPanel;

        private bool isPause = false;
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
            GameState = GameState.NotStarted;
        }
        
        /// <summary>
        /// Sets game state due to encapsulation
        /// </summary>
        /// <param name="newGameState">Value to use as new game state</param>
        public void ChangeGameState(GameState newGameState)
        {
            if (newGameState == GameState.NotStarted) return;

            GameState = newGameState;
        }
        
        /// <summary>
        /// Handles pause state in game
        /// </summary>
        public void TogglePause()
        {
            isPause = !isPause;
            
            if(pauseCanvasPanel != null)
                pauseCanvasPanel.SetActive(isPause);

            Time.timeScale =  isPause ? 0f : 1f;
        }
        
        /// <summary>
        /// Starting current level
        /// </summary>
        public void StartLevel()
        {
            GameState = GameState.Started;

            TurnOffStartPanel();
        }
        
        /// <summary>
        /// Finish level and loads next one
        /// </summary>
        public void FinishLevel()
        {
            Invoke(nameof(LoadNextLevel), 1f);
        }

        /// <summary>
        /// Handles application quit
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        /// <summary>
        /// Loads next level of the game
        /// If next level doesn't exist, then loads first level
        /// </summary>
        void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
                nextSceneIndex = 0;

            SceneManager.LoadScene(nextSceneIndex);
        }
        
        /// <summary>
        /// Shows up restart panel
        /// </summary>
        private void GoToRestartPanel()
        {
            if (finishCanvasPanel != null)
                finishCanvasPanel.SetActive(true);
        }
        
        /// <summary>
        /// Hides start panel
        /// </summary>
        private void TurnOffStartPanel()
        {
            if (startCanvasPanel != null)
                startCanvasPanel.SetActive(false);
        }
    }
}