using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;

using Hirportal.Persistence.DTO;

namespace Hirportal.WPF.Model
{
    public class ModelAsyncCompletedEventArgs<T> : AsyncCompletedEventArgs
    {
        ModelAsyncCompletedEventArgs(Exception ex, bool cancelled) : base(ex, cancelled, null)
        {

        }

        ModelAsyncCompletedEventArgs(T result) : base(null, false, null)
        {
            _data = result;
        }

        private T _data;

        public T Data
        {
            get
            {
                RaiseExceptionIfNecessary();
                return _data;
            }
        }
    }

    public interface INewsModel
    {

        IReadOnlyList<ArticlePreviewDTO> ArticlePreviews { get; }

        AuthorDTO LoggedInUser { get; }

        bool IsBusy { get; }

        void LoadPreviewsAsync();

        EventHandler<ModelAsyncCompletedEventArgs<IReadOnlyList<ArticlePreviewDTO>>> LoadPreviewsAsyncFinished { get; set; }

        void GetArticleAsync(int id);

        void CreateArticleAsync(ArticleDTO article);

        void UpdateArticleAsync(ArticleDTO article);

        EventHandler<ModelAsyncCompletedEventArgs<ArticleDTO>> ArticleLoadFinished { get; set; }

        void DeleteArticleAsync(ArticleDTO building);

        EventHandler<ModelAsyncCompletedEventArgs<bool>> ArticleDeleteFinished { get; set; }

        void LoginAsync(String userName, String userPassword);

        EventHandler<ModelAsyncCompletedEventArgs<AuthorDTO>> LoginAsyncFinished { get; set; }

        void LogoutAsync();

        EventHandler<ModelAsyncCompletedEventArgs<bool>> LogoutAsyncFinished { get; set; }
    }
}
