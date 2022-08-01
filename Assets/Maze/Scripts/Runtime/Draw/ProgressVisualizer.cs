using Project.Procedural.MazeGeneration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ProgressVisualizer
{
    #region UI Fields
    private static Slider _progressFill;
    private static TextMeshProUGUI _progressText;

    private static Slider ProgressFill
    {
        get
        {
            if (!_progressFill) _progressFill = GameObject.Find("progress bar").GetComponent<Slider>();
            return _progressFill;
        }
    }
    private static TextMeshProUGUI ProgressText
    {
        get
        {
            if (!_progressText) _progressText = GameObject.Find("progress text").GetComponent<TextMeshProUGUI>();
            return _progressText;
        }
    }

    #endregion



    public void DisplayDrawProgress(GenerationProgressReport progress)
    {
        ProgressFill.value = progress.ProgressPercentage;
        ProgressText.text = $"Generation completion: {progress.ProgressPercentage * 100f}% ; Time elapsed: {progress.TimeElapsed}s";
    }

    public void Cleanup()
    {
        ProgressFill.value = 0f;
        ProgressText.text = "";
    }
}
