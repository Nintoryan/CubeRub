using TMPro;
using UnityEngine;

namespace Score
{
    public class LevelScore : MonoBehaviour
    {
        private const int ScoreStep = 2;
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CarScoreEffect _carScoreEffect;

        private int _currentScore;

        private static LevelScore Instance;
        public static int CurrentScore => Instance._currentScore;

        private void Start()
        {
            Instance = this;
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
            _currentScore += ScoreStep;
            _text.text = CurrentScore.ToString();
            Instantiate(_carScoreEffect, pos,Quaternion.identity);
        }

        private void AnimateScoreDifference()
        {
            
        }
    }
}

