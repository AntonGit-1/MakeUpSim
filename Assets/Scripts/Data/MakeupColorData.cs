using System.Collections;
using UnityEngine;

namespace Data {
    [CreateAssetMenu(menuName = "GameData/MakeupColorData")]
    public class MakeupColorData : ScriptableObject {
        [SerializeField]
        Color[] colors;

        public Color[] Colors { get => colors; }
    }
}