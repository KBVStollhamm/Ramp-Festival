using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Registration.Controls
{
	/// <summary>
	/// 
	/// </summary>
	public class XAnimatedButton : Button
	{
		private bool _templateApplied;
		private bool _isAnimating;
		private DispatcherTimer _timer;

		static XAnimatedButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(XAnimatedButton), new FrameworkPropertyMetadata(typeof(XAnimatedButton)));
			ContentProperty.OverrideMetadata(typeof(XAnimatedButton), new FrameworkPropertyMetadata(null, OnContentChanging));
		}

		private object OnContentChangingInternal(object oldValue, object newValue)
		{
			if (!_templateApplied || _isAnimating)
				return newValue;

			this.Dispatcher.BeginInvoke(
				(Action)delegate
				{
					_isAnimating = true;
					this.RaiseContentChangingEvent();

					this.StopTimer();

					this._timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 500) };

					EventHandler handler = null;
					handler = (sender, args) =>
					{
						this.StopTimer();
						Content = newValue;
						_isAnimating = false;
					};
					this._timer.Tick += handler;
					this._timer.Start();
				});

			return oldValue;
		}

		private static object OnContentChanging(DependencyObject d, object baseValue)
		{
			XAnimatedButton button = d as XAnimatedButton;

			return button.OnContentChangingInternal(button.Content, baseValue);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_templateApplied = true;
		}

		public static readonly RoutedEvent ContentChangingEvent = EventManager.RegisterRoutedEvent(
			"ContentChanging", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(XAnimatedButton));


		public event RoutedEventHandler ContentChanging
		{
			add { AddHandler(ContentChangingEvent, value); }
			remove { RemoveHandler(ContentChangingEvent, value); }
		}

		private void RaiseContentChangingEvent()
		{
			RaiseEvent(new RoutedEventArgs(ContentChangingEvent, this));
		}

		private void StopTimer()
		{
			if (_timer != null)
			{
				_timer.Stop();
				_timer = null;
			}
		}
	}
}