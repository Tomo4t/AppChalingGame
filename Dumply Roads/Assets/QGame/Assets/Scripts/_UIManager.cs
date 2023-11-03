using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class _UIManager : MonoBehaviour
{
    public TMP_Text questionText;
    public TMP_Text[] answerTexts;
    public Image Qimage;
    public Button[] answerButtons;
    
    public GameManager gameManager; // Assign the GameManager script in the Inspector.

    private QuestionManager questionManager;
    private QuestionData currentQuestion;

    private void Start()
    {
        questionManager = FindObjectOfType<QuestionManager>();
        
        LoadNextQuestion();
    }

    public void LoadNextQuestion()
    {
        if (questionManager != null)
        {
            currentQuestion = questionManager.GetRandomQuestion();

            if (currentQuestion != null)
            {
                questionText.text = currentQuestion.questionText;
                if (currentQuestion.PreViewImage != null)
                {
                    Qimage.sprite = currentQuestion.PreViewImage;
                }

                string[] answerChoices = new string[currentQuestion.WrongAnswers.Length + 1];
                System.Array.Copy(currentQuestion.WrongAnswers, answerChoices, currentQuestion.WrongAnswers.Length);
                answerChoices[answerChoices.Length - 1] = currentQuestion.RightAnswer;
                ShuffleArray(answerChoices);

                for (int i = 0; i < answerTexts.Length; i++)
                {
                    if (i < answerChoices.Length)
                    {
                        answerTexts[i].text = answerChoices[i];

                        // Assign the answer button's text.
                        answerButtons[i].GetComponentInChildren<TMP_Text>().text = answerChoices[i];

                        // Add a click event to the button.
                        int buttonIndex = i; // Capture the index in a local variable.
                        answerButtons[i].onClick.RemoveAllListeners(); // Clear previous listeners.
                        answerButtons[i].onClick.AddListener(() => AnswerButtonClicked(buttonIndex));
                    }
                    else
                    {
                        answerTexts[i].gameObject.SetActive(false);
                        answerButtons[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                questionText.text = "No more questions.";
               
            }
        }
    }




    public void AnswerButtonClicked(int buttonIndex)
    {
        string selectedAnswer = answerTexts[buttonIndex].text;

        if (selectedAnswer == currentQuestion.RightAnswer)
        {
            // Handle correct answer logic here.
            Debug.Log("Correct Answer!");
            gameManager.AnswerCorrect(); // Increase the player's score.

            LoadNextQuestion();
            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].interactable = true;
            }
        }
        else
        {
            // Handle incorrect answer logic here.
            Debug.Log("Incorrect Answer!");

            gameManager.AnswerUnCorrect();

            answerButtons[buttonIndex].interactable = false; // Disable answer
        }

        
       


    }

    private void ShuffleArray(string[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            string temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
