///-----------------------------------------------------------------
///   Namespace:        No Namespace
///   Class:            TemplateGenerator
///   Description:      Dynamic html template generator for .NET
///   Author:           @kdrcetintas
///   Date:             2018-01-01
///   Notes:            Enjoy it.
///   Revision History:

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public class TemplateGenerator
{
    #region Props
    private string HtmlStart = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN""><html>";
    private string HtmlEnd = "</html>";
    private bool HtmlWorker { get; set; }
    private string HeadStart = "<head>";
    private string HeadEnd = "</head>";
    private string Head { get; set; }
    private string BodyStart = "<body>";
    private string BodyEnd = "</body>";
    private string Body { get; set; }
    private string Content { get; set; }
    public string ParserMarkup { get; private set; } // Markup should be contains "Key" string, EG: {{Key}} or {%Key%}
    public List<KeyValuePair<string, string>> ErrorMessages { get; private set; }
    public string LastErrorMessage { get; private set; }
    public List<KeyValuePair<string, string>> EngineStyles { get; private set; }
    public void _EngineStyle(string Key, string Value)
    {
        this.EngineStyles.Add(new KeyValuePair<string, string>(Key, Value));
    }
    #endregion

    private void _Error(string Action, string Message)
    {
        this.ErrorMessages.Add(new KeyValuePair<string, string>(Action, Message));
        this.LastErrorMessage = Message;
    }
    public TemplateGenerator()
    {
        this.HtmlWorker = true;
        this.ErrorMessages = new List<KeyValuePair<string, string>>();
        this.EngineStyles = new List<KeyValuePair<string, string>>();
    }

    #region Main Engine
    public TemplateGenerator LoadTemplateFromFile(string FilePath)
    {
        this.HtmlWorker = false;
        if (File.Exists(FilePath))
        {
            this.Content = File.ReadAllText(FilePath, System.Text.Encoding.UTF8);
        }
        else
        {
            this._Error("LoadTemplateFromFile", String.Format("{0} file can't not found", FilePath));
        }
        return this;
    }
    public TemplateGenerator LoadTemplateFromString(string TemplateContent)
    {
        this.HtmlWorker = false;
        this.Content = TemplateContent;
        return this;
    }
    public string ParseHtmlValid(string Input)
    {
        return Input;
    }
    public string GetOutput()
    {
        if (this.HtmlWorker == true)
        {

            // Generate EngineStyles
            if (this.EngineStyles.Count() > 0)
            {
                this.Head += "<style>";
                foreach (var Group in this.EngineStyles.GroupBy(r => r.Key).FirstOrDefault())
                {
                    this.Head += String.Format("{0}", Group.Key);
                    this.Head += "{";
                    foreach (var cssItem in this.EngineStyles.Where(r => r.Key == Group.Key).ToList())
                    {
                        this.Head += String.Format("{0};", cssItem.Value);
                    }
                    this.Head += "}";
                }
                this.Head += "</style>";
            }
            // Generate EngineStyles

            return this.ParseHtmlValid(String.Concat(this.HtmlStart, this.HeadStart, this.Head, this.HeadEnd, this.BodyStart, this.Body, this.BodyEnd, this.HtmlEnd));
        }
        else
        {
            return this.Content;
        }
    }
    #endregion

    #region Html Engine
    private bool _CharacterSetAppended = false;
    public TemplateGenerator SetCharacterSet(string CharSet)
    {
        if (this._CharacterSetAppended == false)
        {
            this._CharacterSetAppended = true;
            this.Head += @"<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />";
        }
        return this;
    }
    public TemplateGenerator SetGlobalFonts(string FontFamily, string FontSize, int FontWeight)
    {
        this._EngineStyle("html, body", String.Format("font-family: {0}; font-size: {1}; font-weight: {2}", FontFamily, FontSize, FontWeight));
        return this;
    }
    public TemplateGenerator LoadCssFromFile(string FilePath, string Rel = "stylesheet")
    {
        if (File.Exists(FilePath))
        {
            this.Head += String.Format(@"<link href=""{0}"" rel=""{1}"" />", FilePath, Rel);
        }
        else
        {
            this._Error("LoadCssFromFile", String.Format("{0} file can't not found", FilePath));
        }
        return this;
    }
    public TemplateGenerator LoadCssFromString(string StyleString)
    {
        if (!String.IsNullOrEmpty(StyleString))
        {
            this.Head += String.Format(@"<style>{0}</style>", StyleString);
        }
        return this;
    }
    public TemplateGenerator LoadStyleByEngine(string Selector, string Style)
    {
        this._EngineStyle(Selector, Style);
        return this;
    }
    public TemplateGenerator WriteContent(string Content)
    {
        this.Body += Content;
        return this;
    }
    public TemplateGenerator WriteTag(string Tag, string Content, int HasClose = 1)
    {
        this.Body += String.Format("<{0}>{1}{2}", Tag, Content, (HasClose == 1 ? (String.Format("</{0}>", Tag)) : (String.Format("/>"))));
        return this;
    }
    public TemplateGenerator InsertLogo(string ImageUrl, string TargetUrl = "", string Align = "center", string MarginLeft = "0", string MarginRight = "0", string MarginTop = "0", string MarginBottom = "0", string PaddingLeft = "0", string PaddingRight = "0", string PaddingTop = "0", string PaddingBottom = "0")
    {
        this.Body += String.Format(@"<a href=""{0}"" style=""display: inline-block; margin: {1} {2} {3} {4}; padding: {5} {6} {7} {8};""><img src=""{9}"" /></a>", (!String.IsNullOrEmpty(TargetUrl) ? TargetUrl : "javascript:;"), MarginTop, MarginRight, MarginBottom, MarginLeft, PaddingTop, PaddingRight, PaddingBottom, PaddingLeft, ImageUrl);
        return this;
    }



    #endregion

    #region Table Engine
    public TemplateGenerator OpenTable(string Width = "600px", int CellPadding = 0, int CellSpacing = 0, string Align = "center", string Class = "")
    {
        this.Body += String.Format(@"<table style=""width: {0}"" cellpadding=""{1}"" cellspacing=""{2}"" align=""{3}"" class=""{4}"">", Width, CellPadding, CellSpacing, Align, Class);
        return this;
    }
    public TemplateGenerator OpenTableHead(string Class = "")
    {
        this.Body += String.Format(@"<thead class=""{0}"">", Class);
        return this;
    }
    public TemplateGenerator CloseTableHead()
    {
        this.Body += "</thead>";
        return this;
    }
    public TemplateGenerator OpenTableBody(string Class = "")
    {
        this.Body += String.Format(@"<tbody class=""{0}"">", Class);
        return this;
    }
    public TemplateGenerator CloseTableBody()
    {
        this.Body += "</tbody>";
        return this;
    }
    public TemplateGenerator OpenTableRow(string Class = "")
    {
        this.Body += String.Format(@"<tr class=""{0}"">", Class);
        return this;
    }
    public TemplateGenerator CloseTableRow(string Class = "")
    {
        this.Body += "</tr>";
        return this;
    }
    public TemplateGenerator OpenTableRowCell(string Class = "", string ColSpan = "1")
    {
        this.Body += String.Format(@"<td class=""{0}"" colspan=""{1}"">", Class, ColSpan);
        return this;
    }
    public TemplateGenerator CloseTableRowCell(string Class = "")
    {
        this.Body += "</td>";
        return this;
    }
    public TemplateGenerator OpenAndCloseTableRowCell(string Class, string ColSpan, string Content)
    {
        this.Body += String.Format(@"<td class=""{0}"" colspan=""{1}"">{2}</td>", Class, ColSpan, Content);
        return this;
    }
    public TemplateGenerator CloseTable()
    {
        this.Body += "</table>";
        return this;
    }

    #region AutoMethods

    #endregion

    #endregion

    #region Parser Engine
    public TemplateGenerator SetParserMarkup(string Markup)
    {
        this.ParserMarkup = Markup;
        return this;
    }
    public TemplateGenerator ParseValuesByList(IDictionary<string, string> Input)
    {
        if (!String.IsNullOrEmpty(ParserMarkup))
        {
            foreach (var item in Input)
            {
                this.Body = this.Body.Replace(this.ParserMarkup.Replace("Key", item.Key), item.Value);
            }
        }
        return this;
    }
    public TemplateGenerator ParseValuesByList(List<KeyValuePair<string, string>> Input)
    {
        if (!String.IsNullOrEmpty(ParserMarkup))
        {
            foreach (var item in Input)
            {
                this.Body = this.Body.Replace(this.ParserMarkup.Replace("Key", item.Key), item.Value);
            }
        }
        return this;
    }
    public TemplateGenerator ParseValuesByObject(object Input, int ReplaceEmptyStringNullValues = 1)
    {
        if (!String.IsNullOrEmpty(this.ParserMarkup))
        {
            try
            {
                foreach (var findedProp in Input.GetType().GetProperties())
                {
                    try
                    {
                        var findedValue = findedProp.GetValue(Input, null);
                        var outputValue = String.Empty;
                        if (findedValue != null)
                        {
                            outputValue = findedValue.ToString();
                            this.Body = this.Body.Replace(this.ParserMarkup.Replace("Key", findedProp.Name), outputValue);
                        }
                        else if (ReplaceEmptyStringNullValues == 1 && this.Body.IndexOf(this.ParserMarkup.Replace("Key", findedProp.Name)) >= 0)
                        {
                            this.Body = this.Body.Replace(this.ParserMarkup.Replace("Key", findedProp.Name), "");
                        }
                    }
                    catch (Exception e)
                    {
                        ;
                    }
                }
            }
            catch (Exception e)
            {
                ;
            }
        }
        return this;
    }
    #endregion

}
