using UnityEngine;

namespace Data {
    [CreateAssetMenu(menuName = "GameData/SpriteData")]
    public class SpriteData : ScriptableObject {
        [SerializeField]
        Sprite[] sprites;

        public Sprite[] Sprites { get => sprites; }
    }
}

