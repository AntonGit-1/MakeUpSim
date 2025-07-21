using UnityEngine;
using Model;

namespace Data {
    [CreateAssetMenu(menuName = "GameData/SpriteDataCollection")]
    public class SpriteDataCollection : ScriptableObject {
        [SerializeField]
        SpriteData girlBlush_sprites;
        [SerializeField]
        SpriteData girlLipstick_sprites;
        [SerializeField]
        SpriteData girlShadows_sprites;

        [SerializeField]
        SpriteData blush_sprites;
        [SerializeField]
        SpriteData lipstick_sprites;
        [SerializeField]
        SpriteData shadows_sprites;

        public SpriteData GirlBlush_sprites { get => girlBlush_sprites; }
        public SpriteData GirlLipstick_sprites { get => girlLipstick_sprites; }
        public SpriteData GirlShadows_sprites { get => girlShadows_sprites; }
        public SpriteData Blush_sprites { get => blush_sprites; }
        public SpriteData Lipstick_sprites { get => lipstick_sprites; }
        public SpriteData Shadows_sprites { get => shadows_sprites; }

        public SpriteData GetFromType(MakeupType makeupType, bool forObject) {
            if (forObject) {
                return makeupType switch {
                    MakeupType.Blush => Blush_sprites,
                    MakeupType.Lipstick => lipstick_sprites,
                    MakeupType.Shadow => shadows_sprites,
                    _ => null,
                };
            }
            return makeupType switch {
                MakeupType.Blush => GirlBlush_sprites,
                MakeupType.Lipstick => GirlLipstick_sprites,
                MakeupType.Shadow => GirlShadows_sprites,
                _ => null,
            };
        }
    }
}

