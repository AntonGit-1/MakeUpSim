using UnityEngine;
using System.Collections;
using DG.Tweening;
using Presenter;
using Zenject;

namespace View {
    public class HandView : MonoBehaviour {
        [SerializeField]
        GameObject fist_holder, idle_holder;
        [SerializeField]
        GameObject items_parent;
        [SerializeField]
        Animator anim;
        [SerializeField]
        RectTransform referenceTransform;
        [SerializeField]
        UIButton handButton;
        [SerializeField]
        RectTransform page;
        [SerializeField]
        float ySwoopOffset;
        [SerializeField]
        float cursorFollowSpeed = 1f;

        //INJECTED
        RectTransform idle_point, pageTurnBegin_point, pageTurnEnd_point;
        RectTransform brushHold_point, creamHold_point, faceApply_point;
        BrushView blushBrush, eyeBrush;
        CreamView cream;
        InputManager inputManager;

        ItemCachedInfo cachedGrabbedItemInfo;
        IGrabable currentItem;

        IEnumerator e_followCursor;
        bool followingCursor = false;

        public UIButton HandButton { get => handButton; }

        [Inject]
        void Construct(
            AnimationPoints animationPoints,
            HouseholdItems householdItems,
            InputManager inputManager) {

            idle_point = animationPoints.Idle_point;
            pageTurnBegin_point = animationPoints.PageTurnBegin_point;
            pageTurnEnd_point = animationPoints.PageTurnEnd_point;
            brushHold_point = animationPoints.BrushHold_point;
            creamHold_point = animationPoints.CreamHold_point;
            faceApply_point = animationPoints.FaceApply_point;

            blushBrush = householdItems.BlushBrush;
            eyeBrush = householdItems.EyeBrush;
            cream = householdItems.Cream;

            this.inputManager = inputManager;
        }

        void Awake() {
            e_followCursor = EFollowCursor();
        }

        public void ToIdleHand() {
            idle_holder.SetActive(true);
            fist_holder.SetActive(false);
        }
        public void ToFistHand() {
            fist_holder.SetActive(true);
            idle_holder.SetActive(false);
        }

        public void FollowCursor() {
            if (!followingCursor) {
                followingCursor = true;
                StartCoroutine(e_followCursor);
            }
        }
        public void StopFollowingCursor() {
            if (followingCursor) {
                StopCoroutine(e_followCursor);
                followingCursor = false;
            }
        }

        public IEnumerator EFollowCursor() {
            Camera cam = Camera.main;
            while (followingCursor) {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(referenceTransform, inputManager.CursorScreenPosition, cam, out Vector2 targetPosition);
                transform.localPosition = targetPosition;

                //Дополнительная логика следования за курсором (работает не очень хорошо)

                //Vector2 diff = (targetPosition - (Vector2)transform.localPosition);
                //if (Mathf.Abs(diff.x) > 0.5f && Mathf.Abs(diff.y) > 0.5f) {
                //    transform.Translate(cursorFollowSpeed * Time.deltaTime * diff.normalized, Space.World);
                //}
                //else {
                //    transform.localPosition = targetPosition;
                //}

                yield return null;
            }
        }

        public Sequence TurnPage() {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOMove(pageTurnBegin_point.position, .1f));
            sequence.AppendCallback(() => ToFistHand());
            sequence.AppendCallback(() => page.gameObject.SetActive(true));
            sequence.Append(transform.DOMove(pageTurnEnd_point.position, .2f));
            sequence.Join(page.transform.DOScaleX(-1, .2f));
            sequence.AppendCallback(() => page.gameObject.SetActive(false));
            sequence.AppendCallback(() => page.transform.localScale = Vector3.one);
            sequence.AppendCallback(() => ToIdleHand());
            sequence.Append(transform.DOMove(idle_point.position, .2f));

            return sequence;
        }

        public Sequence GrabItem(IGrabable item) {
            Sequence sequence = DOTween.Sequence();

            cachedGrabbedItemInfo = new ItemCachedInfo(item.GrabPoint, item.Parent);
            currentItem = item;

            sequence.Append(transform.DOMove(item.GrabPoint, .25f));
            sequence.Join(transform.DOLocalRotate(new Vector3(0, 0, item.GrabAngle), .25f));
            sequence.AppendCallback(() => ToFistHand());
            sequence.AppendCallback(() => item.Item.transform.SetParent(items_parent.transform));

            return sequence;
        }
        public Sequence GrabBlushBrush() {
            return GrabItem(blushBrush);
        }
        public Sequence GrabEyeBrush() {
            return GrabItem(eyeBrush);
        }
        public Sequence GrabCream() {
            return GrabItem(cream);
        }

        public Sequence ReturnItem() {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOMove(cachedGrabbedItemInfo.cachedPosition, .25f));
            sequence.AppendCallback(() => currentItem.Item.transform.SetParent(cachedGrabbedItemInfo.cachedParent));
            sequence.AppendCallback(() => ToIdleHand());

            return sequence;
        }
        public Sequence ReturnToIdle() {
            Sequence sequence = DOTween.Sequence();

            sequence.AppendCallback(() => ToIdleHand());
            sequence.Append(transform.DOMove(idle_point.position, .4f));
            sequence.Join(transform.DOLocalRotate(Vector3.zero, .4f));

            return sequence;
        }

        public Sequence GoTo(RectTransform target, bool withOffset) {
            Vector3 targetPosition = target.position;

            if (withOffset) {
                targetPosition -= new Vector3(0, -ySwoopOffset, 0);
            }

            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOMove(targetPosition, .25f));

            return sequence;
        }
        public Sequence GoToBrushHoldPosition() {
            return GoTo(brushHold_point, false);
        }
        public Sequence GoToCreamHoldPosition() {
            return GoTo(creamHold_point, false);
        }
        public Sequence GoToFacePosition() {
            return GoTo(faceApply_point, true);
        }
 
        public Sequence PlaySwoopAnimation() {
            Sequence sequence = DOTween.Sequence();

            sequence.AppendCallback(() => anim.Play("Base Layer.Swoop_anim"));
            sequence.AppendInterval(1f);
            sequence.AppendCallback(() => anim.Play("Base Layer.Idle"));

            return sequence;
        }

        readonly struct ItemCachedInfo {
            public readonly Vector3 cachedPosition;
            public readonly Transform cachedParent;

            public ItemCachedInfo(Vector3 cachedPosition, Transform cachedParent) {
                this.cachedPosition = cachedPosition;
                this.cachedParent = cachedParent;
            }
        }
    }
}

