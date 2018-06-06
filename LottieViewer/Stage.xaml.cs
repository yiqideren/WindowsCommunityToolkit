// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Lottie;
using System;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LottieViewer
{
    public sealed partial class Stage : UserControl
    {

        public static DependencyProperty ArtboardColorProperty =
            DependencyProperty.Register(nameof(ArtboardColor), typeof(Color), typeof(Stage), new PropertyMetadata(Colors.White));

        public static DependencyProperty PlayerProperty =
                DependencyProperty.Register(nameof(Player), typeof(CompositionPlayer), typeof(Stage), new PropertyMetadata(null));

        public Stage()
        {
            this.InitializeComponent();
            SetValue(PlayerProperty, _player);

            _player.RegisterPropertyChangedCallback(CompositionPlayer.IsCompositionLoadedProperty, UpdateFileInfo);

            Reset();
        }

        void UpdateFileInfo(DependencyObject obj, DependencyProperty property)
        {
            var diagnostics = _player.Diagnostics;
            if (diagnostics == null)
            {
                _txtFileName.Text = "";
                _txtDuration.Text = "";
                _txtSize.Text = "";
            }
            else
            {
                var diags = (LottieCompositionDiagnostics)diagnostics;
                _txtFileName.Text = diags.FileName;
                _txtDuration.Text = $"{diags.Duration.TotalSeconds} secs";
                var aspectRatio = FloatToRatio(diags.LottieWidth / diags.LottieHeight);
                _txtSize.Text = $"{diags.LottieWidth}x{diags.LottieHeight} ({aspectRatio.Item1.ToString("0.##")}:{aspectRatio.Item2.ToString("0.##")})";
            }
        }

        internal CompositionPlayer Player => _player;

        internal LottieCompositionSource Source => _playerSource;

        public Color ArtboardColor
        {
            get => (Color)GetValue(ArtboardColorProperty);
            set => SetValue(ArtboardColorProperty, value);
        }

        internal async void PlayFile(StorageFile file)
        {
            var startDroppedAnimation = _feedbackLottie.PlayDroppedAnimation();

            _player.Opacity = 0;
            try
            {
                // Load the Lottie composition.
                await Source.SetSourceAsync(file);
            }
            catch (Exception)
            {
                // Failed to load.
                _player.Opacity = 1;
                await _feedbackLottie.PlayLoadFailedAnimation();
                return;
            }

            // Wait until the dropping animation has finished.
            await startDroppedAnimation;

            _player.Opacity = 1;
            Player.Play();

        }


        internal void DoDragEnter()
        {
            _feedbackLottie.PlayDragEnterAnimation();
        }

        internal void DoDragDropped(StorageFile file)
        {
            PlayFile(file);
        }

        internal void DoDragLeave()
        {
            _feedbackLottie.PlayDragLeaveAnimation();
        }

        internal void Reset()
        {
            _feedbackLottie.PlayInitialStateAnimation();
        }


        // Returns a pleasantly simplified ratio for the given value.
        static (double, double) FloatToRatio(double value)
        {
            const int maxRatioProduct = 200;
            var candidateN = 1.0;
            var candidateD = Math.Round(1 / value);
            var error = Math.Abs(value - (candidateN / candidateD));

            for (double n = candidateN, d = candidateD; n * d <= maxRatioProduct && error != 0;)
            {
                if (value > n / d)
                {
                    n++;
                }
                else
                {
                    d++;
                }

                var newError = Math.Abs(value - (n / d));
                if (newError < error)
                {
                    error = newError;
                    candidateN = n;
                    candidateD = d;
                }
            }

            // If we gave up because the numerator or denominator got too big then
            // the number is an approximation that requires some decimal places.
            // Get the real ratio by adjusting the denominator or numerator - whichever
            // requires the smallest adjustment.
            if (error != 0)
            {
                if (value > candidateN / candidateD)
                {
                    candidateN = candidateD * value;
                }
                else
                {
                    candidateD = candidateN / value;
                }
            }
            return (candidateN, candidateD);
        }

    }
}
