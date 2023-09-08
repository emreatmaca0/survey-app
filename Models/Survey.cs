namespace anket_kazan.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public string Survey_Name { get; set; }
        public string Survey_sender { get;set; }
        public string Survey_balance { get;set; }
        public List<Question> Question { get; set; }


    }

    public class Question
    {
        public int Id { get; set; }
        public string Question_Text { get; set; }
        public string Answer_One { get; set; }
        public string Answer_Two { get; set; }
        public string Answer_Three { get; set; }
        public string Answer_Four { get; set; }

        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
    }

    public class Answer
    {
        public int Id { get; set; }
        public string Answer_Text { get; set; }

        public int UserId { get; set; }
        public Users User { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
