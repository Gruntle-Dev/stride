// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;
using Stride.Core.Presentation.Controls;
using Stride.Core.Presentation.Core;
using Stride.Editor.Annotations;
using Stride.Editor.Preview.View;

namespace Stride.Assets.Presentation.Preview.Views
{
    public class ScaleFromSliderBehavior : Behavior<ScaleBar>
    {
        public static readonly DependencyProperty SliderProperty = DependencyProperty.Register("Slider", typeof(Slider), typeof(ScaleFromSliderBehavior), new PropertyMetadata(null, SliderChanged));
        private static DependencyPropertyWatcher watcher;

        public Slider Slider { get { return (Slider)GetValue(SliderProperty); } set { SetValue(SliderProperty, value); } }

        private static void SliderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (ScaleFromSliderBehavior)d;
            var slider = (Slider)e.NewValue;
            slider.LayoutUpdated += behavior.SliderLayoutUpdated;
            watcher = new DependencyPropertyWatcher(slider);
            watcher.RegisterValueChangedHandler(RangeBase.MinimumProperty, behavior.SliderLayoutUpdated);
            watcher.RegisterValueChangedHandler(RangeBase.MaximumProperty, behavior.SliderLayoutUpdated);
        }

        private void SliderLayoutUpdated(object sender, EventArgs e)
        {
            if (double.IsNaN(Slider.Minimum) || double.IsNaN(Slider.Maximum) || Slider.Minimum >= Slider.Maximum)
                return;

            var range = Slider.Maximum - Slider.Minimum;
            var tickCount = range / AssociatedObject.UnitsPerTick;
            var width = Slider.ActualWidth;
            AssociatedObject.StartUnit = Slider.Minimum;
            AssociatedObject.PixelsPerTick = width / tickCount;
        }
    }

    [AssetPreviewView<AnimationPreview>]
    public class AnimationPreviewView : StridePreviewView
    {
        static AnimationPreviewView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationPreviewView), new FrameworkPropertyMetadata(typeof(AnimationPreviewView)));
        }
    }
}
