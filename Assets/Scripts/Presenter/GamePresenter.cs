using System.Collections;
using UnityEngine;
using Zenject;
using View;
using Model;
using DG.Tweening;

namespace Presenter {
    public class GamePresenter : MonoBehaviour {
        //INJECTED
        BookViewWorker bookViewWorker;
        HandView handViewWorker;
        TabViewManager tabViewWorker;
        Girl girlModel;
        Hand hand;
        UIButton spongeButton;
        BrushView blushBrush, eyeBrush;
        CreamView cream;
        GirlView girlView;

        State currentState;
        IdleState idleState;
        TransitionState transitionState;
        ItemReadyToUseState itemReadyToUseState;
        ItemDraggedState itemDraggedState;

        void Awake() {
            idleState = new IdleState(this);
            transitionState = new TransitionState(this);
            itemReadyToUseState = new ItemReadyToUseState(this);
            itemDraggedState = new ItemDraggedState(this);
        }

        [Inject]
        void Construct(
            BookViewWorker bookViewWorker,
            TabViewManager tabViewWorker,
            HandView handViewWorker,
            GirlView girlView,
            Girl girlModel,
            Hand hand,
            HouseholdItems householdItems,
            DelayedStartAnnouncer startAnnouncer) {

            this.bookViewWorker = bookViewWorker;
            this.tabViewWorker = tabViewWorker;
            this.handViewWorker = handViewWorker;
            this.girlView = girlView;
            blushBrush = householdItems.BlushBrush;
            eyeBrush = householdItems.EyeBrush;
            cream = householdItems.Cream;
            spongeButton = householdItems.SpongeButton;

            this.girlModel = girlModel;
            this.hand = hand;

            startAnnouncer.OnDelayedStart += DelayedStart;
        }

        void ChangeState(State newState) {
            currentState?.OnExit();
            currentState = newState;
            currentState?.OnEnter();
        }
        void DelayedStart(DelayedStartAnnouncer startAnnouncer) {
            startAnnouncer.OnDelayedStart -= DelayedStart;
            ChangeState(idleState);
        }

        abstract class State {
            protected GamePresenter owner;

            protected State(GamePresenter owner) {
                this.owner = owner;
            }

            public abstract void OnEnter();
            public abstract void OnExit();
        }

        class IdleState : State {
            public IdleState(GamePresenter owner) : base(owner) { }

            public override void OnEnter() {
                Sub();
            }

            public override void OnExit() {
                Unsub();
            }

            void Sub() {
                owner.tabViewWorker.Tab_events.OnItemClick += OnTabClicked;
                owner.bookViewWorker.Shadows_events.OnItemClick += OnShadowButtonClicked;
                owner.bookViewWorker.Lipstick_events.OnItemClick += OnLipstickButtonClicked;
                owner.bookViewWorker.Blush_events.OnItemClick += OnBlushButtonClicked;
                owner.cream.OnClick += OnCreamButtonClicked;
                owner.spongeButton.OnClick.AddListener(OnSpongeButtonClicked);
            }
            void Unsub() {
                owner.tabViewWorker.Tab_events.OnItemClick -= OnTabClicked;
                owner.bookViewWorker.Shadows_events.OnItemClick -= OnShadowButtonClicked;
                owner.bookViewWorker.Lipstick_events.OnItemClick -= OnLipstickButtonClicked;
                owner.bookViewWorker.Blush_events.OnItemClick -= OnBlushButtonClicked;
                owner.cream.OnClick -= OnCreamButtonClicked;
                owner.spongeButton.OnClick.RemoveListener(OnSpongeButtonClicked);
            }

