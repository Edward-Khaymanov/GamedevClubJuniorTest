using UnityEngine;
using UnityEngine.UI;

namespace ClubTest
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _progressSource;

        public void Fill(float current, float total)
        {
            total = Mathf.Clamp(total, 0f, int.MaxValue);
            current = Mathf.Clamp(current, 0f, total);

            var amount = current / total;
            if (current == 0f)
            {
                amount = 0f;
            }

            _progressSource.fillAmount = amount;
        }
    }
}