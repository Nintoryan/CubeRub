using UnityEngine;
using UnityEngine.Events;

namespace CubeRub.Car
{
    public class TriggersDetector : MonoBehaviour
    {
        public UnityAction OnFinishReached;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Finish>())
            {
                OnFinishReached?.Invoke();
            }
        }
    }
}