            void OnTabClicked(TabViewButton button) {
                Sequence sequence = DOTween.Sequence();

                sequence.Append(owner.bookViewWorker.HideCurrentPage());
                sequence.JoinCallback(() => owner.tabViewWorker.TrySelectTab(button.Index));
                sequence.Append(owner.handViewWorker.TurnPage());
                sequence.Append(owner.bookViewWorker.ShowPage(button.Index));
                sequence.AppendCallback(() => owner.ChangeState(owner.idleState));

                sequence.Play();

                owner.ChangeState(owner.transitionState);
            }
            void OnShadowButtonClicked(MakeupViewButton button) {
                owner.hand.Grab(new Item(button.MakeupType, button.Index));

                Sequence sequence = DOTween.Sequence();

                sequence.Append(owner.handViewWorker.GrabEyeBrush());
                sequence.Append(owner.handViewWorker.GoTo(button.GetComponent<RectTransform>(), true));
                sequence.Append(owner.handViewWorker.PlaySwoopAnimation());
                sequence.Join(owner.eyeBrush.ApplyTipColor(button.Index));
                sequence.Append(owner.handViewWorker.GoToBrushHoldPosition());
                sequence.AppendCallback(() => owner.ChangeState(owner.itemReadyToUseState));

                sequence.Play();

                owner.ChangeState(owner.transitionState);
            }
            void OnLipstickButtonClicked(MakeupViewButton button) {
                owner.hand.Grab(new Item(button.MakeupType, button.Index));

                Sequence sequence = DOTween.Sequence();

                sequence.Append(owner.handViewWorker.GrabItem(button as IGrabable));
                sequence.Append(owner.handViewWorker.GoToBrushHoldPosition());
                sequence.AppendCallback(() => owner.ChangeState(owner.itemReadyToUseState));

                sequence.Play();

                owner.ChangeState(owner.transitionState);
            }
            void OnBlushButtonClicked(MakeupViewButton button) {
                owner.hand.Grab(new Item(button.MakeupType, button.Index));

                Sequence sequence = DOTween.Sequence();

                sequence.Append(owner.handViewWorker.GrabBlushBrush());
                sequence.Append(owner.handViewWorker.GoTo(button.GetComponent<RectTransform>(), true));
                sequence.Append(owner.handViewWorker.PlaySwoopAnimation());
                sequence.Join(owner.blushBrush.ApplyTipColor(button.Index));
                sequence.Append(owner.handViewWorker.GoToBrushHoldPosition());
                sequence.AppendCallback(() => owner.ChangeState(owner.itemReadyToUseState));

                sequence.Play();

                owner.ChangeState(owner.transitionState);
            }
            void OnCreamButtonClicked() {
                owner.hand.Grab(new Item(MakeupType.Cream, 0));

                Sequence sequence = DOTween.Sequence();

                sequence.Append(owner.handViewWorker.GrabCream());
                sequence.Append(owner.handViewWorker.GoToCreamHoldPosition());
                sequence.AppendCallback(() => owner.ChangeState(owner.itemReadyToUseState));

                sequence.Play();

                owner.ChangeState(owner.transitionState);
            }
            void OnSpongeButtonClicked() {
                owner.girlModel.Reset();
                owner.girlView.ResetAll();
            }
        }
        class TransitionState : State {
            public TransitionState(GamePresenter owner) : base(owner) { }

            public override void OnEnter() { }
            public override void OnExit() { }
        }
        class ItemReadyToUseState : State {
            public ItemReadyToUseState(GamePresenter owner) : base(owner) { }

            public override void OnEnter() {
                Sub();
            }
            public override void OnExit() {
                Unsub();
            }

            void Sub() {
                owner.handViewWorker.HandButton.OnDragBegin.AddListener(OnDragBegin);
            }
            void Unsub() {
                owner.handViewWorker.HandButton.OnDragBegin.RemoveListener(OnDragBegin);
            }

            void OnDragBegin() {
                owner.ChangeState(owner.itemDraggedState);
            }
        }
        class ItemDraggedState : State {
            bool faceEntered = false;
            public ItemDraggedState(GamePresenter owner) : base(owner) { }

            public override void OnEnter() {
                owner.handViewWorker.FollowCursor();
                Sub();
            }
            public override void OnExit() {
                faceEntered = false;
                owner.handViewWorker.StopFollowingCursor();
                Unsub();
            }

