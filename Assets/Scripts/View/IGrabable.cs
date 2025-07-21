using UnityEngine;

namespace View {
    public interface IGrabable {
        Vector3 GrabPoint { get; }
        float GrabAngle { get; }
        Transform Parent { get; }
        GameObject Item { get; }
    }
}

