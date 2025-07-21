using System.Collections;
using UnityEngine;

namespace View {
    public class AnimationPoints : MonoBehaviour {
        [SerializeField]
        RectTransform idle_point, pageTurnBegin_point, pageTurnEnd_point;
        [SerializeField]
        RectTransform brushHold_point, creamHold_point, faceApply_point;

        public RectTransform Idle_point { get => idle_point; }
        public RectTransform PageTurnBegin_point { get => pageTurnBegin_point; }
        public RectTransform PageTurnEnd_point { get => pageTurnEnd_point; }
        public RectTransform BrushHold_point { get => brushHold_point; }
        public RectTransform CreamHold_point { get => creamHold_point; }
        public RectTransform FaceApply_point { get => faceApply_point; }
    }
}