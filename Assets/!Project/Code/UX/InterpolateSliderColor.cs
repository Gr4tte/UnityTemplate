using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplate
{
    public class InterpolateSliderColor : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [InfoBox("Add OnValueChanged as listener to the slider")]
#endif
        [SerializeField] private Color _startColor = Color.red;
        [SerializeField] private Color _endColor = Color.green;

        private Slider _slider = null;
        private Image _fill = null;
        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
            if (_slider) _fill = _slider.fillRect.GetComponent<Image>();

            if (_slider && !_fill) _fill = GetComponentsInChildren<Image>().First();
            if (!_fill)
            {
                enabled = false;
                return;
            }

            OnValueChanged(_slider.value);
        }
        
        public void OnValueChanged(float value)
        {
            if (!_slider) return;
            if (!_fill) return;
            
            float t = (value - _slider.minValue) / (_slider.maxValue - _slider.minValue);
            Color color = Color.Lerp(_startColor, _endColor, t);
            _fill.color = color;
        }
    }
}