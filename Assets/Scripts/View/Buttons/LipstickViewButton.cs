using System.Collections;
using UnityEngine;

namespace View {
    public class LipstickViewButton : MakeupViewButton, IGrabable {
        [SerializeField]
        RectTransform grabPoint;
        [SerializeField]
        float grabAngle;

        public Vector3 GrabPoint { get => grabPoint.position; }
        public float GrabAngle { get => grabAngle; }
        public Transform Parent { get => transform.parent; }
        public GameObject Item { get => gameObject; }
    }
}