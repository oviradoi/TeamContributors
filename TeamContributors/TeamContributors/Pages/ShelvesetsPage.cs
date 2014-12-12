using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.MVVM;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamContributors.Base;

namespace TeamContributors.Pages
{
    [TeamExplorerPage(ShelvesetsPage.PageId)]
    public class ShelvesetsPage : TeamExplorerBasePage
    {
        public const string PageId = "0A2645B0-A26E-4566-A40E-B67B189DB6AE";
        private TeamFoundationIdentity _identity;

        private ObservableCollection<Shelveset> _shelvesets = new ObservableCollection<Shelveset>();

        public ObservableCollection<Shelveset> Shelvesets
        {
            get { return _shelvesets; }
            set { _shelvesets = value; RaisePropertyChanged("Shelvesets"); }
        }

        public ICommand ShelvesetDetails { get; private set; }

        public ShelvesetsPage()
        {
            this.Title = "View Shelvesets";
            this.PageContent = new ShelvesetsPageView();
            this.View.ParentSection = this;

            ShelvesetDetails = new RelayCommand(p => ShelvesetDetailsCommand(p as Shelveset));
        }

        protected ShelvesetsPageView View
        {
            get { return this.PageContent as ShelvesetsPageView; }
        }

        public override void Initialize(object sender, PageInitializeEventArgs e)
        {
            base.Initialize(sender, e);

            TeamFoundationIdentity id = (TeamFoundationIdentity)e.Context;
            if (id != null)
            {
                _identity = id;
                this.Refresh();
            }
        }

        private void ShelvesetDetailsCommand(Shelveset shelveset)
        {
            ITeamExplorer teamExplorer = this.GetService<ITeamExplorer>();
            if (teamExplorer != null && shelveset != null)
            {
                teamExplorer.NavigateToPage(new Guid(TeamExplorerPageIds.ShelvesetDetails), shelveset);
            }
        }

        public override async void Refresh()
        {
            base.Refresh();
            await this.RefreshAsync();
        }

        private async Task RefreshAsync()
        {
            try
            {
                // Set our busy flag and clear the previous data
                this.IsBusy = true;
                this.Shelvesets.Clear();

                ObservableCollection<Shelveset> shelvesetsList = new ObservableCollection<Shelveset>();

                // Make the server call asynchronously to avoid blocking the UI
                await Task.Run(() =>
                {
                    ITeamFoundationContext context = this.CurrentContext;
                    if (context != null && context.HasCollection && context.HasTeamProject)
                    {
                        VersionControlServer vcs = context.TeamProjectCollection.GetService<VersionControlServer>();
                        if (vcs != null)
                        {
                            IEnumerable<Shelveset> shelvesets =
                                vcs.QueryShelvesets(null, _identity.UniqueName)
                                .OrderByDescending(s => s.CreationDate);

                            foreach (Shelveset s in shelvesets)
                            {
                                shelvesetsList.Add(s);
                            }
                        }
                    }
                });

                this.Shelvesets = shelvesetsList;
            }
            catch (Exception ex)
            {
                this.ShowNotification(ex.Message, NotificationType.Error);
            }
            finally
            {
                // Always clear our busy flag when done
                this.IsBusy = false;
            }
        }

        public override void SaveContext(object sender, PageSaveContextEventArgs e)
        {
            e.Context = _identity;
        }
    }
}
