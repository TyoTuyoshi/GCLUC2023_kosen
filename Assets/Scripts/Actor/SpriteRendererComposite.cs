using System.Linq;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using AssignAction = System.Action<Actor.SpriteRendererComposite, UnityEngine.SpriteRenderer[]>;
#endif

namespace Actor
{
    /// <summary>
    ///     SpriteRendererの描画順序をまとめる
    /// </summary>
    public class SpriteRendererComposite : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] renderers;
        [SerializeField] private int[] _defaultOrder;
        [SerializeField] private int _afterOrder;

        public int SortingOrder
        {
            get => renderers.First().sortingOrder;
            set
            {
                _afterOrder = value;
                var diff = _defaultOrder.Last() - _afterOrder;
                foreach (var ele in _defaultOrder.Zip(renderers, (order, spriteRenderer) => (order, spriteRenderer)))
                    ele.spriteRenderer.sortingOrder = ele.order - diff;
            }
        }

        private void Start()
        {
            _defaultOrder = renderers.OrderBy(v => v.sortingOrder).Select(v => v.sortingOrder).ToArray();
            renderers = renderers.OrderBy(v => v.sortingOrder).ToArray();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SpriteRendererComposite))]
    public class SpriteRendererCompositeEditor : Editor
    {
        private AssignAction _func;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var comp = (SpriteRendererComposite)target;
            if (GUILayout.Button("Detect Renderer"))
            {
                if (_func == null)
                {
                    var targetParam = Expression.Parameter(typeof(SpriteRendererComposite), "target");
                    var valueParam = Expression.Parameter(typeof(SpriteRenderer[]), "value");
                    var body = Expression.Assign(Expression.PropertyOrField(targetParam, "renderers"), valueParam);
                    var lambda = Expression.Lambda<AssignAction>(body, targetParam, valueParam);
                    _func = lambda.Compile();
                }

                _func(comp, comp.GetComponentsInChildren<SpriteRenderer>());
            }
        }
    }
#endif
}