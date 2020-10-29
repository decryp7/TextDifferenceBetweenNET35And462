using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace FormattedTextTestCommon.Views
{
    public delegate void EmptyDelegate();

    public class VisualHost : FrameworkElement, IVisualHost
    {
        private readonly IList<DrawingVisual> visualChildren = new List<DrawingVisual>() {new DrawingVisual()};

        public VisualHost()
        {
            Draw("Please wait.", 72, new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal,
                FontStretches.Normal);
        }

        public void Draw(string text, int fontSize, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight,
            FontStretch fontStretch)
        {
            using (DrawingContext drawingContext = visualChildren.First().RenderOpen())
            {
                Typeface typeface = new Typeface(fontFamily, fontStyle,
                    fontWeight,
                    fontStretch);
     
                FormattedText formattedText = new FormattedText(text,
                    System.Globalization.CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);

                Rect formattedTextRect = new Rect(new Point(0, 0), new Size(formattedText.Width, formattedText.Height));

                //Reference: http://csharphelper.com/blog/2015/05/get-font-metrics-in-a-wpf-program-using-c/
                double formattedTextWithoutOverhangStartX = formattedText.OverhangLeading;
                double formattedTextWithoutOverhangStartY = formattedText.Baseline - typeface.CapsHeight * fontSize;
                double formattedTextWithoutOverhangWidth =
                    (formattedText.Width - (formattedText.OverhangTrailing < 0 ? 0 : formattedText.OverhangTrailing)) -
                    formattedText.OverhangLeading;
                double formattedTextWithoutOverhangHeight = (formattedText.Height + formattedText.OverhangAfter) -
                                                            (formattedText.Baseline - typeface.CapsHeight * fontSize);

                Rect formattedTextWithoutOverhang = new Rect(
                    new Point(formattedTextWithoutOverhangStartX, formattedTextWithoutOverhangStartY),
                    new Size(formattedTextWithoutOverhangWidth, formattedTextWithoutOverhangHeight));

                Rect overhangLeadingRect = new Rect(0, formattedTextWithoutOverhangStartY,
                    Math.Abs(formattedText.OverhangLeading), formattedTextWithoutOverhangHeight);
                Rect overhangTrailingRect = new Rect(
                    Math.Min(formattedText.Width, formattedText.Width - formattedText.OverhangTrailing),
                    formattedTextWithoutOverhangStartY, Math.Abs(formattedText.OverhangTrailing),
                    formattedTextWithoutOverhangHeight);

                drawingContext.DrawRectangle(Brushes.Gold, null, overhangLeadingRect);
                drawingContext.DrawRectangle(Brushes.Orange, null, overhangTrailingRect);

                drawingContext.DrawRectangle(null, new Pen(Brushes.Red, 1), formattedTextWithoutOverhang);
                drawingContext.DrawRectangle(null, new Pen(Brushes.DarkBlue, 1), formattedTextRect);

                drawingContext.DrawText(formattedText, new Point(0, 0));

                this.Width = Math.Max(formattedTextRect.Width, formattedTextRect.Width + (formattedText.OverhangTrailing * -1));
                this.Height = formattedTextRect.Height;
            }
        }

        #region Overrides of FrameworkElement

        protected override int VisualChildrenCount => visualChildren.Count;

        protected override Visual GetVisualChild(int index)
        {
            return visualChildren[index];
        }

        #endregion
    }
}
