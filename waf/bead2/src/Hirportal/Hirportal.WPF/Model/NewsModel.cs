using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Hirportal.Persistence.DTO;
using Hirportal.WPF.Persistence;

namespace Hirportal.WPF.Model
{
    public class NewsModel : INewsModel
    {
        private INewsPersistence newsPersistence;

        private object lockObj = new object();

        public NewsModel(INewsPersistence newsPersistence)
        {
            this.newsPersistence = newsPersistence;
        }

        public IReadOnlyList<ArticlePreviewDTO> ArticlePreviews { get; private set; }

        public AuthorDTO LoggedInUser { get; private set; }

        public bool IsBusy { get; private set; }

        public EventHandler<ModelAsyncCompletedEventArgs<IReadOnlyList<ArticlePreviewDTO>>> LoadPreviewsAsyncFinished { get; set; }
        public EventHandler<ModelAsyncCompletedEventArgs<ArticleDTO>> ArticleLoadFinished { get; set; }
        public EventHandler<ModelAsyncCompletedEventArgs<bool>> ArticleDeleteFinished { get; set; }
        public EventHandler<ModelAsyncCompletedEventArgs<AuthorDTO>> LoginAsyncFinished { get; set; }
        public EventHandler<ModelAsyncCompletedEventArgs<bool>> LogoutAsyncFinished { get; set; }

        private bool AcquireBusy()
        {
            lock (lockObj)
            {
                if (IsBusy)
                {
                    return false;
                }
                else
                {
                    IsBusy = true;
                    return true;
                }
            }
        }

        private void ReleaseBusy()
        {
            lock(lockObj)
            {
                IsBusy = false;
            }
        }

        public void CreateArticleAsync(ArticleDTO article)
        {
            if (AcquireBusy())
            {
                
            }
        }

        public void DeleteArticleAsync(ArticleDTO building)
        {
            throw new NotImplementedException();
        }

        public void GetArticleAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void LoadPreviewsAsync()
        {
            throw new NotImplementedException();
        }

        public void LoginAsync(string userName, string userPassword)
        {
            throw new NotImplementedException();
        }

        public void LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public void UpdateArticleAsync(ArticleDTO article)
        {
            throw new NotImplementedException();
        }
    }
}
