using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplate
{
    [RequireComponent(typeof(Slider))]
    public class InterpolateSliderColor : MonoBehaviour
    {
        [SerializeField] private Color _startColor = Color.red;
        [SerializeField] private Color _endColor = Color.green;

        private Slider _slider = null;
        private Image _fill = null;
        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(OnValueChanged);
            
            if (_slider.fillRect.TryGetComponent(out Image image))
            {
                _fill = image;
            }
            else
            {
                enabled = false;
                return;
            }

            OnValueChanged(_slider.value);
        }
        
        private void OnValueChanged(float value)
        {
            float t = (value - _slider.minValue) / (_slider.maxValue - _slider.minValue);
            Color color = Color.Lerp(_startColor, _endColor, t);
            _fill.color = color;
        }
    }
}