using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ViewModels
{
	public class SequencingViewModel
	{
		public SequencingViewModel()
		{
		}

		public ReadOnlyObservableCollection<SequencingItemViewModel> Sequence { get; private set; }
	}
}
