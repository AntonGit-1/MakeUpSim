using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Data {
    public class AddressableDispatchService : MonoBehaviour {
        [SerializeField]
        AssetReferenceT<SpriteDataCollection> sprites_ref;
        [SerializeField]
        AssetReferenceT<MakeupColorDataCollection> makeupColors_ref;

        CancellationTokenSource cts = new CancellationTokenSource();

        SpriteDataCollection sprites_data;
        MakeupColorDataCollection makeupColors_data;

        public SpriteDataCollection Sprites_data { get => sprites_data; }
        public MakeupColorDataCollection MakeupColors_data { get => makeupColors_data; }

        public async UniTask LoadAll() {
            await UniTask.WhenAll(
                LoadSprites(cts.Token),
                LoadColors(cts.Token)
            );
        }

        async UniTask LoadSprites(CancellationToken token) {
            token.ThrowIfCancellationRequested();
            sprites_data = await sprites_ref.LoadAssetAsync().WithCancellation(token);
        }
        async UniTask LoadColors(CancellationToken token) {
            token.ThrowIfCancellationRequested();
            makeupColors_data = await makeupColors_ref.LoadAssetAsync().WithCancellation(token);
        }

        public void UnloadAll() {
            sprites_ref.ReleaseAsset();
            makeupColors_ref.ReleaseAsset();
        }

        void OnDestroy() {
            cts.Cancel();
            UnloadAll();
        }
    }
}

