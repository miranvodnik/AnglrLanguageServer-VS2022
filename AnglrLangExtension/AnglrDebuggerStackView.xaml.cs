using AnglrLogLibrary;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public class AnglrDebuggerStackElement
    {
        public int Id { get; set; }
        public bool IsTerminal { get; set; }
        public int State { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public AnglrDebuggerStackElement () { }
        public AnglrDebuggerStackElement (AnglrDebuggerStackElement item)
        {
            Id = item.Id;
            IsTerminal = item.IsTerminal;
            State = item.State;
            Name = item.Name;
            Value = item.Value;
        }
    }

    public class ObservableStack<T> : List<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public ObservableStack () : base () { }

        public ObservableStack (IEnumerable<T> collection) : base (collection) { }

        public ObservableStack (int capacity) : base (capacity) { }

        public void Push (T item)
        {
            Add (item);
            OnCollectionChanged (NotifyCollectionChangedAction.Add, item, Count - 1);
            OnPropertyChanged (nameof (Count));
        }

        public T Pop (int count = 1)
        {
            T item = default;
            while (count-- > 0)
            {
                item = Peek ();
                RemoveAt (Count - 1);
                OnCollectionChanged (NotifyCollectionChangedAction.Remove, item, Count);
                OnPropertyChanged (nameof (Count));
            }
            return item;
        }

        public new T Peek () => this [Count - 1];

        public new void Clear ()
        {
            base.Clear ();
            OnCollectionChanged (NotifyCollectionChangedAction.Reset);
            OnPropertyChanged (nameof (Count));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged (NotifyCollectionChangedAction action, T item = default, int index = 0)
        {
            CollectionChanged?.Invoke (this, (item == null) && (action != NotifyCollectionChangedAction.Reset)
                ? new NotifyCollectionChangedEventArgs (action)
                : new NotifyCollectionChangedEventArgs (action, item, index));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged (string propertyName)
        {
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }
    }

    public class AnglrDebuggerStackInfo : ObservableStack<AnglrDebuggerStackElement>
    {
        public AnglrDebuggerStackInfo (AnglrDebuggerStackInfo stackInfo)
        {
            foreach (var item in stackInfo)
                base.Push (new AnglrDebuggerStackElement (item));
        }
        public AnglrDebuggerStackInfo (AnglrDebuggerStackElement item) => base.Push (item);
    }

    /// <summary>
    /// Interaction logic for AnglrDebuggerStackView.xaml
    /// </summary>
    public partial class AnglrDebuggerStackView : UserControl
    {
        public IAnglrLogger AnglrLogger { get; private set; }
        private IAnglrLangService _anglrLangService = null;
        public IAnglrLangService AnglrLangService
        {
            get => _anglrLangService;
            set
            {
                _anglrLangService = value;
                AnglrLogger = _anglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();
            }
        }
        public AnglrDebuggerStackInfo ParserStack { get; private set; }
        public int StackNr { get; set; }

        public AnglrDebuggerStackView ()
        {
            InitializeComponent ();
            ParserStack = null;
        }

        public void InitParserStack (AnglrDebuggerStackInfo parserStack, int stackNr)
        {
            StackNr = stackNr;
            if (parserStack == null)
            {
                ParserStack = new AnglrDebuggerStackInfo
                (
                    new AnglrDebuggerStackElement ()
                    {
                        Id = 0,
                        IsTerminal = false,
                        State = 0,
                        Name = "",
                        Value = ""
                    }
                );
            }
            else
            {
                ParserStack = new AnglrDebuggerStackInfo (parserStack);
            }
            ParserStack.CollectionChanged += AnglrDebuggerStackView_CollectionChanged;
        }

        public string ReducedValue { get; set; }

        private void AnglrDebuggerStackView_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedAction action = e.Action;
            if (action != NotifyCollectionChangedAction.Remove)
                return;

            if ((e.OldItems == null) || (e.OldItems.Count == 0))
                return;

            foreach (var item in e.OldItems)
            {
                AnglrDebuggerStackElement element = item as AnglrDebuggerStackElement;
                if (element == null)
                    return;

                AnglrLogger?.DebugLine ($"AnglrDebuggerStackView_CollectionChanged, value = {element.Value}");
                string val = element.Value + " " + ReducedValue;
                ReducedValue = val.Trim ();
            }
        }
    }

    public class AnglrDebuggerStackSelector : DataTemplateSelector
    {
        public DataTemplate TerminalTemplate { get; set; }
        public DataTemplate NonTerminalTemplate { get; set; }

        public AnglrDebuggerStackSelector () { }

        public override DataTemplate SelectTemplate (object item, System.Windows.DependencyObject container)
        {
            AnglrDebuggerStackElement anglrDebuggerStackElement = item as AnglrDebuggerStackElement;
            return (anglrDebuggerStackElement == null) ? null : anglrDebuggerStackElement.IsTerminal ? TerminalTemplate : NonTerminalTemplate;
        }
    }

}
