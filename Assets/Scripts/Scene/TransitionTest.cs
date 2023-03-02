using UnityEngine;
using UnityEngine.UIElements;

namespace Scene
{
    public class TransitionTest : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        private void Start()
        {
            var button = uiDocument.rootVisualElement.Q<Button>("StartButton");
            button.clicked += () => { SceneLoader.Instance.TransitionScene("TestScene2"); };
        }
    }
}