// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using Microsoft.Graph;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PeoplePicker Control is a simple control that allows for selection of one or more users from an organizational AD.
    /// </summary>
    [TemplatePart(Name = SearchBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = SearchResultListBoxPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = SelectionsListBoxPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = SearchResultPopupName, Type = typeof(Popup))]
    public partial class PeoplePicker : Control
    {
        private const string SearchBoxPartName = "SearchBox";
        private const string SearchResultListBoxPartName = "SearchResultListBox";
        private const string SearchResultPopupName = "SearchResultPopup";
        private const string SelectionsListBoxPartName = "SelectionsListBox";

        private TextBox _searchBox;
        private ListBox _searchResultListBox;
        private ListBox _selectionsListBox;
        private Popup _searchResultPopup;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeoplePicker"/> class.
        /// </summary>
        public PeoplePicker()
        {
            DefaultStyleKey = typeof(PeoplePicker);
        }

        /// <summary>
        /// Called when applying the control template.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            IsLoading = false;
            if (_searchBox != null)
            {
                _searchBox.TextChanged -= SearchBox_OnTextChanged;
                _searchBox.SizeChanged -= SearchBox_OnSizeChanged;
                _searchBox.KeyUp -= SearchBox_OnKeyUp;
            }

            if (_searchResultListBox != null)
            {
                _searchResultListBox.SelectionChanged -= SearchResultListBox_OnSelectionChanged;
            }

            if (_selectionsListBox != null)
            {
                _selectionsListBox.Tapped -= SelectionsListBox_Tapped;
            }

            _searchBox = GetTemplateChild(SearchBoxPartName) as TextBox;
            _searchResultListBox = GetTemplateChild(SearchResultListBoxPartName) as ListBox;
            _selectionsListBox = GetTemplateChild(SelectionsListBoxPartName) as ListBox;
            _searchResultPopup = GetTemplateChild(SearchResultPopupName) as Popup;

            SearchResultList = new ObservableCollection<Person>();
            Selections = Selections ?? new ObservableCollection<Person>();
            if (_searchBox != null)
            {
                _searchBox.TextChanged += SearchBox_OnTextChanged;
                _searchBox.SizeChanged += SearchBox_OnSizeChanged;
                _searchBox.KeyUp += SearchBox_OnKeyUp;
            }

            if (_searchResultListBox != null)
            {
                _searchResultListBox.SelectionChanged += SearchResultListBox_OnSelectionChanged;
            }

            if (_selectionsListBox != null)
            {
                _selectionsListBox.Tapped += SelectionsListBox_Tapped;
            }

            base.OnApplyTemplate();
        }

        private void SearchBox_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                InputPane inputPane = InputPane.GetForCurrentView();
                if (inputPane != null)
                {
                    inputPane.TryHide();
                    _searchBox.RemoveFocusEngagement();
                }
            }
        }
    }
}