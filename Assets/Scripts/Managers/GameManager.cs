using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private GameObject startCanvasPanel;
        [SerializeField] private GameObject pauseCanvasPanel;
        [SerializeField] private GameObject finishCanvasPanel;

        private bool isPause = false;

        public GameState GameState
        {
            get => _gameState;
            private set
            {
                _gameState = value;
                OnGameStateChange?.Invoke(_gameState);

                GoToRestartPanel();
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
            GameState = GameState.NotStarted;
        }
        
        public void ChangeGameState(GameState newGameState)
        {
            if (newGameState == GameState.NotStarted) return;

            GameState = newGameState;
        }
        
        public void TogglePause()
        {
            isPause = !isPause;
            pauseCanvasPanel.SetActive(isPause);

            Time.timeScale =  isPause ? 0f : 1f;
        }
        
        public void StartLevel()
        {
            GameState = GameState.Started;

            TurnOffStartPanel();
        }
        
        public void FinishLevel()
        {
            Invoke(nameof(LoadNextLevel), 1f);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
                nextSceneIndex = 0;

            SceneManager.LoadScene(nextSceneIndex);
        }
        
        private void GoToRestartPanel()
        {
            if (_gameState == GameState.Finished)
                finishCanvasPanel.SetActive(true);
        }
        private void TurnOffStartPanel()
        {
            if (startCanvasPanel != null)
                startCanvasPanel.SetActive(false);
        }
    }
}