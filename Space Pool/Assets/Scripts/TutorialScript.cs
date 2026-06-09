using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Scrollbar sb;
    public int totalSteps = 5;
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI progressionText;
    public TextMeshProUGUI headerText;
    public Button menuButton;
    public int currentStep = -1;

    public List<string> headerList = new List<string>()
    {
        "What is Space Ball Pool?",
        "Rules",
        "Basic Controls",
        "Shooting",
        "Scoring/Game Over",
        "Tutorial: "
    };

    public List<string> tutorialList = new List<string>
{
        "Space Ball Pool is a 3D take on classic pool, played in zero gravity. Aim, shoot, and sink balls across a floating arena, following the same core rules as traditional pool. Your objective is to clear your assigned color balls and then sink the Black Hole Ball to win.",
        "The game follows standard pool logic. Once the first ball is scored, you’re assigned either Color 1 balls or Color 2 balls. Only your color can be sunk until you’ve cleared them all. The Black Hole Ball is your final shot—sink it too early and you lose.",
        "Use the arrow keys to move and rotate the camera. Click and drag with the mouse to aim and line up your shot. Scroll to zoom in or out for a better view of the table.",
        "Click and drag backward to charge your shot—the farther you pull, the stronger your hit. Release to fire the cue ball through space and send the other balls flying.",
        "Sink your assigned color balls into the corner scoring zones to remove them from the field. Once all your color balls are cleared, aim for the Black Hole Ball. The first player to sink it after clearing their set wins the match."
    };


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (sb == null)
        {
            return;
        }

        int step = Mathf.FloorToInt(sb.value * totalSteps) + 1;

        step = (step == 6) ? 5 : step; // Clamp to max step, weird behavior at max value

        if (step != currentStep)
        {
            currentStep = step;
            DisplayTutorialText();
            DisplayProgressionText();
        }

    }

    public void DisplayTutorialText()
    {
        headerText.text = headerList[5] + headerList[currentStep - 1];
        tutorialText.text = tutorialList[currentStep - 1];
    }

    public void DisplayProgressionText()
    {
        progressionText.text = $"({currentStep}/{totalSteps})";

        if (currentStep == 5)
        {
            menuButton.GetComponentInChildren<TextMeshProUGUI>().text = "I'm Ready!";
        }
        else
        {
            menuButton.GetComponentInChildren<TextMeshProUGUI>().text = "Main Menu";
        }
    }
}
