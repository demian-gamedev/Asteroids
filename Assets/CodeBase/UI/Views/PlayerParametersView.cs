using CodeBase.Common.Attributes;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Views
{
    public class PlayerParametersView : MonoBehaviour
    {
        [Data("Position")]
        public TextMeshProUGUI PositionText;
        [Data("Rotation")]
        public TextMeshProUGUI RotationText;
        [Data("Speed")]
        public TextMeshProUGUI SpeedText;
        [Space(10)]
        [Data("ChargesLeft")]
        public TextMeshProUGUI ChargesLeftText;
        [Data("ChargeReload")]
        public TextMeshProUGUI ChargeReloadText;
    }
}