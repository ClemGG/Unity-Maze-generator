using UnityEngine;


namespace Project.Procedural.MazeGeneration
{
    public abstract class MazeGenerator : MonoBehaviour, IDemo<object>
    {
        [field: SerializeField] public GenerationSettingsSO Settings { get; set; }
        public IGrid Grid { get; set; }
        public IDrawMethod<object> DrawMethod { get; set; }


        [ContextMenu("Cleanup")]
        public void Cleanup()
        {
            if(DrawMethod is not null)
            {
                DrawMethod.Cleanup();
            }
        }


        [ContextMenu("Execute Generation Algorithm")]
        public void Execute()
        {
            SetupGrid();
            Generate();
            Draw();

        }

        public abstract void SetupGrid();

        public abstract void Generate();

        public virtual void Draw()
        {
            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            DrawMethod = InterfaceFactory.GetDrawMode(Settings);
            DrawMethod.Draw(Grid);
        }
    }
}