using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class MainMazeGenerator : MazeGenerator
    {
        //This class will draw the maze asynchronously.
        //As the maze gets bigger, the game might freeze for a long time.
        //This allows us to mitigate this issue and display the progress on screen.
        private GenerationProgress Progress { get; set; } = new();
        private ProgressVisualizer ProgressVisualizer { get; set; } = new();

        [ContextMenu("Cleanup Async")]
        public new void Cleanup()
        {
            base.Cleanup();
            StopAllCoroutines();
            ProgressVisualizer.Cleanup();
            OnProgressDone(null);
        }


        [ContextMenu("Execute Async")]
        public void ExecuteAsync()
        {
            if (Application.isPlaying)
            {
                Progress.ProgressChanged += OnDrawProgressChanged;
                Progress.ProgressChanged += OnDrawProgressChanged;
                Progress.ProgressDone += OnProgressDone;

                StopAllCoroutines();
                SetupGrid();
                Generate();
                DrawAsync();
            }
            else
            {
                Debug.LogError("Error : Execute Async can only be used in Play mode or in a build.");
            }

        }

        public override void SetupGrid()
        {
            Grid = Settings.DrawMode == DrawMode.Console ? new DistanceGrid(Settings) : new ColoredGrid(Settings);
        }

        public override void Generate()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            genAlg.Execute(Grid);


            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            Grid.SetDistances(start.GetDistances());

        }


        public void DrawAsync()
        {
            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            DrawMethod = InterfaceFactory.GetDrawMode(Settings);

            StartCoroutine(DrawMethod.DrawAsync(Grid, Progress));
        }

        private void OnDrawProgressChanged(object sender, GenerationProgressReport e)
        {
            ProgressVisualizer.DisplayProgress(e);
        }

        private void OnProgressDone(GenerationProgressReport e)
        {
            Progress.ProgressChanged -= OnDrawProgressChanged;
            Progress.ProgressDone -= OnProgressDone;
        }
    }
}