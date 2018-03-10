# templategenerator

<b>Information:</b><br /><br />
It's a dynamic html generator and object parser via property names with reflection.<br />
Usable at send information mails, account registration / confirm emails, and more.

Example Usage:<br />
<code>
 var Output = new TemplateGenerator()<br />
.SetCharacterSet("utf-8")<br />
.SetGlobalFonts("Tahoma", "10px", 400)<br />
.LoadCssFromString("tr > td {padding: 5px;}")<br />
.OpenTable("600px", 0, 0, "left", "")<br />
.OpenTableBody()<br />
.OpenTableRow()<br />
.OpenAndCloseTableRowCell("", "", "Name:")<br />
.OpenAndCloseTableRowCell("", "", "{{FirstName}}")<br />
.OpenAndCloseTableRowCell("", "", "Surname:")<br />
.OpenAndCloseTableRowCell("", "", "{{LastName}}")<br />
.CloseTableRow()<br />
.OpenTableRow()<br />
.OpenAndCloseTableRowCell("", "", "Identity No:")<br />
.OpenAndCloseTableRowCell("", "", "{{IdentityNo}}")<br />
.OpenAndCloseTableRowCell("", "", "Email:")<br />
.OpenAndCloseTableRowCell("", "", "{{Email}}")<br />
.CloseTableRow()<br />
.CloseTableBody()<br />
.CloseTable()<br />
.SetParserMarkup("{{Key}}")<br />
.ParseValuesByObject(findedSimpleObject)<br />
.GetOutput();<br />
</code>
