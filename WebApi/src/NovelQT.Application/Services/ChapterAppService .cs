using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NovelQT.Application.Interfaces;
using NovelQT.Application.Responses;
using NovelQT.Application.ViewModels;
using NovelQT.Domain.Commands.Author;
using NovelQT.Domain.Commands.Book;
using NovelQT.Domain.Commands.Category;
using NovelQT.Domain.Core.Bus;
using NovelQT.Domain.Interfaces;
using NovelQT.Domain.Models;
using NovelQT.Domain.Models.Enum;
using NovelQT.Domain.Specifications;
using NovelQT.Infra.Data.Repository.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NovelQT.Application.Services
{
    public class ChapterAppService : IChapterAppService
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;
        private readonly IChapterRepository _chapterRepository;

        public ChapterAppService(
            IMapper mapper,
            IMediatorHandler bus,
            IEventStoreRepository eventStoreRepository,
            IChapterRepository chapterRepository
            )
        {
            _mapper = mapper;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
            _chapterRepository = chapterRepository;
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ChapterResponse GetChapterByBookIdAndOrder(Guid bookId, int order)
        {
            var chapter = _chapterRepository.GetChapterByBookIdAndOrder(bookId, order);

            return _mapper.Map<ChapterResponse>(chapter);
        }

        public ChapterResponse GetChapterById(Guid id)
        {
            var chapter = _chapterRepository.GetById(id);

            return _mapper.Map<ChapterResponse>(chapter);
        }

        public IEnumerable<ChapterResponse> GetChapterListByBookId(Guid bookId)
        {
            return _chapterRepository.GetChapterListByBookId(bookId).ProjectTo<ChapterResponse>(_mapper.ConfigurationProvider);
        }

        public RepositoryResponses<ChapterResponse> GetChapterListByBookId(Guid bookId, int skip, int take, string query)
        {
            var response = _chapterRepository.GetPagination(new ChapterFilterPaginatedSpecification(bookId, skip*take, take, query));
            var bookResponses = response.Queryable.ProjectTo<ChapterResponse>(_mapper.ConfigurationProvider);

            return new RepositoryResponses<ChapterResponse>(bookResponses, response.TotalRecords);
        }

        public ChapterResponse GetLastChapterByBookId(Guid bookId)
        {
            var chapter = _chapterRepository.GetLastChapterByBookId(bookId);

            return _mapper.Map<ChapterResponse>(chapter);
        }
    }
}
