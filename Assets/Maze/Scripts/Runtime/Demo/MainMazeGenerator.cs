using System;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class MainMazeGenerator : MazeGenerator
    {
        //This class will draw the maze asynchronously.
        //As the maze gets bigger, the game might freeze for a long time.
        //This allows us to mitigate this issue and display the progress on screen.
        private Progress<GenerationProgressReport> Progress { get; set; }
        private ProgressVisualizer ProgressVisualizer { get; set; } = new();

        [ContextMenu("Cleanup Async")]
        public new void Cleanup()
        {
            base.Cleanup();
            StopAllCoroutines();
            ProgressVisualizer.Cleanup();
            OnProgressDone();
        }


        [ContextMenu("Execute Async")]
        public void ExecuteAsync()
        {
            if (Application.isPlaying)
            {
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

            Progress = new();
            Progress.ProgressChanged += OnDrawProgressChanged;
            StartCoroutine(DrawMethod.DrawAsync(Grid, Progress));
        }

        private void OnDrawProgressChanged(object sender, GenerationProgressReport e)
        {
            ProgressVisualizer.DisplayDrawProgress(e);
            if(Mathf.Approximately(e.ProgressPercentage, 1f))
            {
                OnProgressDone();
            }
        }

        private void OnProgressDone()
        {
            Progress.ProgressChanged -= OnDrawProgressChanged;
        }
    }
}