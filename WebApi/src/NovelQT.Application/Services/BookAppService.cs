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
    public class BookAppService : IBookAppService
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IChapterRepository _chapterRepository;

        public BookAppService(
            IMapper mapper,
            IMediatorHandler bus,
            IEventStoreRepository eventStoreRepository,
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository,
            IChapterRepository chapterRepository
            )
        {
            _mapper = mapper;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _chapterRepository = chapterRepository;
        }

        public void Crawl(string url)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.71 Safari/537.36");

            try
            {
                // Thực hiện truy vấn GET
                HttpResponseMessage response = httpClient.GetAsync(url).Result;

                // Hiện thị thông tin header trả về
                //ShowHeaders(response.Headers);

                // Phát sinh Exception nếu mã trạng thái trả về là lỗi
                response.EnsureSuccessStatusCode();

                // Đọc nội dung content trả về - ĐỌC CHUỖI NỘI DUNG
                string htmltext = WebUtility.HtmlDecode(response.Content.ReadAsStringAsync().Result);
                var info = Regex.Match(htmltext, @"(?=<div class=""book-information cf"").*?(?<=<div class=""content-nav-wrap cf"">)", RegexOptions.Singleline);

                var book = new BookViewModel();
                
                book.Name = Regex.Match(info.Value, @"(?<=<h1>).*?(?=<\/h1>)", RegexOptions.Singleline).Value.ToString();

                string bookAuthor = Regex.Match(info.Value, @"(?<=class=""blue"">).*?(?=<\/a>)").Value.ToString();

                book.Cover = Regex.Match(info.Value, @"(?<=src="").*?(?="")").Value.ToString();

                book.Status = Regex.Match(info.Value, @"(?<=<span class=""blue"">).*?(?=<\/span>)").Value.ToString();

                string bookCategoryTemp = Regex.Match(info.Value, @"(?<=the-loai).*?(?<=<\/a>)", RegexOptions.Singleline).Value.ToString();
                string bookCategory = Regex.Match(bookCategoryTemp, @"(?<="">).*?(?=<\/a>)", RegexOptions.Singleline).Value.ToString();

                book.Like = int.Parse(Regex.Match(info.Value, @"(?<=ULtwOOTH-like"">).*?(?= <\/span>)").Value.ToString());

                book.View = int.Parse(Regex.Match(info.Value, @"(?<=ULtwOOTH-view"">).*?(?=<\/span>)").Value.ToString());

                book.Key = Regex.Match(info.Value, @"(?<=likeStory\(').*?(?=')").Value.ToString();


                var author = _mapper.Map<AuthorViewModel>(_authorRepository.GetByName(bookAuthor));
                if (author == null)
                {
                    var registerAuthorCommand = _mapper.Map<RegisterNewAuthorCommand>(new AuthorViewModel(bookAuthor));
                    Bus.SendCommand(registerAuthorCommand);
                }
                book.AuthorId = _mapper.Map<AuthorViewModel>(_authorRepository.GetByName(bookAuthor)).Id;

                var category = _mapper.Map<CategoryViewModel>(_categoryRepository.GetByName(bookCategory));
                if (category == null)
                {
                    var registerCategoryCommand = _mapper.Map<RegisterNewCategoryCommand>(new CategoryViewModel(bookCategory));
                    Bus.SendCommand(registerCategoryCommand);
                }
                book.CategoryId = _mapper.Map<CategoryViewModel>(_categoryRepository.GetByName(bookCategory)).Id;
                
                

                var bookInDatabase = _mapper.Map<BookViewModel>(_bookRepository.GetByKey(book.Key));
                if (bookInDatabase == null)
                {
                    book.IndexStatus = IndexStatusEnum.ScheduledIndex;

                    var registerBookCommand = _mapper.Map<RegisterNewBookCommand>(book);
                    Bus.SendCommand(registerBookCommand);
                    bookInDatabase = _mapper.Map<BookViewModel>(_bookRepository.GetByKey(book.Key));
                }
                else
                {
                    // TODO: Update
                }
                


                response = httpClient.GetAsync("https://truyen.tangthuvien.vn/story/chapters?story_id=" + book.Key).Result;
                string htmlList = WebUtility.HtmlDecode(response.Content.ReadAsStringAsync().Result);



                var chapterList = Regex.Matches(htmlList, @"(?=<li).*?(?=<\/li>)", RegexOptions.Singleline);
                foreach (var chapter in chapterList)
                {
                    var chapterViewModel = new ChapterViewModel();

                    chapterViewModel.BookId = bookInDatabase.Id;

                    chapterViewModel.Order = int.Parse(Regex.Match(chapter.ToString(), @"(?<=title="").*?(?="" ng-chap)").Value.ToString());
                    chapterViewModel.Name = Regex.Match(chapter.ToString(), @"(?<=chapter-text"">).*?(?=<\/span>)").Value.ToString();
                    chapterViewModel.Url = Regex.Match(chapter.ToString(), @"(?<=href="" ).*?(?= "")").Value.ToString();

                    var chapterInDatabase = _mapper.Map<ChapterViewModel>(_chapterRepository.GetByBookIdAndOrder(chapterViewModel.BookId, chapterViewModel.Order));
                    if (chapterInDatabase == null)
                    {
                        response = httpClient.GetAsync(chapterViewModel.Url).Result;
                        string htmlChapter = WebUtility.HtmlDecode(response.Content.ReadAsStringAsync().Result);
                        string chapterContentTemp = Regex.Match(htmlChapter, @"(?=box-chap box-chap).*?(?<=<\/div>)", RegexOptions.Singleline).Value.ToString();
                        chapterViewModel.Content = Regex.Match(chapterContentTemp, @"(?<=>).*?(?=<\/div>)", RegexOptions.Singleline).Value.ToString();

                        chapterViewModel.IndexStatus = IndexStatusEnum.ScheduledIndex;

                        var registerChapterCommand = _mapper.Map<RegisterNewChapterCommand>(chapterViewModel);
                        Bus.SendCommand(registerChapterCommand);
                    }

                }


            }
            catch (Exception ex)
            {
            }
        }

        public BookResponse GetById(Guid id)
        {
            return _mapper.Map<BookResponse>(_bookRepository.GetById(id));
        }

        public IEnumerable<BookResponse> GetAll()
        {
            return _bookRepository.GetAll().ProjectTo<BookResponse>(_mapper.ConfigurationProvider);
        }

        public RepositoryResponses<BookResponse> GetAll(int skip, int take, string query)
        {
            var books = _bookRepository.GetAll(new BookFilterPaginatedSpecification(skip, take, query));
            var bookResponses = books.ProjectTo<BookResponse>(_mapper.ConfigurationProvider);
            
            return new RepositoryResponses<BookResponse>(bookResponses, this.GetAll().Count());
            //return _bookRepository.GetAll(new BookFilterPaginatedSpecification(skip, take))
            //    .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
