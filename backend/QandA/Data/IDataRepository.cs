﻿using QandA.Models;

namespace QandA.Data
{
    public interface IDataRepository
    {
        IEnumerable<QuestionGetManyResponse> GetQuestions();
        IEnumerable<QuestionGetManyResponse> GetQuestionsWithAnswers();
        IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search);
        IEnumerable<QuestionGetManyResponse> GetQuestionsBySearchWithPaging(string search, int pageNumber, int pageSize);
        IEnumerable<QuestionGetManyResponse> GetUnansweredQuestions();
        Task<IEnumerable<QuestionGetManyResponse>> GetUnansweredQuestionsAsync();
        QuestionGetSingleResponse GetQuestion(int questionId);
        Task<QuestionGetSingleResponse> GetQuestionAsync(int questionId);
        bool QuestionExists(int questionId);
        AnswerGetResponse GetAnswer(int answerId);
        QuestionGetSingleResponse PutQuestion(int questionId,
QuestionPutRequest question);

        void DeleteQuestion(int questionId);

        AnswerGetResponse PostAnswer(AnswerPostFullRequest answer);
        QuestionGetSingleResponse PostQuestion(QuestionPostFullRequest questionPostRequest);
        Task<QuestionGetSingleResponse> PostQuestionAsync(QuestionPostFullRequest question);
    }
}
