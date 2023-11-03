using UnityEngine;

public enum QuestionType { MultipleChoice, YesNo }

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/Question")]
public class QuestionData : ScriptableObject

{
    public Sprite PreViewImage;

    public QuestionType questionType;
    public string questionText;
    
    
    public string[] WrongAnswers;
    public string RightAnswer;

    public bool AnswerIs;



}
