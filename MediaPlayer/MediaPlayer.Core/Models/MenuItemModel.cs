using MvvmCross.Commands;
using System.Collections.ObjectModel;

namespace MediaPlayer.Core.Models
{
    /// <summary>
    /// Heirarchical class structure for populating a <see cref="TreeView"/>
    /// </summary>
    public class MenuItemModel
    {
        /// <summary>
        /// Name of the navigation option - what is seen on trNav
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Which <see cref="Page"/> to be opened by this <see cref="MenuItem">MenuItem</see> 
        /// </summary>
        public IMvxCommand NavCommand { get; set; }

        /// <summary>
        /// <see cref="ObservableCollection{T}"/> of this <see cref="MenuItem">MenuItem's</see> children
        /// </summary>
        public ObservableCollection<MenuItemModel> Children { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class with an empty <see cref="ObservableCollection{T}"/> of children
        /// </summary>
        /// <param name="title">The <see cref="Title">Title</see> of this instance of <see cref="MenuItem"/></param>
        /// <param name="pageType">The <see cref="PageType">Title</see> of this instance of <see cref="MenuItem"/></param>
        public MenuItemModel(string title, IMvxCommand navCommand)
        {
            Title = title;
            NavCommand = navCommand;
            this.Children = new ObservableCollection<MenuItemModel>();
        }
    }
}
