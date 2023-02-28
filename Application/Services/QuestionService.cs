using Application.Commons;
using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.QuizModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsservice;
        private readonly AppConfiguration _configuration;
        //public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        //{
        //    _unitOfWork = unitOfWork;
        //    _mapper = mapper;
        //}
        //public QuestionService(IUnitOfWork unitOfWork, IClaimsService claimsService)
        //{
        //    _unitOfWork = unitOfWork;
        //    _claimsservice = claimsService;
        //}
        public QuestionService(IUnitOfWork unitofwork, IClaimsService claimsservice, IUnitOfWork unitOfWork, IMapper mapper, AppConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _claimsservice = claimsservice;
        }


        public async Task<List<Question>> Filter(ICollection<Guid> TopicList, ICollection<int> QuizType)
        {
            List<Question> quizBanks1 = await _unitOfWork.QuestionRepository.GetAllAsync();
            List<Question> quizBanks = new List<Question>();
            List<Question> quizType = new List<Question>();
            //throwlis new NotImplementedException();
            //var topicList = await _unitOfWork.QuizBankRepository.FilterQuizTestWithTopic(TopicList);
            foreach (var quizTest in QuizType)
            {
                Question comsuon = quizBanks1.Find(x => x.QuizTypeID == quizTest);
                quizType.Add(comsuon);
            }
            foreach (var quizTest in TopicList)
            {
                Question comsuon = quizBanks1.Find(x => x.TopicID == quizTest);
                quizBanks.Add(comsuon);
            }
            //var quizType = await _unitOfWork.QuizBankRepository.FilterQuizTestWithType(QuizType);
            List<Question> lastchane = new List<Question>();

            foreach (var topic in quizBanks)
            {
                foreach (var quiz in quizType)
                {
                    if (topic == quiz)
                    {
                        lastchane.Add(topic);
                    }
                }

            }

            return lastchane;
        }

        public async Task<IEnumerable<Question>> Search(string SearchName)
        {
            //throw new NotImplementedException();
            var searchByName = await _unitOfWork.QuestionRepository.FindAsync(x => x.Content.Contains(SearchName));
            if (searchByName is null)
            {
                throw new Exception();

            }
            return searchByName;

        }

        public async Task<bool> AddQuestionToBank(CreateQuizIntoBankDTO quizDto)
        {
            // check correst answer exist in 4 answer
            if (quizDto.CorrectAnswer != quizDto.Answer1 &&
                quizDto.CorrectAnswer != quizDto.Answer2 &&
                quizDto.CorrectAnswer != quizDto.Answer3 &&
                quizDto.CorrectAnswer != quizDto.Answer4)
            {
                return false;
            }
            // check topic exist if null retrun false
            var checkTopicExist = await _unitOfWork.TopicRepository.GetByIdAsync(quizDto.TopicID);
            if (checkTopicExist == null)
            {
                return false;
            }
            // Create new question, dont check same content
            var newQuestion = new Question
            {
                Content = quizDto.Content,
                Answer1 = quizDto.Answer1,
                Answer2 = quizDto.Answer2,
                Answer3 = quizDto.Answer3,
                Answer4 = quizDto.Answer4,
                CorrectAnswer = quizDto.CorrectAnswer,
                TopicID = quizDto.TopicID,
                QuizTypeID = quizDto.TypeID,
                // CreationDate = DateTime.Now.Date,
                //CreatedBy = _claimsservice.GetCurrentUserId
            };
            await _unitOfWork.QuestionRepository.AddAsync(newQuestion);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> CreateEmptyQuizTest(CreateEmptyQuizDTO quizDto)
        {
            // check lecture exist
            /*var checkLecture = await _unitOfWork.LectureRepository.GetByIdAsync(quizDto.LectureID);
            if (checkLecture == null)
            {
                return false;
            }*/

            // create quiz test
            var newQuiz = new Quiz
            {
                NumberOfQuiz = 0,
                LectureID = quizDto.LectureID,
            };
            await _unitOfWork.QuizRepository.AddAsync(newQuiz);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> AddQuestionToQuizTest(AddQuestionToQuizTestDTO quizDto)
        {
            var newQuestionToTest = new DetailQuizQuestion
            {
                QuizID = quizDto.QuizId,
                QuestionID = quizDto.QuestionId,
                SubmitQuizID = Guid.NewGuid(),  // tam thoi fix cung, quizDto.SubmitQuiz, 
            };
            // count +1 numberOfQuiz when 1 question add to quiz
            var getQuizTest = await _unitOfWork.QuizRepository.GetByIdAsync(quizDto.QuizId);
            getQuizTest.NumberOfQuiz += 1;
            _unitOfWork.QuizRepository.Update(getQuizTest);

            await _unitOfWork.DetailQuizQuestionRepository.AddAsync(newQuestionToTest);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> UpdateQuizTest(Guid quizTestId, UpdateQuizTestDTO quizDto)
        {
            // check quiz test exist
            var checkQuizTestExist = await _unitOfWork.QuizRepository.GetByIdAsync(quizTestId);
            var checkCurrentQuestion = await _unitOfWork.QuestionRepository.GetByIdAsync(quizDto.IdQuestionWantToUpdate);
            var checkNewQuestion = await _unitOfWork.QuestionRepository.GetByIdAsync(quizDto.NewQuestionId);
            if (checkQuizTestExist == null || checkCurrentQuestion == null || checkNewQuestion == null)
            {
                return false;
            }
            // get all question of quiz test
            var allQuestionOfQuiz = await _unitOfWork.QuizRepository.GetAllQuestionByQuizTestId(quizTestId);
            bool check = false;
            foreach (var item in allQuestionOfQuiz)
            {           
                //update
                if (item.QuestionID == quizDto.IdQuestionWantToUpdate)
                {
                    item.QuestionID = quizDto.NewQuestionId;
                    _unitOfWork.DetailQuizQuestionRepository.Update(item);
                    check = true;
                    break;
                }
            }
            if (!check) return false;
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> DeleteQuizTest(Guid quizTestId)
        {
            // check quiz test exist
            var checkQuizTestExist = await _unitOfWork.QuizRepository.GetByIdAsync(quizTestId);
            if (checkQuizTestExist == null)
            {
                return false;
            }
            _unitOfWork.QuizRepository.SoftRemove(checkQuizTestExist);

            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}
