﻿using ReduxSimple.Uwp.Samples.Counter;
using ReduxSimple.Uwp.Samples.Pokedex;
using ReduxSimple.Uwp.Samples.TicTacToe;
using ReduxSimple.Uwp.Samples.TodoList;
using System;
using System.Reactive.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using static Microsoft.Toolkit.Uwp.UI.Extensions.ApplicationViewExtensions;
using static Windows.UI.Core.AppViewBackButtonVisibility;
using static ReduxSimple.Uwp.Samples.Common.EventTracking;
using Windows.System;

namespace ReduxSimple.Uwp.Samples
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            GoToCounterButton.Events().Click
                .Subscribe(_ =>
                {
                    TrackNavigation(nameof(MainPage), nameof(CounterPage));
                    Frame.Navigate(typeof(CounterPage));
                });

            GoToTicTacToeButton.Events().Click
                .Subscribe(_ =>
                {
                    TrackNavigation(nameof(MainPage), nameof(TicTacToePage));
                    Frame.Navigate(typeof(TicTacToePage));
                });

            GoToTodoListButton.Events().Click
                .Subscribe(_ =>
                {
                    TrackNavigation(nameof(MainPage), nameof(TodoListPage));
                    Frame.Navigate(typeof(TodoListPage));
                });

            GoToPokedexButton.Events().Click
                .Subscribe(_ =>
                {
                    TrackNavigation(nameof(MainPage), nameof(PokedexPage));
                    Frame.Navigate(typeof(PokedexPage));
                });

            GoToGitHubButton.Events().Click
                .Subscribe(async _ =>
                {
                    var uri = new Uri("https://github.com/Odonno/ReduxSimple");
                    await Launcher.LaunchUriAsync(uri);
                });

            // Extend view into title bar
            SetExtendViewIntoTitleBar(this, true);

            // Set TitleBar properties (colors)
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 72, 42, 203);
            titleBar.ButtonPressedBackgroundColor = Color.FromArgb(200, 72, 42, 203);
            titleBar.ButtonForegroundColor = Colors.Black;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Subscribe only once the MainPage is shown at the start of the app
            if (e.NavigationMode == NavigationMode.New)
            {
                // Go back when back button clicked
                var systemNavigationManager = Windows.UI.Core.SystemNavigationManager.GetForCurrentView();

                Observable.FromEventPattern<EventHandler<Windows.UI.Core.BackRequestedEventArgs>, Windows.UI.Core.BackRequestedEventArgs>(
                    h => systemNavigationManager.BackRequested += h,
                    h => systemNavigationManager.BackRequested -= h
                )
                    .Where(_ => Frame.CanGoBack)
                    .Subscribe(_ =>
                    {
                        TrackNavigation(Frame.Content.GetType().Name, nameof(MainPage));
                        Frame.GoBack();
                    });

                // Show back button when required
                Frame.Events().Navigated
                   .Subscribe(_ =>
                   {
                       SetBackButtonVisibility(this, Frame.CanGoBack ? Visible : Collapsed);
                   });
            }
        }
    }
}