            void Sub() {
                owner.handViewWorker.HandButton.OnPressUp.AddListener(OnPressUp);

                owner.girlView.FaceHitBox.OnEnter.AddListener(OnFaceEnter);
                owner.girlView.FaceHitBox.OnExit.AddListener(OnFaceExit);

                owner.girlModel.OnBlushChanged += OnModelBlushChange;
                owner.girlModel.OnLipstickChanged += OnModelLipstickChange;
                owner.girlModel.OnShadowsChanged += OnModelShadowsChange;
                owner.girlModel.OnAcneChanged += OnModelAcneChange;
            }
            void Unsub() {
                owner.handViewWorker.HandButton.OnPressUp.RemoveListener(OnPressUp);

                owner.girlView.FaceHitBox.OnEnter.RemoveListener(OnFaceEnter);
                owner.girlView.FaceHitBox.OnExit.RemoveListener(OnFaceExit);

                owner.girlModel.OnBlushChanged -= OnModelBlushChange;
                owner.girlModel.OnLipstickChanged -= OnModelLipstickChange;
                owner.girlModel.OnShadowsChanged -= OnModelShadowsChange;
                owner.girlModel.OnAcneChanged -= OnModelAcneChange;
            }

            void OnPressUp() {
                if (faceEntered) {
                    owner.girlModel.ApplyFromHand(owner.hand);
                }
                else {
                    owner.ChangeState(owner.itemReadyToUseState);
                }
            }

            void OnFaceEnter() {
                faceEntered = true;
            }
            void OnFaceExit() {
                faceEntered = false;
            }

            void OnModelBlushChange(int index) {
                Sequence sequence = DOTween.Sequence();

                sequence.Append(owner.handViewWorker.GoToFacePosition());
                sequence.Append(owner.handViewWorker.PlaySwoopAnimation());
                sequence.Join(owner.girlView.ChangeBlush(index));
                sequence.Append(owner.handViewWorker.ReturnItem());
                sequence.Append(owner.handViewWorker.ReturnToIdle());
                sequence.Join(owner.blushBrush.ResetTipColor());
                sequence.AppendCallback(() => owner.ChangeState(owner.idleState));

                sequence.Play();

                owner.ChangeState(owner.transitionState);
            }
            void OnModelShadowsChange(int index) {
                Sequence sequence = DOTween.Sequence();

                sequence.Append(owner.handViewWorker.GoToFacePosition());
                sequence.Append(owner.handViewWorker.PlaySwoopAnimation());
                sequence.Join(owner.girlView.ChangeShadows(index));
                sequence.Append(owner.handViewWorker.ReturnItem());
                sequence.Append(owner.handViewWorker.ReturnToIdle());
                sequence.Join(owner.eyeBrush.ResetTipColor());
                sequence.AppendCallback(() => owner.ChangeState(owner.idleState));

                sequence.Play();

                owner.ChangeState(owner.transitionState);
            }
            void OnModelLipstickChange(int index) {
                Sequence sequence = DOTween.Sequence();

                sequence.Append(owner.handViewWorker.GoToFacePosition());
                sequence.Append(owner.handViewWorker.PlaySwoopAnimation());
                sequence.Join(owner.girlView.ChangeLipstick(index));
                sequence.Append(owner.handViewWorker.ReturnItem());
                sequence.Append(owner.handViewWorker.ReturnToIdle());
                sequence.AppendCallback(() => owner.ChangeState(owner.idleState));

                sequence.Play();

                owner.ChangeState(owner.transitionState);
            }
            void OnModelAcneChange(bool value) {
                Sequence sequence = DOTween.Sequence();

                sequence.Append(owner.handViewWorker.GoToFacePosition());
                sequence.Append(owner.handViewWorker.PlaySwoopAnimation());
                sequence.Join(owner.girlView.RemoveAcne());
                sequence.Append(owner.handViewWorker.ReturnItem());
                sequence.Append(owner.handViewWorker.ReturnToIdle());
                sequence.AppendCallback(() => owner.ChangeState(owner.idleState));

                sequence.Play();

                owner.ChangeState(owner.transitionState);
            }
        }

        void OnDestroy() {
            ChangeState(null);
        }
    }
}