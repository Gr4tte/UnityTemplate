using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplate.Events;

namespace UnityTemplate
{
    public class DemoUI : MonoBehaviour
    {
        [SerializeField] private SceneCollection _wave2Scenes;
        [SerializeField] private RectTransform _hp;
        [SerializeField] private RectTransform _score;
        [SerializeField] private RectTransform _round;
        [SerializeField] private Button _nextRoundButton;
        
        private void Awake()
        {
            _round.localScale = Vector3.zero;
            _hp.anchoredPosition += new Vector2(0f, 100f);
            _score.anchoredPosition += new Vector2(0f, 100f);
            _nextRoundButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 300f);

            _hp.DOAnchorPosY(_hp.anchoredPosition.y - 100f, 0.5f)
                .SetEase(Ease.OutBack);

            _score.DOAnchorPosY(_score.anchoredPosition.y - 100f, 0.5f)
                .SetEase(Ease.OutBack);

            _nextRoundButton.GetComponent<RectTransform>().DOAnchorPosY(_nextRoundButton.GetComponent<RectTransform>().anchoredPosition.y + 300f, 0.5f)
                .SetEase(Ease.OutBack);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<Events.SceneCollectionLoaded>(OnSceneCollectionLoaded);
            _nextRoundButton.onClick.AddListener(OnNextRoundPressed);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<Events.SceneCollectionLoaded>(OnSceneCollectionLoaded);
            _nextRoundButton.onClick.RemoveListener(OnNextRoundPressed);
        }

        private void OnNextRoundPressed()
        {
            SceneSystem.LoadCollection(_wave2Scenes);
        }
        
        private void OnSceneCollectionLoaded(SceneCollectionLoaded e)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_round.DOScale(Vector3.zero, 0.3f)
                .SetEase(Ease.InOutQuad));
            seq.Append(_round.DOScale(Vector3.one, 0.4f)
                    .SetEase(Ease.OutBack))
                .SetDelay(0.2f);

            seq.Play();
        }
    }
}