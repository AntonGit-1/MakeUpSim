using System.Collections;
using UnityEngine;

namespace View {
    public class HouseholdItems : MonoBehaviour {
        [SerializeField]
        BrushView blushBrush, eyeBrush;
        [SerializeField]
        CreamView cream;
        [SerializeField]
        UIButton spongeButton;

        public BrushView BlushBrush { get => blushBrush; }
        public BrushView EyeBrush { get => eyeBrush; }
        public CreamView Cream { get => cream; }
        public UIButton SpongeButton { get => spongeButton; }
    }
}