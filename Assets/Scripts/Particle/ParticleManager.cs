using System;
using System.Linq;
using AutoGenerate;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;
using Utils;

namespace Particle
{
    public class ParticleManager : MonoBehaviour
    {
        private static ParticleManager _instance;
        [SerializeField] private AddressableAssetGroup vfxGroup;
        private AssetReferenceT<VisualEffect>[] _vfxRefs;
        public AddressableAssetGroup VfxGroup => vfxGroup;

        public static ParticleManager Instance => _instance ??= FindObjectOfType<ParticleManager>();

        private void Awake()
        {
            _vfxRefs = vfxGroup.entries.Select(e => new AssetReferenceT<VisualEffect>(e.guid)).ToArray();
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        public async UniTask PlayVfx(VfxEnum vfxType, float durationSec, Vector3 pos = default, Quaternion rot = default)
        {
            var assetRef = _vfxRefs[(int)vfxType];
            var vfx = await Addressables.InstantiateAsync(assetRef, pos, rot);
            await UniTask.Delay(TimeSpan.FromSeconds(durationSec));
            assetRef.ReleaseInstance(vfx);
        }
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
                if (GUILayout.Button("Play")) UniTask.Create(async () => await manager.PlayVfx(_vfx.Item1, _vfx.Item2));
            }
        }
    }
#endif
}