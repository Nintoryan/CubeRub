using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Score
{
    public class LevelScore : MonoBehaviour
    {
        private const int ScoreStep = 2;
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CarScoreEffect _carScoreEffect;

        private static LevelScore Instance;

        public static int CurrentScore
        {
            get => PlayerPrefs.GetInt("Money");
            set => PlayerPrefs.SetInt("Money", value);
        }

        private void Start()
        {
            Instance = this;
            _text.text = $"{CurrentScore} $";
            var lvs = FindObjectsOfType<LevelScore>();
            if (lvs.Length != 1)
            {
                Debug.LogError("Multiple LevelScores in scene!:");
                foreach (var ls in lvs)
                {
                    Debug.LogError(ls.name);
                }
            }
        }

        public static void IncreaseScore(Vector3 pos)
        {
            Instance.IncScore(pos);
            
        }
        
        private void IncScore(Vector3 pos)
        {
            AnimateScoreDifference();
            CurrentScore += ScoreStep;
            _text.text = $"{CurrentScore} $";
            Instantiate(_carScoreEffect, pos,Quaternion.identity);
        }

        private void AnimateScoreDifference()
        {
            var s = DOTween.Sequence();
            s.Append(_text.transform.DOScale(1.2f * Vector3.one, 0.2f));
            s.Append(_text.transform.DOScale(Vector3.one, 0.2f));
        }
    }
}

