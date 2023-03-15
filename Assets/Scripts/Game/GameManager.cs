using MapSelection;
using Scene;
using UnityEngine;
using Utils;

namespace Game
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public float PrevStageTime;

        public void GoToResult()
        {
            GameObject.FindWithTag("Stage").TryGetComponent(out IStage stage);
            PrevStageTime = stage.Elapsed;
            SceneLoader.Instance.TransitionScene("ResultScene");
        }
    }
}