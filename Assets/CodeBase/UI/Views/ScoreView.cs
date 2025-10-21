using CodeBase.Common.Attributes;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Views
{
    public class ScoreView : MonoBehaviour
    {
        [Data("Score")]
        public TextMeshProUGUI ScoreText;
    }
}