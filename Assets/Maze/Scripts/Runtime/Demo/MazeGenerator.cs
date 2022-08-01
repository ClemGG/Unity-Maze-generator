using UnityEngine;


namespace Project.Procedural.MazeGeneration
{
    public abstract class MazeGenerator : MonoBehaviour, IDemo
    {
        [field: SerializeField] public GenerationSettingsSO Settings { get; set; }
        public IDrawableGrid Grid { get; set; }
        public IDrawMethod DrawMethod { get; set; }

        [ContextMenu("Cleanup Sync")]
        public void Cleanup()
        {
            if(DrawMethod is not null)
            {
                DrawMethod.Cleanup();
            }
        }


        [ContextMenu("Execute Sync")]
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
            DrawMethod.DrawSync(Grid);
        }
    }
}