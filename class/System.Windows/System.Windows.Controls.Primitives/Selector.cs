//
// Selector.cs
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2008 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Collections.Specialized;

namespace System.Windows.Controls.Primitives {
	public abstract class Selector : ItemsControl {
		internal const string TemplateScrollViewerName = "ScrollViewer";

		public static readonly DependencyProperty SelectedIndexProperty =
			DependencyProperty.RegisterCore ("SelectedIndex", typeof(int), typeof(Selector),
						     new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedIndexChanged)));

		static void OnSelectedIndexChanged (DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((Selector) o).SelectedIndexChanged (o, e);
		}

		// This is not a core property because it is a non-parenting property
		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register ("SelectedItem", typeof(object), typeof(Selector),
						     new PropertyMetadata(new PropertyChangedCallback(OnSelectedItemChanged_cb)));

		
		static void OnSelectedItemChanged_cb (DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((Selector) o).SelectedItemChanged (o, e);
		}

		internal Selector ()
		{
			// Set default values for ScrollViewer attached properties 
			ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Auto);
			ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
		}

		bool Changing {
			get; set;
		}

		internal bool IsSelectionActive {
			get; set;
		}

		public int SelectedIndex {
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue (SelectedIndexProperty, value); }
		}

		public object SelectedItem {
			get { return GetValue (SelectedItemProperty); }
			set { SetValue (SelectedItemProperty, value); }
		}

		internal ScrollViewer TemplateScrollViewer {
			get; private set;
		}

		
		public event SelectionChangedEventHandler SelectionChanged;

		void SelectedIndexChanged (DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			object oldItem = SelectedItem;
			int newVal = (int) e.NewValue;
			if (newVal == (int) e.OldValue || Changing) {
				SelectedIndex = newVal;
				return;
			}

			Changing = true;
			try {
				if (newVal < 0)
					ClearValue (SelectedItemProperty);
				else if (newVal < Items.Count)
					SelectedItem = Items [newVal];
			} finally {
				Changing = false;
			}
			RaiseSelectionChanged (o, new SelectionChangedEventArgs (oldItem, SelectedItem));
		}
		
		void SelectedItemChanged (DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == e.OldValue || Changing) {
				SelectedItem = e.NewValue;
				return;
			}
			
			Changing = true;
			try {
				int index = e.NewValue == null ? -1 : Items.IndexOf (e.NewValue);
				if (index == -1 && e.NewValue != null) {
					SelectedIndex = e.OldValue == null ? -1 : Items.IndexOf (e.OldValue);
					if (e.OldValue == null)
						ClearValue (SelectedItemProperty);
					else
						SelectedItem = e.OldValue;
				}
				else {
					SelectedItem = e.NewValue;
					SelectedIndex = index;
					RaiseSelectionChanged (o, new SelectionChangedEventArgs (e.OldValue, e.NewValue));
				}
			} finally {
				Changing = false;
			}
		}
		
		void OnSelectedItemChanged (object oldValue, object newValue)
		{
			if (oldValue != null) {
				ListBoxItem oldItem = GetContainerItem (Items.IndexOf (oldValue));
				if (oldItem != null)
					oldItem.IsSelected = false;
			}

			if (newValue != null) {
				ListBoxItem newItem = GetContainerItem (Items.IndexOf (newValue));
				if (newItem != null) {
					newItem.IsSelected = true;
					newItem.Focus ();
				}
			}
		}

		void RaiseSelectionChanged (object o, SelectionChangedEventArgs e)
		{
			object oldVal = e.RemovedItems.Count == 1 ? e.RemovedItems [0] : null;
			object newVal = e.AddedItems.Count == 1 ? e.AddedItems [0] : null;
			OnSelectedItemChanged (oldVal, newVal);
			
			SelectionChangedEventHandler h = SelectionChanged;
			if (h != null)
				h (o, e);
		}

		public static bool GetIsSelectionActive (DependencyObject element)
		{
			if (element == null)
				throw new ArgumentNullException ("element");

			Selector s = (element as Selector);
			return s == null ? false : s.IsSelectionActive;
		}

		protected override void ClearContainerForItemOverride (DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride (element, item);
			if (element == null)
				throw new NullReferenceException ();
			
			ListBoxItem lbItem = (ListBoxItem) element;
			lbItem.Content = null;
			lbItem.IsSelected = false;
			if (SelectedItem == item)
				SelectedItem = null;
		}

		public override void OnApplyTemplate ()
		{
			base.OnApplyTemplate ();
			TemplateScrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;
			
			if (TemplateScrollViewer != null)
			{
				TemplateScrollViewer.TemplatedParentHandlesScrolling = true;
				// Update ScrollViewer values
				TemplateScrollViewer.HorizontalScrollBarVisibility = ScrollViewer.GetHorizontalScrollBarVisibility(this); 
				TemplateScrollViewer.VerticalScrollBarVisibility = ScrollViewer.GetVerticalScrollBarVisibility(this); 
			}
		}

		protected override void OnItemsChanged (System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action) {
			case NotifyCollectionChangedAction.Add:
				// Ensure we don't fire a SelectionChanged event when we're just updating the index
				Changing = true;
				if (e.NewStartingIndex <= SelectedIndex)
					SelectedIndex ++;
				Changing = false;
				break;
			case NotifyCollectionChangedAction.Reset:
				SelectedIndex = -1;
				break;
				
			case NotifyCollectionChangedAction.Remove:
			case NotifyCollectionChangedAction.Replace:
			default:
				// Yes this is broken, SelectedItem and SelectedIndex do get out of sync with reality.
				break;
			}
			base.OnItemsChanged (e);
		}
		
		internal virtual void NotifyListItemClicked(ListBoxItem listBoxItem) 
		{
			
		}
		
		internal virtual void NotifyListItemLoaded (ListBoxItem listBoxItem)
		{
			
		}
		
		internal virtual void NotifyListItemGotFocus(ListBoxItem listBoxItemNewFocus)
		{
			
		}
		
		internal virtual void NotifyListItemLostFocus(ListBoxItem listBoxItemOldFocus)
		{
			
		}
	}
}
