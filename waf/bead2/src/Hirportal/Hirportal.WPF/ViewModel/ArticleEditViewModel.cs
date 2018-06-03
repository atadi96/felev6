using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hirportal.Persistence.DTO;
using Hirportal.WPF.Persistence;

namespace Hirportal.WPF.ViewModel
{
    class ArticleEditViewModel : ViewModelBase
    {
        private INewsPersistence model;

        private bool isReady;

        public bool IsReady
        {
            get => isReady;
            private set
            {
                isReady = value;
                OnPropertyChanged();
            }
        }

        private ArticleUploadDTO article;

        public string Title
        {
            get => article.Title;
            set { article.Title = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => article.Description;
            set { article.Description = value; OnPropertyChanged(); }
        }

        public string Content
        {
            get => article.Content;
            set { article.Content = value; OnPropertyChanged(); }
        }

        public bool DeleteImages
        {
            get => article.DeleteImages;
            set { article.DeleteImages = value; OnPropertyChanged(); }
        }

        private ObservableCollection<byte[]> images;

        public ObservableCollection<byte[]> Images
        {
            get => images;
            private set { images = value; OnPropertyChanged(); }
        }

        private ObservableCollection<byte[]> newImages;

        public ObservableCollection<byte[]> NewImages
        {
            get => newImages;
            private set
            {
                newImages = value;
                article.NewImages = newImages.ToArray();
                OnPropertyChanged();
            }
        }

        public bool Leading
        {
            get => article.Leading;
            set { article.Leading = value; OnPropertyChanged(); }
        }

        public DelegateCommand BackCommand { get; private set; }

        public EventHandler BackEvent { get; set; }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand AddImageCommand { get; private set; }

        public ArticleEditViewModel(INewsPersistence model, int? articleID = null)
        {
            this.model = model;
            article = new ArticleUploadDTO();
            BackCommand = new DelegateCommand(_ => BackEvent?.Invoke(this, EventArgs.Empty));
            IsReady = false;
            FetchArticle(articleID);
        }

        private async void Save()
        {
            IsReady = false;
            bool result = false;
            try
            {
                if (article.Id < 1)
                {
                    result = await model.CreateArticleAsync(article);
                }
                else
                {
                    result = await model.UpdateArticleAsync(article);
                }

                if (result)
                {
                    OnMessageApplication("Save successful!");
                }
                else
                {
                    OnMessageApplication("Save refused by the server!");
                }
            }
            catch (PersistenceUnavailableException ex)
            {
                PersistenceError(ex);
            }
            IsReady = true;
        }

        private async void FetchArticle(int? articleID = null)
        {
            if (articleID == null)
            {
                article.Id = -1;
                Title = "";
                Description = "";
                Content = "";
                DeleteImages = false;
                NewImages = new ObservableCollection<byte[]>();
                Images = new ObservableCollection<byte[]>();
                IsReady = true;
            }
            else
            {
                try
                {
                    var articleDTO = await model.GetArticleAsync(articleID.Value);
                    Title = articleDTO.Title;
                    Description = articleDTO.Description;
                    Content = articleDTO.Content;
                    DeleteImages = false;
                    NewImages = new ObservableCollection<byte[]>();
                    Images = new ObservableCollection<byte[]>(articleDTO.Images.Select(img => img.Data));
                    IsReady = true;
                }
                catch (PersistenceUnavailableException ex)
                {
                    PersistenceError(ex);
                    IsReady = true;
                }
            }
            
        }

        private void PersistenceError(PersistenceUnavailableException ex)
        {
            OnMessageApplication($"Persistence unavailable: {ex.Message}");
        }
    }
}
