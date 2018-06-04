using Hirportal.WPF.Model;
using Hirportal.WPF.Persistence;
using Hirportal.Persistence.DTO;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Hirportal.WPF.ViewModel
{
    /// <summary>
    /// A nézetmodell típusa.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private INewsPersistence _model;
        private ObservableCollection<ArticlePreviewDTO> _articlePreviews;
        private bool _isLoaded;
        private AuthorDTO _author;

        public ObservableCollection<ArticlePreviewDTO> ArticlePreviews
        {
            get { return _articlePreviews; }
            private set
            {
                if (_articlePreviews != value)
                {
                    _articlePreviews = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
            private set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public AuthorDTO Author
        {
            get { return _author; }
            private set
            {
                _author = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand CreateArticleCommand { get; private set; }

        public DelegateCommand EditArticleCommand { get; private set; }

        public DelegateCommand DeleteArticleCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public event EventHandler ExitApplication;

        public event EventHandler CreateArticle;

        public event EventHandler<int> EditArticle;

        public MainViewModel(INewsPersistence model, AuthorDTO author)
        {
            _model = model ?? throw new ArgumentNullException("model");
            _isLoaded = false;
            _author = author;

            DeleteArticleCommand = new DelegateCommand(param => DeleteArticle((int)param));

            EditArticleCommand = new DelegateCommand(param => EditArticleAction((int)param));

            ExitCommand = new DelegateCommand(_ => ExitApplication?.Invoke(this, EventArgs.Empty));

            CreateArticleCommand = new DelegateCommand(_ => CreateArticleAction());

            Refresh();
        }

        private async void DeleteArticle(int articleID)
        {
            IsLoaded = false;
            var x = MessageBox.Show("Are you sure you want to delete the article?", "Delete article", MessageBoxButton.YesNo);
            if (x == MessageBoxResult.Yes)
            {
                try
                {
                    bool result = await _model.DeleteArticleAsync(articleID);
                    MessageBox.Show(result ? "Article deleted successfully." : "Article delete failed!");
                }
                catch (PersistenceUnavailableException ex)
                {
                    PersistenceError(ex);
                }
            }
            Refresh();
        }

        private void CreateArticleAction()
        {
            IsLoaded = false;
            CreateArticle?.Invoke(this, EventArgs.Empty);
            Refresh();
        }

        private void EditArticleAction(int articleID)
        {
            IsLoaded = false;
            EditArticle?.Invoke(this, articleID);
            Refresh();
        }

        private async void Refresh()
        {
            IsLoaded = false;
            ArticlePreviews = new ObservableCollection<ArticlePreviewDTO>(await _model.GetUserArticlesAsync());
            IsLoaded = true;
        }

        private void PersistenceError(PersistenceUnavailableException ex)
        {
            OnMessageApplication($"Persistence unavailable: {ex.Message}");
        }
    }
}
