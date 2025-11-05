using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplate
{
    public class DemoMainMenu : MonoBehaviour
    {
        [SerializeField] private SceneCollection _gameplayScenes;
        [SerializeField] private RectTransform _uiContainer;
        [SerializeField] private Button _startButton;
        
        private void OnEnable()
        {
            SceneSystem.RegisterSyncUnloadTask(gameObject, TweenUI);
            _startButton.onClick.AddListener(OnStartPressed);
        }
        
        private void OnDisable()
        {
            SceneSystem.UnregisterSyncUnloadTask(TweenUI);
            _startButton.onClick.RemoveListener(OnStartPressed);
        }

        private void OnStartPressed()
        {
            SceneSystem.LoadCollection(_gameplayScenes);
        }

        private async Task TweenUI()
        {
            await _uiContainer.DOAnchorPosX(_uiContainer.anchoredPosition.x -600f, 0.5f)
                .SetEase(Ease.InBack)
                .AsyncWaitForCompletion();
        }
    }
}