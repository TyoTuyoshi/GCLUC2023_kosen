using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using Utils;

namespace Particle
{
    public class ParticleManager : MonoBehaviour
    {
        [SerializeField] private AddressableAssetGroup vfxGroup;
        public AddressableAssetGroup VfxGroup => vfxGroup;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ParticleManager))]
    public class ParticleManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var manager = (target as ParticleManager)!;

            if (Application.isPlaying || manager.VfxGroup == null) return;

            if (GUILayout.Button("Detect Vfx"))
                EnumCreator.CreateSource(manager.VfxGroup.entries.Select(v => v.address),
                    "Assets/Plugins/AutoGenerate/VfxEnum.cs");
        }
    }
#endif
}