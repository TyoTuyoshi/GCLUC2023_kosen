using System.Collections.Generic;
using System.Linq;
using AutoGenerate;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
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
        private const int MaxPooledInstance = 10;
        private static ParticleManager _instance;
        [SerializeField] private AssetReferenceT<VisualEffect>[] vfxRefs;
        private readonly Dictionary<VfxEnum, Stack<VisualEffect>> _pool = new();

        public static ParticleManager Instance => _instance ??= FindObjectOfType<ParticleManager>();

        private void OnDestroy()
        {
            _instance = null;
        }

        public void PlayVfx(VfxEnum vfxType, float durationSec, Vector3 pos = default, Quaternion rot = default)
        {
            UniTask.Create(async () =>
            {
                var vfx = await GetVfx(vfxType, pos, rot);
                vfx.Play();
                await DOVirtual.DelayedCall(durationSec, () => ReturnVfx(vfxType, vfx)).SetLink(gameObject);
            });
        }

        private void ReturnVfx(VfxEnum vfxType, VisualEffect vfx)
        {
            if (_pool[vfxType].Count >= MaxPooledInstance)
                Addressables.ReleaseInstance(vfx.gameObject);
            else
                _pool[vfxType].Push(vfx);
        }

        private async UniTask<VisualEffect> GetVfx(VfxEnum vfxType, Vector3 pos = default, Quaternion rot = default)
        {
            while (true)
            {
                if (!_pool.ContainsKey(vfxType)) _pool[vfxType] = new Stack<VisualEffect>(MaxPooledInstance);
                var vfxPool = _pool[vfxType];

                if (vfxPool.TryPop(out var pooledInstance))
                {
                    if (pooledInstance == null) continue;

                    var instanceTrans = pooledInstance.transform;
                    instanceTrans.position = pos;
                    instanceTrans.rotation = rot;
                    return pooledInstance;
                }

                var obj = await Addressables.InstantiateAsync(vfxRefs[(int)vfxType], pos, rot);
                obj.OnDestroyAsObservable().Subscribe(_ => Addressables.ReleaseInstance(obj)).AddTo(obj);
                obj.TryGetComponent(out VisualEffect instance);
                return instance;
            }
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