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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TestApp
{
	/// <summary>
	/// 
	/// </summary>
	public partial class PlayerControl : UserControl
	{
		Storyboard _hideBorder;
		Storyboard _showBorder;

		public PlayerControl()
		{
			InitializeComponent();

			PlayerViewModel viewModel = this.DataContext as PlayerViewModel;
			viewModel.PlayerName = "Player 1";

			viewModel.PropertyChanging += ViewModel_PropertyChanging;

			_hideBorder = Resources["HideBorder"] as Storyboard;
			_hideBorder.Completed += HideBorder_Completed;

			_showBorder = Resources["ShowBorder"] as Storyboard;
		}

		private void ViewModel_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
		{
			Dispatcher.BeginInvoke((Action)(() => BeginStoryboard(_hideBorder)), DispatcherPriority.Send);
			this.Wait(TimeSpan.FromSeconds(0.5));
		}

		private void HideBorder_Completed(object sender, EventArgs e)
		{
			BeginStoryboard(_showBorder);
		}
	}

	public static class DispatcherObjectExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dispatcherObject"></param>
		/// <param name="timeSpan"></param>
		public static void Wait(this DispatcherObject dispatcherObject, TimeSpan timeSpan)
		{
			Wait(dispatcherObject, timeSpan.Ticks);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dispatcherObject"></param>
		/// <param name="ticks"></param>
		public static void Wait(this DispatcherObject dispatcherObject, long ticks)
		{
			long dtEnd = DateTime.Now.AddTicks(ticks).Ticks;

			while (DateTime.Now.Ticks < dtEnd)
			{
				dispatcherObject.Dispatcher.Invoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate(object unused) { return null; }, null);
			}
		}
	}
}
