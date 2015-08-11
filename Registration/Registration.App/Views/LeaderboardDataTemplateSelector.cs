using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Registration.ReadModel;

namespace Registration.Views
{
	public class LeaderboardDataTemplateSelector : DataTemplateSelector
	{
		public DataTemplate SinglePlayerTemplate { get; set; }
		public DataTemplate TeamTemplate { get; set; }
		public DataTemplate ChildrenTemplate { get; set; }
		public DataTemplate WomenTemplate { get; set; }

		public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
		{
			var boardItem = item as LeaderboardItem;

			if (boardItem != null)
			{
				switch (boardItem.GameType)
				{
					case GameType.SinglePlayerGame:
						return this.SinglePlayerTemplate;
					case GameType.TeamGame:
						return this.TeamTemplate;
					case GameType.ChildGame:
						return this.ChildrenTemplate;
					case GameType.WomenGame:
						return this.WomenTemplate;
				}
			}

			return base.SelectTemplate(item, container);
		}
	}
}
