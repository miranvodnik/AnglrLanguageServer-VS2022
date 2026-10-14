using AnglrDebuggerJsonRpcMessages;
using AnglrLogLibrary;
using Microsoft.NET.StringTools;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    /// Interaction logic for AnglrPDASnapshotViewerTab.xaml
    /// </summary>

    public class ColumnWidths : Dictionary<int, double> { }
    public static class AnglrMonoChromaticColors
    {
        public static ImmutableArray<ImmutableArray<string>> ColorValues { get; } = new ImmutableArray<ImmutableArray<string>> ()
        {
            new ImmutableArray<string> () { "#FFD399", "#FEB24D", "#FE9001", "#B26501", "#663A00"},  // orange palette
            new ImmutableArray<string> () { "#99FFA4", "#4DFE5F", "#01FE1A", "#01B212", "#00660B"},  // green palette
            new ImmutableArray<string> () { "#99FFEF", "#4DFEE4", "#01FED8", "#01B297", "#006656"},  // light blue palette
            new ImmutableArray<string> () { "#99D6FF", "#4DB7FE", "#0199FE", "#016BB2", "#003D66"},  // steel blue palette
            new ImmutableArray<string> () { "#99A0FF", "#4D59FE", "#0112FE", "#010DB2", "#000766"},  // dark blue palette
            new ImmutableArray<string> () { "#FF99FF", "#FE4DFE", "#FE01FE", "#B201B2", "#660066"},  // pink palette
            new ImmutableArray<string> () { "#FF99A4", "#FE4D5F", "#FE011A", "#B20112", "#66000B"},  // red palette
        };
        public static readonly ImmutableArray<ImmutableArray<Color>> ColorPalettes =
            ColorValues.Select (row =>
                row.Select (c => (Color) ColorConverter.ConvertFromString (c))
                   .ToImmutableArray ())
            .ToImmutableArray ();
        public static ImmutableArray<Color> OrangePalette { get; } = ColorPalettes [0];
        public static readonly Color OrangePaletteUltraLightColor = OrangePalette [1];
        public static readonly Color OrangePaletteLightColor = OrangePalette [2];
        public static readonly Color OrangePaletteColor = OrangePalette [3];
        public static readonly Color OrangePaletteDarkColor = OrangePalette [4];
        public static readonly Color OrangePaletteUltraDarkColor = OrangePalette [5];

        public static readonly SolidColorBrush OrangePaletteUltraLightBrush = new SolidColorBrush (OrangePaletteUltraLightColor);
        public static readonly SolidColorBrush OrangePaletteLightBrush = new SolidColorBrush (OrangePaletteLightColor);
        public static readonly SolidColorBrush OrangePaletteBrush = new SolidColorBrush (OrangePaletteColor);
        public static readonly SolidColorBrush OrangePaletteDarkBrush = new SolidColorBrush (OrangePaletteDarkColor);
        public static readonly SolidColorBrush OrangePaletteUltraDarkBrush = new SolidColorBrush (OrangePaletteUltraDarkColor);
        public static ImmutableArray<Color> GreenPalette { get; } = ColorPalettes [1];
        public static readonly Color GreenPaletteUltraLightColor = GreenPalette [1];
        public static readonly Color GreenPaletteLightColor = GreenPalette [2];
        public static readonly Color GreenPaletteColor = GreenPalette [3];
        public static readonly Color GreenPaletteDarkColor = GreenPalette [4];
        public static readonly Color GreenPaletteUltraDarkColor = GreenPalette [5];

        public static readonly SolidColorBrush GreenPaletteUltraLightBrush = new SolidColorBrush (GreenPaletteUltraLightColor);
        public static readonly SolidColorBrush GreenPaletteLightBrush = new SolidColorBrush (GreenPaletteLightColor);
        public static readonly SolidColorBrush GreenPaletteBrush = new SolidColorBrush (GreenPaletteColor);
        public static readonly SolidColorBrush GreenPaletteDarkBrush = new SolidColorBrush (GreenPaletteDarkColor);
        public static readonly SolidColorBrush GreenPaletteUltraDarkBrush = new SolidColorBrush (GreenPaletteUltraDarkColor);
        public static ImmutableArray<Color> LightBluePalette { get; } = ColorPalettes [2];
        public static readonly Color LightBluePaletteUltraLightColor = LightBluePalette [1];
        public static readonly Color LightBluePaletteLightColor = LightBluePalette [2];
        public static readonly Color LightBluePaletteColor = LightBluePalette [3];
        public static readonly Color LightBluePaletteDarkColor = LightBluePalette [4];
        public static readonly Color LightBluePaletteUltraDarkColor = LightBluePalette [5];

        public static readonly SolidColorBrush LightBluePaletteUltraLightBrush = new SolidColorBrush (LightBluePaletteUltraLightColor);
        public static readonly SolidColorBrush LightBluePaletteLightBrush = new SolidColorBrush (LightBluePaletteLightColor);
        public static readonly SolidColorBrush LightBluePaletteBrush = new SolidColorBrush (LightBluePaletteColor);
        public static readonly SolidColorBrush LightBluePaletteDarkBrush = new SolidColorBrush (LightBluePaletteDarkColor);
        public static readonly SolidColorBrush LightBluePaletteUltraDarkBrush = new SolidColorBrush (LightBluePaletteUltraDarkColor);
        public static ImmutableArray<Color> SteelBluePalette { get; } = ColorPalettes [3];
        public static readonly Color SteelBluePaletteUltraLightColor = SteelBluePalette [1];
        public static readonly Color SteelBluePaletteLightColor = SteelBluePalette [2];
        public static readonly Color SteelBluePaletteColor = SteelBluePalette [3];
        public static readonly Color SteelBluePaletteDarkColor = SteelBluePalette [4];
        public static readonly Color SteelBluePaletteUltraDarkColor = SteelBluePalette [5];

        public static readonly SolidColorBrush SteelBluePaletteUltraLightBrush = new SolidColorBrush (SteelBluePaletteUltraLightColor);
        public static readonly SolidColorBrush SteelBluePaletteLightBrush = new SolidColorBrush (SteelBluePaletteLightColor);
        public static readonly SolidColorBrush SteelBluePaletteBrush = new SolidColorBrush (SteelBluePaletteColor);
        public static readonly SolidColorBrush SteelBluePaletteDarkBrush = new SolidColorBrush (SteelBluePaletteDarkColor);
        public static readonly SolidColorBrush SteelBluePaletteUltraDarkBrush = new SolidColorBrush (SteelBluePaletteUltraDarkColor);
        public static ImmutableArray<Color> DarkBluePalette { get; } = ColorPalettes [4];
        public static readonly Color DarkBluePaletteUltraLightColor = DarkBluePalette [1];
        public static readonly Color DarkBluePaletteLightColor = DarkBluePalette [2];
        public static readonly Color DarkBluePaletteColor = DarkBluePalette [3];
        public static readonly Color DarkBluePaletteDarkColor = DarkBluePalette [4];
        public static readonly Color DarkBluePaletteUltraDarkColor = DarkBluePalette [5];

        public static readonly SolidColorBrush DarkBluePaletteUltraLightBrush = new SolidColorBrush (DarkBluePaletteUltraLightColor);
        public static readonly SolidColorBrush DarkBluePaletteLightBrush = new SolidColorBrush (DarkBluePaletteLightColor);
        public static readonly SolidColorBrush DarkBluePaletteBrush = new SolidColorBrush (DarkBluePaletteColor);
        public static readonly SolidColorBrush DarkBluePaletteDarkBrush = new SolidColorBrush (DarkBluePaletteDarkColor);
        public static readonly SolidColorBrush DarkBluePaletteUltraDarkBrush = new SolidColorBrush (DarkBluePaletteUltraDarkColor);
        public static ImmutableArray<Color> PinkPalette { get; } = ColorPalettes [5];
        public static readonly Color PinkPaletteUltraLightColor = PinkPalette [1];
        public static readonly Color PinkPaletteLightColor = PinkPalette [2];
        public static readonly Color PinkPaletteColor = PinkPalette [3];
        public static readonly Color PinkPaletteDarkColor = PinkPalette [4];
        public static readonly Color PinkPaletteUltraDarkColor = PinkPalette [5];

        public static readonly SolidColorBrush PinkPaletteUltraLightBrush = new SolidColorBrush (PinkPaletteUltraLightColor);
        public static readonly SolidColorBrush PinkPaletteLightBrush = new SolidColorBrush (PinkPaletteLightColor);
        public static readonly SolidColorBrush PinkPaletteBrush = new SolidColorBrush (PinkPaletteColor);
        public static readonly SolidColorBrush PinkPaletteDarkBrush = new SolidColorBrush (PinkPaletteDarkColor);
        public static readonly SolidColorBrush PinkPaletteUltraDarkBrush = new SolidColorBrush (PinkPaletteUltraDarkColor);
        public static ImmutableArray<Color> RedPalette { get; } = ColorPalettes [6];
        public static readonly Color RedPaletteUltraLightColor = RedPalette [1];
        public static readonly Color RedPaletteLightColor = RedPalette [2];
        public static readonly Color RedPaletteColor = RedPalette [3];
        public static readonly Color RedPaletteDarkColor = RedPalette [4];
        public static readonly Color RedPaletteUltraDarkColor = RedPalette [5];

        public static readonly SolidColorBrush RedPaletteUltraLightBrush = new SolidColorBrush (RedPaletteUltraLightColor);
        public static readonly SolidColorBrush RedPaletteLightBrush = new SolidColorBrush (RedPaletteLightColor);
        public static readonly SolidColorBrush RedPaletteBrush = new SolidColorBrush (RedPaletteColor);
        public static readonly SolidColorBrush RedPaletteDarkBrush = new SolidColorBrush (RedPaletteDarkColor);
        public static readonly SolidColorBrush RedPaletteUltraDarkBrush = new SolidColorBrush (RedPaletteUltraDarkColor);

        static AnglrMonoChromaticColors () { }
    }
    public partial class AnglrPDASnapshotViewerTab : UserControl
    {
        public IAnglrLogger Logger { get; }
        public AnglrDebuggerGetPDAStack PdaSnapshot { get; }
        public ObservableCollection<AnglrDebuggerGetPDAStackCell> PDAStackCells { get; }
        public static ColumnWidths ColWidths { get; } = new ColumnWidths ();
        public AnglrPDASnapshotViewerTab (IAnglrLogger logger, AnglrDebuggerGetPDAStack pdaSnapshot)
        {
            InitializeComponent ();
            Logger = logger;
            PdaSnapshot = pdaSnapshot;
            DataContext = this;
            PDAStackCells = new ObservableCollection<AnglrDebuggerGetPDAStackCell> (PdaSnapshot.PDAStackCells);
            foreach (var column in pdaList.Columns)
            {
                if (ColWidths.TryGetValue (column.DisplayIndex, out double value))
                    column.Width = value;
                var dpd = DependencyPropertyDescriptor.FromProperty
                (
                    DataGridColumn.WidthProperty,
                    typeof (DataGridColumn)
                );
                dpd.AddValueChanged (column, (s, e) =>
                {
                    var col = s as DataGridColumn;
                    if (col == null)
                        return;
                    ColWidths [col.DisplayIndex] = col.ActualWidth;
                });
            }
        }

        private void pdaList_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            Logger?.InfoLine ($"selection changed, item count = {pdaList.SelectedItems.Count}");
            foreach (var item in e.AddedItems)
            {
                AnglrDebuggerGetPDAStackCell pDAStackCell = item as AnglrDebuggerGetPDAStackCell;
                if (pDAStackCell == null)
                    continue;
                Logger?.InfoLine ($"\tadded   item: {pDAStackCell.State}, {pDAStackCell.Code}, {pDAStackCell.Name}");
            }
            foreach (var item in e.RemovedItems)
            {
                AnglrDebuggerGetPDAStackCell pDAStackCell = item as AnglrDebuggerGetPDAStackCell;
                if (pDAStackCell == null)
                    continue;
                Logger?.InfoLine ($"\tremoved item: {pDAStackCell.State}, {pDAStackCell.Code}, {pDAStackCell.Name}");
            }
        }
    }
}
