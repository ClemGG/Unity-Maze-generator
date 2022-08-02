
/* This class holds the progress status for the current async task.
 * Used to report the progress rate for the drawing of the maze.
 */
using System;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class GenerationProgressReport
    {
        //From 0 to 1
        public float ProgressPercentage { get; set; } = 0f;
        public float TimeElapsed { get; set; } = 0f;
        private string ReportName { get; set; } = "Algorithm"; //Don't forget to assign this

        public GenerationProgressReport(string reportName = "Algorithm")
        {
            ReportName = reportName;
        }


        public void UpdateTrackTime(float frameDuration = 0f)
        {
            TimeElapsed += frameDuration;
        }

        //Use these if you only want to track time at the start and end of the Coroutine.
        //Otherwise, track it manually with UpdateTrackTime()
        public void StartTrackTime()
        {
            TimeElapsed = Time.time;
        }
        public void StopTrackTime()
        {
            TimeElapsed = Time.time - TimeElapsed;
        }


        public override string ToString()
        {
            return string.Format($"{ReportName} completion: {ProgressPercentage * 100f}%; Time elapsed: {TimeElapsed:0.00}s");
        }
    }
}