using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MediaPlayer.Classes
{
    /// <summary>
    /// Heirarchical class structure for populating a <see cref="TreeView"/>
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Name of the navigation option - what is seen on trNav
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Which <see cref="Page"/> to be opened by this <see cref="MenuItem">MenuItem</see> 
        /// </summary>
        public Type PageType { get; set; }

        /// <summary>
        /// <see cref="ObservableCollection{T}"/> of this <see cref="MenuItem">MenuItem's</see> children
        /// </summary>
        public ObservableCollection<MenuItem> Children { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class with an empty <see cref="ObservableCollection{T}"/> of children
        /// </summary>
        /// <param name="title">The <see cref="Title">Title</see> of this instance of <see cref="MenuItem"/></param>
        /// <param name="pageType">The <see cref="PageType">Title</see> of this instance of <see cref="MenuItem"/></param>
        public MenuItem(string title, Type pageType)
        {
            Title = title;
            PageType = pageType;
            this.Children = new ObservableCollection<MenuItem>();
        }
    }
}
