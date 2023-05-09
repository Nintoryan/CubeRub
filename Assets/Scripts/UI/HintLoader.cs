using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HintLoader:MonoBehaviour
    {
        [SerializeField] private Sprite[] _levelHits;
        [SerializeField] private Image _presenter;

        private void Start()
        {
            _presenter.sprite = _levelHits[LevelProgressHandler.CurrentLevelID%_levelHits.Length];
        }
    }
}