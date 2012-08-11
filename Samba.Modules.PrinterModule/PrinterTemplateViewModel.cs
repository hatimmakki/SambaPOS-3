﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ICSharpCode.AvalonEdit.Document;
using Samba.Domain.Models.Settings;
using Samba.Localization.Properties;
using Samba.Presentation.Common.ModelBase;
using Samba.Services;

namespace Samba.Modules.PrinterModule
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PrinterTemplateViewModel : EntityViewModelBase<PrinterTemplate>
    {
        private readonly IPrinterService _printerService;

        [ImportingConstructor]
        public PrinterTemplateViewModel(IPrinterService printerService)
        {
            _printerService = printerService;
        }

        public string Template { get { return Model.Template; } set { Model.Template = value; } }
        public string Layout { get { return Model.Layout; } set { Model.Layout = value; } }
        public bool MergeLines { get { return Model.MergeLines; } set { Model.MergeLines = value; } }

        public TextDocument TemplateText { get; set; }
        public TextDocument LayoutText { get; set; }

        public override Type GetViewType()
        {
            return typeof(PrinterTemplateView);
        }

        public override string GetModelTypeString()
        {
            return Resources.PrinterTemplate;
        }

        private IDictionary<string, string> _descriptions;
        public IDictionary<string, string> Descriptions
        {
            get { return _descriptions ?? (_descriptions = CreateDescriptions()); }
        }

        private IDictionary<string, string> CreateDescriptions()
        {
            return _printerService.CreateTagDescriptions();
        }

        protected override void Initialize()
        {
            base.Initialize();
            TemplateText= new TextDocument(Template);
            LayoutText = new TextDocument(Layout??"");
        }

        protected override void OnSave(string value)
        {
            Template = TemplateText.Text;
            Layout = LayoutText.Text;
            base.OnSave(value);
        }
    }
}
