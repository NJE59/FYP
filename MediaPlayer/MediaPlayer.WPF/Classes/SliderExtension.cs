// <copyright file="SliderExtension.cs" company="Nathan Errington">
// Copyright (c) Nathan Errington. All rights reserved.
// </copyright>

namespace MediaPlayer.WPF.Classes
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using MvvmCross.Commands;

    /// <summary>
    /// Extension for the WPF <see cref="Slider"/> control to add events for when the user starts and stops dragging the thumb.
    /// </summary>
    public class SliderExtension
    {
        /// <summary>
        /// Dependency property for the Drag Started event command.
        /// </summary>
        public static readonly DependencyProperty DragStartedCommandProperty = DependencyProperty.RegisterAttached(
            "DragStartedCommand",
            typeof(IMvxCommand),
            typeof(SliderExtension),
            new PropertyMetadata(default(IMvxCommand), OnDragStartedCommandChanged));

        /// <summary>
        /// Dependency property for the Drag Completed event command.
        /// </summary>
        public static readonly DependencyProperty DragCompletedCommandProperty = DependencyProperty.RegisterAttached(
            "DragCompletedCommand",
            typeof(IMvxCommand),
            typeof(SliderExtension),
            new PropertyMetadata(default(IMvxCommand), OnDragCompletedCommandChanged));

        /// <summary>
        /// Gets the Drag Started event command.
        /// </summary>
        /// <param name="element"><inheritdoc cref="DependencyObject" path='/summary'/></param>
        /// <returns><inheritdoc cref="IMvxCommand" path='/summary'/></returns>
        public static IMvxCommand GetDragStartedCommand(DependencyObject element)
        {
            return (IMvxCommand)element.GetValue(DragStartedCommandProperty);
        }

        /// <summary>
        /// Sets the Drag Started event command.
        /// </summary>
        /// <param name="element"><inheritdoc cref="DependencyObject" path='/summary'/></param>
        /// <param name="value"><inheritdoc cref="IMvxCommand" path='/summary'/></param>
        public static void SetDragStartedCommand(DependencyObject element, IMvxCommand value)
        {
            element.SetValue(DragStartedCommandProperty, value);
        }

        /// <summary>
        /// Gets the Drag Completed event command.
        /// </summary>
        /// <param name="element"><inheritdoc cref="DependencyObject" path='/summary'/></param>
        /// <returns><inheritdoc cref="IMvxCommand" path='/summary'/></returns>
        public static IMvxCommand GetDragCompletedCommand(DependencyObject element)
        {
            return (IMvxCommand)element.GetValue(DragCompletedCommandProperty);
        }

        /// <summary>
        /// Sets the Drag Completed event command.
        /// </summary>
        /// <param name="element"><inheritdoc cref="DependencyObject" path='/summary'/></param>
        /// <param name="value"><inheritdoc cref="IMvxCommand" path='/summary'/></param>
        public static void SetDragCompletedCommand(DependencyObject element, IMvxCommand value)
        {
            element.SetValue(DragCompletedCommandProperty, value);
        }

        private static void OnDragStartedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Slider slider)
            {
                return;
            }

            if (e.NewValue is IMvxCommand)
            {
                slider.Loaded += SliderOnLoaded;
            }
        }

        private static void OnDragCompletedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Slider slider)
            {
                return;
            }

            if (e.NewValue is IMvxCommand)
            {
                slider.Loaded += SliderOnLoaded;
            }
        }

        private static void SliderOnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not Slider slider)
            {
                return;
            }

            slider.Loaded -= SliderOnLoaded;

            if (slider.Template.FindName("PART_Track", slider) is not Track track)
            {
                return;
            }

            track.Thumb.DragStarted += (dragStartedSender, dragStartedArgs) =>
            {
                IMvxCommand command = GetDragStartedCommand(slider);
                command.Execute(null);
            };
            track.Thumb.DragCompleted += (dragCompletedSender, dragCompletedArgs) =>
            {
                IMvxCommand command = GetDragCompletedCommand(slider);
                command.Execute(null);
            };
        }
    }
}
