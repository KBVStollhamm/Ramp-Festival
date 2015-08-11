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
	public class SequenceItemDataTemplateSelector : DataTemplateSelector
	{
		public DataTemplate SinglePlayerGameTemplate { get; set; }
		public DataTemplate TeamGameTemplate { get; set; }
		public DataTemplate ChildGameTemplate { get; set; }
		public DataTemplate WomenGameTemplate { get; set; }

		public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
		{
			var si = item as SequencingItem;

			if (si != null)
			{
				switch (si.GameType)
				{
					case GameType.SinglePlayerGame:
						return this.SinglePlayerGameTemplate;
					case GameType.TeamGame:
						return this.TeamGameTemplate;
					case GameType.ChildGame:
						return this.ChildGameTemplate;
					case GameType.WomenGame:
						return this.WomenGameTemplate;
				}
			}

			return base.SelectTemplate(item, container);
		}
	}
}
