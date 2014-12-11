using Microsoft.TeamFoundation.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamContributors.Base
{
    public class TeamExplorerBaseNavigationItem : TeamExplorerBase, ITeamExplorerNavigationItem
    {
        private bool isVisible = true;
        private string text;
        private System.Drawing.Image image;

        public TeamExplorerBaseNavigationItem(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
                this.RaisePropertyChanged("Text");
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this.image;
            }

            set
            {
                this.image = value;
                this.RaisePropertyChanged("Image");
            }
        }

        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }

            set
            {
                this.isVisible = value;
                this.RaisePropertyChanged("IsVisible");
            }
        }

        public virtual void Invalidate()
        {
        }

        public virtual void Execute()
        {
        }
    }
}
