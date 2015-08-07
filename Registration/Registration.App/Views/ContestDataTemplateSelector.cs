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
	public class ContestDataTemplateSelector : DataTemplateSelector
	{
		public DataTemplate SinglePlayerContestTemplate { get; set; }
		public DataTemplate TeamContestTemplate { get; set; }
        public DataTemplate ChildrenContestTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
		{
			var contest = item as Contest;

			if (contest != null)
			{
                switch (contest.ContestType)
                {
                    case ContestType.SinglePlayerContest:
                        return this.SinglePlayerContestTemplate;
                    case ContestType.TeamContest:
                        return this.TeamContestTemplate;
                    case ContestType.ChildrenContest:
                        return this.ChildrenContestTemplate;
                }
			}

			return base.SelectTemplate(item, container);
		}
	}
}
