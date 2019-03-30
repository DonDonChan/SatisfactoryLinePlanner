using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryLinePlanner.OverallTab
{
    class OverallTabController
    {
        private OverallTab _overallTab;

        public OverallTab _OverallTab { get { return _overallTab; } }

        public OverallTabController()
        {
            _overallTab = new OverallTab();
        }
    }
}
