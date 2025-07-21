using System.Collections;
using UnityEngine;
using System;

namespace View {
    public class CreamView : MonoBehaviour, IGrabable {
        [SerializeField]
        RectTransform grabPoint;
        [SerializeField]
        float grabAngle;
        [SerializeField]
        UIButton button;
        public event Action OnClick;

        void Awake() {
            button.OnClick.AddListener(AnnounceOnClick);
        }

        void AnnounceOnClick() {
            OnClick?.Invoke();
        }

        public Vector3 GrabPoint { get => grabPoint.position; }
        public float GrabAngle { get => grabAngle; }
        public Transform Parent { get => transform.parent; }
        public GameObject Item { get => gameObject; }

        void OnDestroy() {
            button.OnClick.RemoveListener(AnnounceOnClick);
        }
    }
}