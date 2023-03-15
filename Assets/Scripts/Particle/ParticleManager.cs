using System.Linq;
using AutoGenerate;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;
using Utils;
#if UNITY_EDITOR
using UnityEditor.AddressableAssets.Settings;
#endif

namespace Particle
{
    public class ParticleManager : MonoBehaviour
    {
        private static ParticleManager _instance;
        [SerializeField] private AssetReferenceT<VisualEffect>[] vfxRefs;

        public static ParticleManager Instance => _instance ??= FindObjectOfType<ParticleManager>();

        private void OnDestroy()
        {
            _instance = null;
        }

        public void PlayVfx(VfxEnum vfxType, float durationSec, Vector3 pos = default, Quaternion rot = default)
        {
            var assetRef = vfxRefs[(int)vfxType];
            UniTask.Create(async () =>
            {
                var vfx = await Addressables.InstantiateAsync(assetRef, pos, rot);
                await DOVirtual.DelayedCall(durationSec, () => assetRef.ReleaseInstance(vfx)).SetLink(gameObject);
            });
        }

#if UNITY_EDITOR
        [SerializeField] private AddressableAssetGroup vfxGroup;
        public AddressableAssetGroup VfxGroup => vfxGroup;
        private void OnValidate()
        {
            if (vfxGroup != null)
                vfxRefs = vfxGroup.entries.Select(e => new AssetReferenceT<VisualEffect>(e.guid)).ToArray();
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ParticleManager))]
    public class ParticleManagerEditor : Editor
    {
        private (VfxEnum, float, bool) _vfx;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var manager = (target as ParticleManager)!;

            if (!Application.isPlaying && manager.VfxGroup != null && GUILayout.Button("Detect Vfx"))
                EnumCreator.CreateSource(manager.VfxGroup.entries.Select(v => v.address),
                    "Assets/Plugins/AutoGenerate/VfxEnum.cs");

            if (Application.isPlaying && (_vfx.Item3 = EditorGUILayout.Foldout(_vfx.Item3, "Debug")))
            {
                _vfx.Item1 = (VfxEnum)EditorGUILayout.EnumPopup("Vfx", _vfx.Item1);
                _vfx.Item2 = EditorGUILayout.FloatField("duration", _vfx.Item2);
                if (GUILayout.Button("Play")) manager.PlayVfx(_vfx.Item1, _vfx.Item2);
            }
        }
    }
#endif
}