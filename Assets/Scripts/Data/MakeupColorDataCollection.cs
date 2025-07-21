using UnityEngine;

namespace Data {
    [CreateAssetMenu(menuName = "GameData/MakeupColorDataCollection")]
    public class MakeupColorDataCollection : ScriptableObject {

        [SerializeField]
        MakeupColorData blushBrush_colors;
        [SerializeField]
        MakeupColorData eyeBrush_colors;

        public MakeupColorData BlushBrush_colors { get => blushBrush_colors; }
        public MakeupColorData EyeBrush_colors { get => eyeBrush_colors; }
    }
}

