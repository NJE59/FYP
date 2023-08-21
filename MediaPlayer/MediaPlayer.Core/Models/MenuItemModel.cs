// <copyright file="MenuItemModel.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.Core.Models
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Heirarchical class structure for populating a <see cref="TreeView"/>.
    /// </summary>
    public class MenuItemModel
    {
        private string title = null!;
        private Type pageType = null!;
        private ObservableCollection<MenuItemModel> children = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItemModel"/> class with an empty <see cref="ObservableCollection{T}"/> of children.
        /// </summary>
        /// <param name="title"><inheritdoc cref="Title" path='/summary'/></param>
        /// <param name="pageType"><inheritdoc cref="PageType" path='/summary'/></param>
        public MenuItemModel(string title, Type pageType)
        {
            this.Title = title;
            this.PageType = pageType;
            this.Children = new ObservableCollection<MenuItemModel>();
        }

        /// <summary>
        /// Gets or sets the name of the navigation option - what is seen on trNav.
        /// </summary>
        public string Title
        {
            get => this.title;
            set => this.title = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="Page"/> to be opened by this <see cref="MenuItem">MenuItem</see>.
        /// </summary>
        public Type PageType
        {
            get => this.pageType;
            set => this.pageType = value;
        }

        /// <summary>
        /// Gets or sets an <see cref="ObservableCollection{MenuItemModel}"/> of the children of this <see cref="MenuItemModel"/>.
        /// </summary>
        public ObservableCollection<MenuItemModel> Children
        {
            get => this.children;
            set => this.children = value;
        }
    }
}
