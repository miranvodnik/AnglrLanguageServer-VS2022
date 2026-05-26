using AnglrJsonRpcMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace AnglrLangExtension
{
    /// <summary>
    /// Interaction logic for AnglrStateTab.xaml
    /// </summary>
    public partial class AnglrStateTab : UserControl, INotifyPropertyChanged
    {
        public AnglrStateItem stateItem { get; set; }

        public ObservableCollection<AnglrGetParserStateCoreData> stateCoreDatas { get; set; }
        public ObservableCollection<AnglrGetParserStateClosureData> stateClosureDatas { get; set; }
        public ObservableCollection<AnglrGetParserStateTransitionData> shiftTransitionDatas { get; set; }
        public ObservableCollection<AnglrGetParserStateTransitionData> gotoTransitionDatas { get; set; }
        public ObservableCollection<AnglrGetParserStateReductionsData> reductionDatas { get; set; }
        public ObservableCollection<KeyValuePair<string, SRConflictList>> SRConflicts { get; set; }
        public ObservableCollection<KeyValuePair<string, RRConflictList>> RRConflicts { get; set; }

        private bool _isCoreDataVisible;
        private bool _isClosureDataVisible;
        private bool _isShiftTransitionDataVisible;
        private bool _isGotoTransitionDataVisible;
        private bool _isReductionDataVisible;
        private bool _isSRConflictsVisible;
        private bool _isRRConflictsVisible;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCoreDataVisible
        {
            get => _isCoreDataVisible;
            set { _isCoreDataVisible = value; OnPropertyChanged (nameof (IsCoreDataVisible)); }
        }
        public bool IsClosureDataVisible
        {
            get => _isClosureDataVisible;
            set { _isClosureDataVisible = value; OnPropertyChanged (nameof (IsClosureDataVisible)); }
        }
        public bool IsShiftTransitionDataVisible
        {
            get => _isShiftTransitionDataVisible;
            set { _isShiftTransitionDataVisible = value; OnPropertyChanged (nameof (IsShiftTransitionDataVisible)); }
        }
        public bool IsGotoTransitionDataVisible
        {
            get => _isGotoTransitionDataVisible;
            set { _isGotoTransitionDataVisible = value; OnPropertyChanged (nameof (IsGotoTransitionDataVisible)); }
        }
        public bool IsReductionDataVisible
        {
            get => _isReductionDataVisible;
            set { _isReductionDataVisible = value; OnPropertyChanged (nameof (IsReductionDataVisible)); }
        }
        public bool IsSRConflictsVisible
        {
            get => _isSRConflictsVisible;
            set { _isSRConflictsVisible = value; OnPropertyChanged (nameof (IsSRConflictsVisible)); }
        }
        public bool IsRRConflictsVisible
        {
            get => _isRRConflictsVisible;
            set { _isRRConflictsVisible = value; OnPropertyChanged (nameof (IsRRConflictsVisible)); }
        }
        public AnglrStateTab (AnglrStateItem stateItem)
        {
            InitializeComponent ();

            this.stateItem = stateItem;

            DataContext = this;

            stateCoreDatas = new ObservableCollection<AnglrGetParserStateCoreData> (stateItem.getParserStateItemResult.CoreSet);
            stateClosureDatas = new ObservableCollection<AnglrGetParserStateClosureData> (stateItem.getParserStateItemResult.ClosureSet);
            shiftTransitionDatas = new ObservableCollection<AnglrGetParserStateTransitionData> (stateItem.getParserStateItemResult.ShiftSet);
            gotoTransitionDatas = new ObservableCollection<AnglrGetParserStateTransitionData> (stateItem.getParserStateItemResult.GotoSet);
            reductionDatas = new ObservableCollection<AnglrGetParserStateReductionsData> (stateItem.getParserStateItemResult.ReductionsSet);
            SRConflicts = new ObservableCollection<KeyValuePair<string, SRConflictList>> (stateItem.SRConflicts);
            RRConflicts = new ObservableCollection<KeyValuePair<string, RRConflictList>> (stateItem.RRConflicts);

            IsCoreDataVisible = stateCoreDatas.Count > 0;
            IsClosureDataVisible = stateClosureDatas.Count > 0;
            IsShiftTransitionDataVisible = shiftTransitionDatas.Count > 0;
            IsGotoTransitionDataVisible = gotoTransitionDatas.Count > 0;
            IsReductionDataVisible = reductionDatas.Count > 0;
            IsSRConflictsVisible = SRConflicts.Count > 0;
            IsRRConflictsVisible = RRConflicts.Count > 0;
        }

        protected void OnPropertyChanged (string propertyName)
        {
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}
