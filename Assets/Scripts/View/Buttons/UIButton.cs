using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View {
    public class UIButton : MonoBehaviour {
        public enum Mode { Click, EnterExit }
        IEnumerator currentClickTimer;
        IEnumerator currentHoldTimer;
        [SerializeField]
        Mode mode;
        [SerializeField]
        bool interactable;

        const float DOUBLE_CLICK_REGISTER_TIME = 0.25f;
        const float CLICK_REGISTER_TIME = 0.2f;
        const float HOLD_REGISTER_TIME = 1.25f;

        public UnityEvent OnClick;
        public UnityEvent OnDoubleClick;
        public UnityEvent OnEnter;
        public UnityEvent OnExit;
        public UnityEvent OnPressDown;
        public UnityEvent OnPressUp;
        public UnityEvent OnDragBegin;
        public UnityEvent OnDragEnd;
        public UnityEvent OnHoldBegin;
        public UnityEvent OnHoldEnd;

        public bool Interactable => interactable;

        public void ResetListeners() {
            OnClick.RemoveAllListeners();
            OnDoubleClick.RemoveAllListeners();
            OnEnter.RemoveAllListeners();
            OnExit.RemoveAllListeners();
            OnPressDown.RemoveAllListeners();
            OnPressUp.RemoveAllListeners();
            OnDragBegin.RemoveAllListeners();
            OnDragEnd.RemoveAllListeners();
            OnHoldBegin.RemoveAllListeners();
            OnHoldEnd.RemoveAllListeners();
        }

        public virtual void ProcessOnEnter() {
            if (interactable && mode == Mode.EnterExit) {
                OnEnter?.Invoke();
            }
        }
        public virtual void ProcessOnExit() {
            if (interactable && mode == Mode.EnterExit) {
                OnExit?.Invoke();
            }
        }

        public virtual void ProcessOnPressDown() {
            if (interactable && mode == Mode.Click) {
                OnPressDown?.Invoke();
                TryStartClickTimer();
                TryStartHoldTimer();
            }
        }
        public virtual void ProcessOnPressUp() {
            if (interactable && mode == Mode.Click) {
                OnPressUp?.Invoke();
            }
        }

        public virtual void ProcessOnDragBegin() {
            if (interactable) {
                OnDragBegin?.Invoke();
            }
        }
        public virtual void ProcessOnDragEnd() {
            if (interactable) {
                OnDragEnd?.Invoke();
            }
        }

        protected virtual void TryStartClickTimer() {
            if (currentClickTimer == null) {
                currentClickTimer = EDoubleClickTimer();
                StartCoroutine(currentClickTimer);
            }
        }
        protected virtual void TryStartHoldTimer() {
            if (currentHoldTimer == null) {
                currentHoldTimer = EHoldTimer();
                StartCoroutine(currentHoldTimer);
            }
        }

        protected virtual IEnumerator EDoubleClickTimer() {
            bool clickTrack = false;
            OnPressUp.AddListener(TrackClick);

            void TrackClick() {
                clickTrack = true;
            }

            void Exit() {
                OnPressUp.RemoveListener(TrackClick);
                currentClickTimer = null;
            }

            float clickTime = 0;
            bool clicked = false;

            while (clickTime < CLICK_REGISTER_TIME) {
                if (clickTrack) {
                    clicked = true;
                    clickTime = 0;
                    clickTrack = false;
                    break;
                }
                clickTime += Time.deltaTime;
                yield return null;
            }

            if (clicked) {
                while (clickTime < DOUBLE_CLICK_REGISTER_TIME) {
                    if (clickTrack) {
                        OnDoubleClick?.Invoke();
                        Exit();
                        yield break;
                    }
                    clickTime += Time.deltaTime;
                    yield return null;
                }
                OnClick?.Invoke();
                Exit();
                yield break;
            }
            Exit();
        }
        protected virtual IEnumerator EHoldTimer() {
            bool liftTrack = false;
            OnPressUp.AddListener(TrackLift);
            OnDragBegin.AddListener(TrackLift);

            void TrackLift() {
                liftTrack = true;
            }

            void Exit() {
                OnPressUp.RemoveListener(TrackLift);
                OnDragBegin.RemoveListener(TrackLift);
                currentHoldTimer = null;
            }

            float holdTime = 0;
            bool held = false;

            while (holdTime < HOLD_REGISTER_TIME) {
                if (liftTrack) {
                    break;
                }
                holdTime += Time.deltaTime;
                if (holdTime >= HOLD_REGISTER_TIME) {
                    held = true;
                }
                yield return null;
            }
            liftTrack = false;
            if (held) {
                OnHoldBegin?.Invoke();
                while (!liftTrack) {
                    yield return null;
                }
                OnHoldEnd?.Invoke();
            }
            Exit();
        }

        public virtual void SetInteractable(bool value) {
            interactable = value;
        }
        public void ChangeMode(Mode newMode) {
            mode = newMode;
        }
    }

}


