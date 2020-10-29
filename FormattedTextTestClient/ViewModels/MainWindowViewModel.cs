using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CsvHelper;
using FormattedTextTestClient.Services;
using FormattedTextTestCommon;
using ReactiveUI;
using FontFamily = System.Windows.Media.FontFamily;

namespace FormattedTextTestClient.ViewModels
{
    internal class MainWindowViewModel : ReactiveObject
    {
        private FontFamilyConverter fontFamilyConverter = new FontFamilyConverter();

        //Text
        private string text = "テキスト RR.RR";
        public string Text
        {
            get => text;
            set => this.RaiseAndSetIfChanged(ref text, value);
        }

        //Font 
        private string selectedFont;
        public ReactiveList<string> Fonts { get; private set; }
        public string SelectedFont
        {
            get => selectedFont;
            set => this.RaiseAndSetIfChanged(ref selectedFont, value);
        }

        //Font styles
        private const string fontNormal = "Normal";
        private const string fontItalic = "Italic";
        private const string fontBold = "Bold";
        private const string fontRegular = "Regular";
        private const string fontBoldItalic = "BoldItalic";
        private string selectedFontStyle;
        public ReactiveList<string> FontStyles { get; private set; }
        public string SelectedFontStyle
        {
            get => selectedFontStyle;
            set => this.RaiseAndSetIfChanged(ref selectedFontStyle, value);
        }

        //Font size
        private int fontSize = 200;
        public int FontSize
        {
            get => fontSize;
            set => this.RaiseAndSetIfChanged(ref fontSize, value);
        }

        //FormattedTextMetrics .NET 3.5
        private FormattedTextMetrics formattedTextMetrics35;
        public FormattedTextMetrics FormattedTextMetrics35
        {
            get => formattedTextMetrics35;
            set => this.RaiseAndSetIfChanged(ref formattedTextMetrics35, value);
        }

        //FormattedTextMetrics .NET 4.6.2
        private FormattedTextMetrics formattedTextMetrics462;
        public FormattedTextMetrics FormattedTextMetrics462
        {
            get => formattedTextMetrics462;
            set => this.RaiseAndSetIfChanged(ref formattedTextMetrics462, value);
        }

        //FormattedTextMetrics Difference
        private FormattedTextMetricsDifference formattedTextMetricsDifference;
        public FormattedTextMetricsDifference FormattedTextMetricsDifference
        {
            get => formattedTextMetricsDifference;
            set => this.RaiseAndSetIfChanged(ref formattedTextMetricsDifference, value);
        }

        //Use .NET 3.5 Fallback Font
        private bool useNET35FallbackFont;
        public bool UseNET35FallbackFont
        {
            get => useNET35FallbackFont;
            set => this.RaiseAndSetIfChanged(ref useNET35FallbackFont, value);
        }

        //IncludeAllInkInBoundingBox
        private bool includeAllInkInBoundingBox;
        public bool IncludeAllInkInBoundingBox
        {
            get => includeAllInkInBoundingBox;
            set => this.RaiseAndSetIfChanged(ref includeAllInkInBoundingBox, value);
        }

        //CalculateCommand
        public ReactiveCommand CalculateCommand { get; private set; }

        //GenerateCSVCommand
        public ReactiveCommand GenerateCSVCommand { get; private set; }

        public MainWindowViewModel()
        {
            PopulateFonts();
            PopulateFontStyles();

            BehaviorSubject<bool> canExecute = new BehaviorSubject<bool>(true);

            CalculateCommand =
                ReactiveCommand.CreateFromTask(() => { return Task.Run(() => CalculateFormattedTextMetrics()); },
                    canExecute);
            GenerateCSVCommand = ReactiveCommand.CreateFromTask(() => { return Task.Run(() => GenerateCSV()); }, canExecute);

            Observable.CombineLatest(
                this.WhenAnyObservable(x => x.CalculateCommand.IsExecuting),
                this.WhenAnyObservable(x => x.GenerateCSVCommand.IsExecuting),
                this.WhenAny(
                    x => x.Text,
                    t => string.IsNullOrEmpty(t.Value))).Select(x => !x.Any(r => r)).Subscribe(canExecute);

            this.WhenAnyValue(
                    x => x.Text,
                    x => x.SelectedFont,
                    x => x.SelectedFontStyle,
                    x => x.FontSize,
                    x => x.UseNET35FallbackFont,
                    x => x.IncludeAllInkInBoundingBox)
                .Select(x => Unit.Default)
                .Throttle(TimeSpan.FromMilliseconds(300)).InvokeCommand(CalculateCommand);

            UseNET35FallbackFont = true;
            IncludeAllInkInBoundingBox = false;
        }

