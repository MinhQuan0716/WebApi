using Application.ViewModels.QuizModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IQuestionService
    {

        Task<IEnumerable<Question>> Search(string SearchName);

        Task<List<Question>> Filter(ICollection<Guid> TopicList, ICollection<int> QuizType);

        public Task<bool> AddQuestionToBank(CreateQuizIntoBankDTO quizDto);
        public Task<bool> CreateEmptyQuizTest(CreateEmptyQuizDTO quizDto);
        public Task<bool> AddQuestionToQuizTest(AddQuestionToQuizTestDTO quizDto);
        public Task<bool> UpdateQuizTest(Guid quizTestId, UpdateQuizTestDTO quizDto);
        public Task<bool> DeleteQuizTest(Guid quizTestId);

        public Task<DoingQuizDTO> ViewDoingQuiz(Guid QuizID);

        public Task<bool> DoingQuizService(ICollection<AnswerQuizQuestionDTO> answerQuizQuestionDTO);

        public Task<List<ViewDetailResultDTO>> ViewMarkDetail(Guid SubmitQuizID);

        public Task<double> MarkQuiz(Guid QuizID, Guid DetailTrainingDetailTrainingClassParticipateId);

        public Task<AnswerQuizDetailTraineeDTO> ViewDetaildoneQuiz(Guid QuizID);
    }
}
