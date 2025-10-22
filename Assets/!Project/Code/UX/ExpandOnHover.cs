using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityTemplate
{
    public class ExpandOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _expansion = 1.1f;
        [SerializeField] private float _duration = 0.1f;
        [SerializeField] private bool _fixedTime = false;
        
        private Vector3 _originalScale;
        
        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(_originalScale * _expansion, _duration)
                .SetEase(Ease.OutBack)
                .SetUpdate(_fixedTime);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(_originalScale, _duration)
                .SetEase(Ease.InBack)
                .SetUpdate(_fixedTime);
        }
    }
}