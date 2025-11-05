using UnityEngine;

namespace UnityTemplate
{
    public class DemoCharacterController : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        private void Update()
        {
            float moveX = 0;
            moveX += Input.GetKey(KeyCode.A) ? -_speed : 0;
            moveX += Input.GetKey(KeyCode.D) ? _speed : 0;
            
            transform.position += new Vector3(moveX * Time.deltaTime, 0, 0);
        }
    }
}