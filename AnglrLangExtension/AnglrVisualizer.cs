using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrJsonRpcMethods;
using AnglrLibrary;
using AnglrLogLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AnglrLangExtension
{

    public class AnglrDrawingVisual : DrawingVisual
    {
        public static CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;
        public static FlowDirection FlowDirection { get; set; } = FlowDirection.LeftToRight;
        public static string TypefaceName { get; set; } = "Consolas";
        public static int FontSize { get; set; } = 14;
        public static Brush Brush { get; set; } = Brushes.Black;
        public static Pen Pen { get; set; } = new Pen (Brush, 1);
        public static Brush TerminalSymbolBackground { get; set; } = Brushes.LightGreen;
        public static Brush ConstantSymbolBackground { get; set; } = Brushes.LightGray;
        public static Brush NonTerminalSymbolBackground { get; set; } = Brushes.LightBlue;
        public static int Margin { get; set; } = 2;
        public static int RoundingRadius { get; set; } = 6;
        public static int ConnectorLength { get; set; } = 20;
        public int Index { get; protected set; }
    }

    public interface IAnglrVisualCloneable
    {
        AnglrDrawingVisual Clone ();
    }

    public interface IAnglrEventHandler
    {
        void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseEnter (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseLeave (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseMove (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
        void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger);
    }

    public class AnglrTerminalSymbolVisual : AnglrDrawingVisual, IAnglrVisualCloneable, IAnglrEventHandler
    {
        public void Draw (string name, int index)
        {
            Index = index;
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    name,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawLine (Pen, new Point (0, (height + 2 * Margin) / 2), new Point (ConnectorLength, (height + 2 * Margin) / 2));
                dc.PushTransform (new TranslateTransform (ConnectorLength, 0));
                dc.DrawRectangle (TerminalSymbolBackground, Pen, new Rect (0, 0, width + 2 * Margin, height + 2 * Margin));
                dc.DrawText (text, new Point (Margin, Margin));
                dc.Pop ();
            }
            Drawing.Freeze ();
        }

        public AnglrDrawingVisual Clone ()
        {
            AnglrTerminalSymbolVisual cloned = new AnglrTerminalSymbolVisual ();
            cloned.Index = Index;
            if (Transform != null)
                cloned.Transform = Transform.Clone ();
            cloned.Offset = Offset;
            using (var dc = cloned.RenderOpen ())
            {
                dc.DrawDrawing (Drawing.Clone ());
            }
            cloned.Drawing.Freeze ();
            return cloned;
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { logger?.InfoLine ($"mouse down in terminal symbol nr. {Index} at ({point})"); }
        public void OnMouseEnter (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrConstantSymbolVisual : AnglrDrawingVisual, IAnglrVisualCloneable, IAnglrEventHandler
    {
        public void Draw (string name, int index)
        {
            Index = index;
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    name,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawLine (Pen, new Point (0, (height + 2 * Margin) / 2), new Point (ConnectorLength, (height + 2 * Margin) / 2));
                dc.PushTransform (new TranslateTransform (ConnectorLength, 0));
                dc.DrawRectangle (ConstantSymbolBackground, Pen, new Rect (0, 0, width + 2 * Margin, height + 2 * Margin));
                dc.DrawText (text, new Point (Margin, Margin));
                dc.Pop ();
            }
            Drawing.Freeze ();
        }

        public AnglrDrawingVisual Clone ()
        {
            AnglrConstantSymbolVisual cloned = new AnglrConstantSymbolVisual ();
            cloned.Index = Index;
            if (Transform != null)
                cloned.Transform = Transform.Clone ();
            cloned.Offset = Offset;
            using (var dc = cloned.RenderOpen ())
            {
                dc.DrawDrawing (Drawing.Clone ());
            }
            cloned.Drawing.Freeze ();
            return cloned;
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { logger?.InfoLine ($"mouse down in constant symbol nr. {Index} at ({point})"); }
        public void OnMouseEnter (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrNonTerminalSymbolVisual : AnglrDrawingVisual, IAnglrVisualCloneable, IAnglrEventHandler
    {
        public void Draw (string name, int index)
        {
            Index = index;
            using (var dc = RenderOpen ())
            {
                FormattedText text = new FormattedText
                (
                    name,
                    CultureInfo,
                    FlowDirection,
                    new Typeface (TypefaceName),
                    FontSize,
                    Brush,
                    1.0
                );
                double width = text.Width;
                double height = text.Height;

                dc.DrawLine (Pen, new Point (0, (height + 2 * Margin) / 2), new Point (ConnectorLength, (height + 2 * Margin) / 2));
                dc.PushTransform (new TranslateTransform (ConnectorLength, 0));
                dc.DrawRoundedRectangle (NonTerminalSymbolBackground, Pen, new Rect (0, 0, width + 2 * Margin, height + 2 * Margin), RoundingRadius, RoundingRadius);
                dc.DrawText (text, new Point (Margin, Margin));
                dc.Pop ();
            }
            Drawing.Freeze ();
        }

        public AnglrDrawingVisual Clone ()
        {
            AnglrNonTerminalSymbolVisual cloned = new AnglrNonTerminalSymbolVisual ();
            cloned.Index = Index;
            if (Transform != null)
                cloned.Transform = Transform.Clone ();
            cloned.Offset = Offset;
            using (var dc = cloned.RenderOpen ())
            {
                dc.DrawDrawing (Drawing.Clone ());
            }
            cloned.Drawing.Freeze ();
            return cloned;
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { logger?.InfoLine ($"mouse down in non-terminal symbol nr. {Index} at ({point})"); }
        public void OnMouseEnter (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrContainerSymbolVisual : AnglrDrawingVisual, IAnglrVisualCloneable, IAnglrEventHandler
    {
        public AnglrDrawingVisual Clone ()
        {
            AnglrContainerSymbolVisual cloned = new AnglrContainerSymbolVisual ();
            cloned.Index = Index;
            if (Transform != null)
                cloned.Transform = Transform.Clone ();
            cloned.Offset = Offset;
            foreach (var child in Children)
            {
                if (!(child is IAnglrVisualCloneable))
                    continue;
                cloned.Children.Add ((child as IAnglrVisualCloneable)?.Clone ());
            }
            return cloned;
        }

        public void OnMouseDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { logger?.InfoLine ($"mouse down in container nr. {Index} at ({point})"); }
        public void OnMouseEnter (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeave (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseMove (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonDown (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseRightButtonUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
        public void OnMouseUp (object sender, MouseButtonEventArgs e, Point point, IAnglrLogger logger) { }
    }

    public class AnglrDrawingDictionary : Dictionary<int, AnglrContainerSymbolVisual> { }

    public static class AnglrSyntaxRuleDrawingBuilder
    {
        public static AnglrDrawingVisual DrawTerminalSymbol (string name, int index)
        {
            AnglrTerminalSymbolVisual visual = new AnglrTerminalSymbolVisual ();
            visual.Draw (name, index);
            return visual;
        }

        public static AnglrDrawingVisual DrawConstantSymbol (string name, int index)
        {
            AnglrConstantSymbolVisual visual = new AnglrConstantSymbolVisual ();
            visual.Draw (name, index);
            return visual;
        }

        public static AnglrDrawingVisual DrawNonTerminalSymbol (string name, int index)
        {
            AnglrNonTerminalSymbolVisual visual = new AnglrNonTerminalSymbolVisual ();
            visual.Draw (name, index);
            return visual;
        }

        public static AnglrDrawingDictionary BuildCanonicalSyntaxRulesDrawings (AnglrGetParserSyntaxRulesResult syntaxRulesResult)
        {
            AnglrDrawingDictionary SyntaxRuleVisuals = new AnglrDrawingDictionary ();
            foreach (var syntaxRule in syntaxRulesResult.SyntaxRuleList)
            {
                var syntaxRuleName = syntaxRule.SyntaxRuleName;
                var name = syntaxRuleName.Name;
                var id = syntaxRuleName.Id;
                foreach (var production in syntaxRule.Productions)
                {
                    int index = 0;
                    double horizontalOffset = 0.0;
                    AnglrContainerSymbolVisual container = new AnglrContainerSymbolVisual ();
                    var prodNr = production.ProductionNumber;
                    using (var dc = container.RenderOpen ())
                    {
                        if (production.RhsNodeSet.Length == 0)
                        {
                            AnglrDrawingVisual drawing = DrawConstantSymbol ("%empty", index++);
                            drawing.Offset = new Vector (horizontalOffset, 0);
                            container.Children.Add (drawing);
                            horizontalOffset += drawing.Drawing.Bounds.Width;
                        }
                        else
                        {
                            foreach (var rhsNode in production.RhsNodeSet)
                            {
                                AnglrDrawingVisual drawing = null;
                                if (rhsNode.Declarator == 18)
                                {
                                    if ((rhsNode.Synonym != null) && (rhsNode.Synonym.Length > 0))
                                        drawing = DrawConstantSymbol (rhsNode.Synonym, index++);
                                    else
                                        drawing = DrawTerminalSymbol (rhsNode.Name, index++);
                                }
                                else
                                    drawing = DrawNonTerminalSymbol (rhsNode.Name, index++);
                                drawing.Offset = new Vector (horizontalOffset, 0);
                                container.Children.Add (drawing);
                                horizontalOffset += drawing.Drawing.Bounds.Width;
                            }
                        }
                    }
                    SyntaxRuleVisuals [production.ProductionNumber] = container;
                }
            }
            return SyntaxRuleVisuals;
        }
    }

    internal class AnglrVisualizer : SyntaxTreeWalker
    {
        public AnglrVisualizer ()
        {
            _parser_part__Event += AnglrVisualizer__parser_part__Event;
            _anglr_syntax_rule_list__Event += AnglrVisualizer__anglr_syntax_rule_list__Event;
            _anglr_syntax_rule__Event += AnglrVisualizer__anglr_syntax_rule__Event;
            _anglr_nested_rule__Event += AnglrVisualizer__anglr_nested_rule__Event;
            _anglr_syntax_production_list_name__Event += AnglrVisualizer__anglr_syntax_production_list_name__Event;
            _anglr_syntax_production_list__Event += AnglrVisualizer__anglr_syntax_production_list__Event;
            _anglr_syntax_production__Event += AnglrVisualizer__anglr_syntax_production__Event;
            _production_name__Event += AnglrVisualizer__production_name__Event;
            _priority_assoc_specification__Event += AnglrVisualizer__priority_assoc_specification__Event;
            _priority_specification__Event += AnglrVisualizer__priority_specification__Event;
            _associativity_specification__Event += AnglrVisualizer__associativity_specification__Event;
            _name_list__Event += AnglrVisualizer__name_list__Event;
            _marker_list__Event += AnglrVisualizer__marker_list__Event;
            _marker__Event += AnglrVisualizer__marker__Event;
            _g_name__Event += AnglrVisualizer__g_name__Event;
            _name__Event += AnglrVisualizer__name__Event;
            _cardinality_delimiter__Event += AnglrVisualizer__cardinality_delimiter__Event;
            _cardinality__Event += AnglrVisualizer__cardinality__Event;
            _delimiter__Event += AnglrVisualizer__delimiter__Event;
        }

        private bool AnglrVisualizer__parser_part__Event (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_rule_list__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_list_.production_kind kind, _anglr_syntax_rule_list_ p__anglr_syntax_rule_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_rule__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_nested_rule__Event (SyntaxTreeCallbackReason reason, _anglr_nested_rule_.production_kind kind, _anglr_nested_rule_ p__anglr_nested_rule_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_production_list_name__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_.production_kind kind, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_production_list__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_.production_kind kind, _anglr_syntax_production_list_ p__anglr_syntax_production_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__anglr_syntax_production__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_.production_kind kind, _anglr_syntax_production_ p__anglr_syntax_production_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__production_name__Event (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__priority_assoc_specification__Event (SyntaxTreeCallbackReason reason, _priority_assoc_specification_.production_kind kind, _priority_assoc_specification_ p__priority_assoc_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__priority_specification__Event (SyntaxTreeCallbackReason reason, _priority_specification_.production_kind kind, _priority_specification_ p__priority_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__associativity_specification__Event (SyntaxTreeCallbackReason reason, _associativity_specification_.production_kind kind, _associativity_specification_ p__associativity_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__name_list__Event (SyntaxTreeCallbackReason reason, _name_list_.production_kind kind, _name_list_ p__name_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__marker_list__Event (SyntaxTreeCallbackReason reason, _marker_list_.production_kind kind, _marker_list_ p__marker_list_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__marker__Event (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__g_name__Event (SyntaxTreeCallbackReason reason, _g_name_.production_kind kind, _g_name_ p__g_name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__name__Event (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    AnglrDrawingVisual visual = null;
                    switch (kind)
                    {
                        case _name_.production_kind.g__name__1:
                            break;
                        case _name_.production_kind.g__name__2:
                            visual = AnglrSyntaxRuleDrawingBuilder.DrawConstantSymbol (p__name_.m__cstring_.text, -1);
                            break;
                        case _name_.production_kind.g__name__3:
                        {
                            SymbolToken p_SymbolToken = (SymbolToken) ((AppInfo) p__name_.appInfo) [AppInfoType.SymbolToken];
                            if (p_SymbolToken == null)
                                break;
                            if (p_SymbolToken.declarator != (uint) AnglrClassificationType.NonTerminalName)
                                visual = AnglrSyntaxRuleDrawingBuilder.DrawTerminalSymbol (p__name_.m__identifier_.text, -1);
                            else
                                visual = AnglrSyntaxRuleDrawingBuilder.DrawNonTerminalSymbol (p__name_.m__identifier_.text, -1);
                        }
                        break;
                    }
                    if (visual == null)
                        break;
                }
                break;
            }
            return true;
        }

        private bool AnglrVisualizer__cardinality_delimiter__Event (SyntaxTreeCallbackReason reason, _cardinality_delimiter_.production_kind kind, _cardinality_delimiter_ p__cardinality_delimiter_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__cardinality__Event (SyntaxTreeCallbackReason reason, _cardinality_.production_kind kind, _cardinality_ p__cardinality_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrVisualizer__delimiter__Event (SyntaxTreeCallbackReason reason, _delimiter_.production_kind kind, _delimiter_ p__delimiter_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }
    }
}
