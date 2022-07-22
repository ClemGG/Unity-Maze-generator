using UnityEngine;


namespace Project.Procedural.MazeGeneration
{
    public abstract class MazeGenerator : MonoBehaviour, IDemo
    {
        [field: SerializeField] public GenerationSettingsSO Settings { get; set; }
        public Grid Grid { get; set; }
        public IDraw DrawMethod { get; set; }


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

        public abstract void Draw();
    }
}