using System;
using System.Collections;
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
                StartCoroutine(ExecuteAsyncCo());
            }
            else
            {
                Debug.LogError("Error : Execute Async can only be used in Play mode or in a build.");
            }
            
        }

        private IEnumerator ExecuteAsyncCo()
        {
            SetupGrid();
            yield return GenerateAsync();
            DrawAsync();
        }


        public override void SetupGrid()
        {
            if (Settings.AsciiMask || Settings.ImageMask)
            {
                Mask m = Settings.ImageMask ?
                         Mask.FromImgFile(Settings.ImageMask, Settings.Extension) :
                         Settings.AsciiMask ?
                         Mask.FromText(Settings.AsciiMask.name) :
                         new(Settings);
                MaskedGrid mg = new(m);
                Grid = mg;
            }
            else
            {
                Grid = Settings.DrawMode == DrawMode.Console ? new DistanceGrid(Settings) : new ColoredGrid(Settings);
            }
        }

        public override void Generate()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            genAlg.ExecuteSync(Grid);
            Grid.Braid();

            Cell start = Grid.RandomCell();
            Grid.SetDistances(start.GetDistances());

        }
        
        
        public IEnumerator GenerateAsync()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            genAlg.Report = new("Generation");

            Progress = new();
            Progress.ProgressChanged += OnGenerationProgressChanged;
            yield return StartCoroutine(genAlg.ExecuteAsync(Grid, Progress));
            Grid.Braid();

            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            Grid.SetDistances(start.GetDistances());

        }


        public void DrawAsync()
        {
            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            DrawMethod = InterfaceFactory.GetDrawMode(Settings);
            DrawMethod.Report = new("Rendering");

            Progress = new();
            Progress.ProgressChanged += OnDrawProgressChanged;
            StartCoroutine(DrawMethod.DrawAsync(Grid, Progress));
        }

        private void OnDrawProgressChanged(object sender, GenerationProgressReport e)
        {
            ProgressVisualizer.DisplayDrawProgress(e);
            if (Mathf.Approximately(e.ProgressPercentage, 1f))
            {
                OnDrawProgressDone();
            }
        }

        private void OnGenerationProgressChanged(object sender, GenerationProgressReport e)
        {
            ProgressVisualizer.DisplayGenerationProgress(e);
            if (Mathf.Approximately(e.ProgressPercentage, 1f))
            {
                OnGenerationProgressDone();
            }
        }

        private void OnProgressDone()
        {
            OnDrawProgressDone();
            OnGenerationProgressDone();
        }
        private void OnDrawProgressDone()
        {
            Progress.ProgressChanged -= OnDrawProgressChanged;
        }
        private void OnGenerationProgressDone()
        {
            Progress.ProgressChanged -= OnGenerationProgressChanged;
        }
    }
}