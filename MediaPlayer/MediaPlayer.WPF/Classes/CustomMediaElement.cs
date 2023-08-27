// <copyright file="CustomMediaElement.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF.Classes
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using MediaPlayer.Core.Interfaces;
    using MvvmCross.Commands;
    using MvvmCross.Platforms.Wpf.Binding;

    /// <summary>
    /// PLACEHOLDER.
    /// </summary>
    public class CustomMediaElement : MediaElement
    {
        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.RegisterAttached(
            "IsPlaying",
            typeof(bool),
            typeof(CustomMediaElement),
            new (default(bool), OnIsPlayingChanged));

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public static readonly DependencyProperty IsPausedProperty = DependencyProperty.RegisterAttached(
            "IsPaused",
            typeof(bool),
            typeof(CustomMediaElement),
            new (default(bool), OnIsPausedChanged));

        /// <summary>
        /// Gets or sets PLACEHOLDER.
        /// </summary>
        public static readonly DependencyProperty IsStoppedProperty = DependencyProperty.RegisterAttached(
            "IsStopped",
            typeof(bool),
            typeof(CustomMediaElement),
            new (default(bool), OnIsStoppedChanged));

        /// <summary>
        /// Gets or sets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool IsPlaying
        {
            get => (bool)this.GetValue(IsPlayingProperty);
            set => this.SetValue(IsPlayingProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool IsPaused
        {
            get => (bool)this.GetValue(IsPausedProperty);
            set => this.SetValue(IsPausedProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether PLACEHOLDER.
        /// </summary>
        public bool IsStopped
        {
            get => (bool)this.GetValue(IsStoppedProperty);
            set => this.SetValue(IsStoppedProperty, value);
        }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public new void Play()
        {
            base.Play();
            this.IsPlaying = true;
        }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public new void Pause()
        {
            base.Pause();
            this.IsPaused = true;
        }

        /// <summary>
        /// PLACEHOLDER.
        /// </summary>
        public new void Stop()
        {
            base.Stop();
            this.IsStopped = true;
        }

        private static void OnIsPlayingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomMediaElement mediaControllerTest && mediaControllerTest.IsPlaying)
            {
                mediaControllerTest.Play();
                mediaControllerTest.IsPaused = false;
                mediaControllerTest.IsStopped = false;
            }
        }

        private static void OnIsPausedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomMediaElement mediaControllerTest && mediaControllerTest.IsPaused)
            {
                mediaControllerTest.Pause();
                mediaControllerTest.IsPlaying = false;
                mediaControllerTest.IsStopped = false;
            }
        }

        private static void OnIsStoppedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomMediaElement mediaControllerTest)
            {
                mediaControllerTest.Stop();
                mediaControllerTest.IsPlaying = false;
                mediaControllerTest.IsPaused = false;
            }
        }
    }
}
