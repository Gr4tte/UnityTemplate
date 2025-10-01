using DG.Tweening;
using UnityEngine;

namespace BiscuitTD
{
    public class BlinkOnAwake : MonoBehaviour
    {
        [SerializeField] private float _scaleOut = 1.2f;
        [SerializeField] private float _durationOut = 0.1f;
        [SerializeField] private Ease _easeOut = Ease.Linear;
        [SerializeField] private float _scaleIn = 1f;
        [SerializeField] private float _durationIn = 0.1f;
        [SerializeField] private Ease _easeIn = Ease.Linear;
        
        private void Awake()
        {
            Vector3 originalScale = transform.localScale;
            transform.localScale = originalScale * _scaleIn;
            
            Sequence seq = DOTween.Sequence().SetLoops(-1);
            seq.Append(transform.DOScale(originalScale * _scaleOut, _durationOut).SetEase(_easeOut));
            seq.Append(transform.DOScale(originalScale * _scaleIn, _durationIn).SetEase(_easeIn));
            
            seq.Play();
        }
    }
}