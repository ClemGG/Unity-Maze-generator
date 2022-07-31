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

        [ContextMenu("Cleanup (also progress)")]
        public new void Cleanup()
        {
            base.Cleanup();
            ProgressVisualizer.Cleanup();
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


        public override void Draw()
        {
            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            DrawMethod = InterfaceFactory.GetDrawMode(Settings);

            Progress = new();
            Progress.ProgressChanged += DisplayProgress;
            DrawMethod.DrawAsync(Grid, Progress);
        }

        private void DisplayProgress(object sender, GenerationProgressReport e)
        {
            ProgressVisualizer.DisplayProgress(e);
        }
    }
}