        private void GenerateCSV()
        {
            string outputFileName = string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}_{3}_{4}.csv",
                Text,
                SelectedFontStyle,
                FontSize,
                UseNET35FallbackFont ? "NET35FallbackFont" : string.Empty,
                IncludeAllInkInBoundingBox ? "IncludeAllInkInBoundingBox" : string.Empty);

            if (File.Exists(outputFileName))
            {
                File.Delete(outputFileName);
            }

            using (TextWriter textWriter = File.CreateText(outputFileName))
            {
                using (CsvWriter csvWriter = new CsvWriter(textWriter))
                {
                    IEnumerable<string> installedFonts =
                        new InstalledFontCollection().Families
                            .Where(family => !string.IsNullOrEmpty(family.Name))
                            .Select(family =>
                                ((FontFamily) fontFamilyConverter.ConvertFromString(family.Name)).Source)
                            .OrderBy(s => s);

                    csvWriter.WriteHeader<ConsolidatedFormattedTextMetrics>();
                    csvWriter.NextRecord();

                    foreach (string installedFont in installedFonts)
                    {
                        SelectedFont = installedFont;
                        CalculateFormattedTextMetrics();

                        ConsolidatedFormattedTextMetrics consolidatedFormattedTextMetrics =
                            new ConsolidatedFormattedTextMetrics(
                                installedFont, 
                                FormattedTextMetrics35,
                                FormattedTextMetrics462);
                        csvWriter.WriteRecord(consolidatedFormattedTextMetrics);
                        csvWriter.NextRecord();
                    }
                }
            }
        }

        private void CalculateFormattedTextMetrics()
        {
            FontStyle fontStyle = GetFontStyle(SelectedFontStyle);
            FontWeight fontWeight = GetFontWeight(SelectedFontStyle);

            //draw empty first
            FormattedTextService35.Instance.Draw(string.Empty, FontSize, SelectedFont, fontStyle.ToString(), fontWeight.ToString(), fontNormal);
            FormattedTextMetrics462 =
                FormattedTextService462.Instance.Draw(string.Empty, FontSize, SelectedFont, fontStyle.ToString(),
                    fontWeight.ToString(), fontNormal, UseNET35FallbackFont, IncludeAllInkInBoundingBox);

            //draw actual text
            FormattedTextMetrics35 =
                FormattedTextService35.Instance.Draw(Text, FontSize, SelectedFont, fontStyle.ToString(),
                    fontWeight.ToString(), fontNormal, UseNET35FallbackFont, IncludeAllInkInBoundingBox);
            FormattedTextMetrics462 =
                FormattedTextService462.Instance.Draw(Text, FontSize, SelectedFont, fontStyle.ToString(),
                    fontWeight.ToString(), fontNormal, UseNET35FallbackFont, IncludeAllInkInBoundingBox);

            FormattedTextMetricsDifference = new FormattedTextMetricsDifference(formattedTextMetrics35, formattedTextMetrics462);
        }

        private void PopulateFontStyles()
        {
            FontStyles = new ReactiveList<string> {fontRegular, fontItalic, fontBold, fontBoldItalic};

            SelectedFontStyle = FontStyles.First();
        }

        private void PopulateFonts()
        {
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();

            Fonts = new ReactiveList<string>();
            foreach (System.Drawing.FontFamily installedFontFamily in installedFontCollection.Families)
            {
                if (!string.IsNullOrEmpty(installedFontFamily.Name))
                {
                    FontFamily font = (FontFamily) fontFamilyConverter.ConvertFromString(installedFontFamily.Name);
                    Fonts.Add(font.Source);
                }
            }

            SelectedFont = Fonts.First();
        }

        private FontStyle GetFontStyle(string fontStyle)
        {
            switch (fontStyle)
            {
                case fontItalic:
                    return System.Windows.FontStyles.Italic;
                case fontBoldItalic:
                    return System.Windows.FontStyles.Italic;
            }

            return System.Windows.FontStyles.Normal;
        }

        private FontWeight GetFontWeight(string fontStyle)
        {
            switch (fontStyle)
            {
                case fontBold:
                    return FontWeights.Bold;
                case fontBoldItalic:
                    return FontWeights.Bold;
            }

            return FontWeights.Normal;
        }
    }
}
