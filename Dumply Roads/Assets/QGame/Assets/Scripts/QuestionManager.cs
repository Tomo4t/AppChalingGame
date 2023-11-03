using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public List<QuestionData> questions;
    private List<QuestionData> unusedQuestions;

    private void Start()
    {
        // Create a copy of the questions list to track unused questions.
        unusedQuestions = new List<QuestionData>(questions);
    }

    public QuestionData GetRandomQuestion()
    {
        if (unusedQuestions.Count > 0)
        {
            int randomIndex = Random.Range(0, unusedQuestions.Count);
            QuestionData randomQuestion = unusedQuestions[randomIndex];

            // Remove the used question from the list.
            unusedQuestions.RemoveAt(randomIndex);

            return randomQuestion;
        }
        else
        {
            Debug.LogWarning("No more questions available.");
            return null;
        }
    }
}
