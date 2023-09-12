// <copyright file="CustomDataGrid.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF.Classes
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Custom version of the <see cref="Grid"/> that features the ability to control its <see cref="Visibility"/> via data binding.
    /// </summary>
    public class CustomGrid : Grid
    {
        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public static readonly DependencyProperty IsShowingProperty = DependencyProperty.RegisterAttached(
            "IsShowing",
            typeof(bool),
            typeof(CustomGrid),
            new (default(bool), OnIsShowingChanged));

        /// <summary>
        /// Gets or sets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool IsShowing
        {
            get => (bool)this.GetValue(IsShowingProperty);
            set => this.SetValue(IsShowingProperty, value);
        }

        private static void OnIsShowingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomGrid customGrid)
            {
                customGrid.Visibility = customGrid.IsShowing ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
