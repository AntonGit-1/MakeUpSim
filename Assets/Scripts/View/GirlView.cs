using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Data;

namespace View {
    public class GirlView : MonoBehaviour {
        [SerializeField]
        float animationTime = .6f;
        [SerializeField]
        SpriteData lispstick_data, shadows_data, blush_data;
        [SerializeField]
        Image lipstick_img, shadows_img, acne_img, blush_img;
        [SerializeField]
        UIButton faceHitBox;

        public UIButton FaceHitBox { get => faceHitBox; }


        void Awake() {
            ResetAll();
        }

        public Sequence ChangeLipstick(int index) {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(lipstick_img.DOFade(0, animationTime / 2f));
            sequence.AppendCallback(() => lipstick_img.sprite = lispstick_data.Sprites[index]);
            sequence.Append(lipstick_img.DOFade(1, animationTime / 2f));
            return sequence;
        }
        public Sequence ChangeShadows(int index) {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(shadows_img.DOFade(0, animationTime / 2f));
            sequence.AppendCallback(() => shadows_img.sprite = shadows_data.Sprites[index]);
            sequence.Append(shadows_img.DOFade(1, animationTime / 2f));
            return sequence;
        }
        public Sequence ChangeBlush(int index) {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(blush_img.DOFade(0, animationTime / 2f));
            sequence.AppendCallback(() => blush_img.sprite = blush_data.Sprites[index]);
            sequence.Append(blush_img.DOFade(1, animationTime / 2f));
            return sequence;
        }
        public Tween RemoveAcne() {
            return acne_img.DOFade(0, animationTime);
        }

        public void ResetAll() {
            Color resetCol = new Color(1, 1, 1, 0);
            lipstick_img.color = resetCol;
            shadows_img.color = resetCol;
            blush_img.color = resetCol;
            acne_img.color = new Color(1, 1, 1, 1);
        }
    }
}