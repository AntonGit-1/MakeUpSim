using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Data;

namespace View {
    public class BrushView : MonoBehaviour, IGrabable {
        [SerializeField]
        RectTransform grabPoint;
        [SerializeField]
        float grabAngle;
        [SerializeField]
        Image tip_img;
        float animationTime = .75f;
        [SerializeField]
        MakeupColorData colorData;

        public Vector3 GrabPoint { get => grabPoint.position; }
        public float GrabAngle { get => grabAngle; }
        public Transform Parent { get => transform.parent; }
        public GameObject Item { get => gameObject; }

        public Sequence ApplyTipColor(int colorIndex) {
            Color newColor = colorData.Colors[colorIndex];

            Sequence sequence = DOTween.Sequence();

            sequence.AppendCallback(() => tip_img.color = newColor);
            sequence.Append(tip_img.DOFade(1, animationTime));

            return sequence;
        }

        public Tween ResetTipColor() {
            return tip_img.DOFade(0, animationTime);
        }
    }
}

