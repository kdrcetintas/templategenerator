# templategenerator

Information:

It's a dynamic html generator and object parser via property names with reflection.

Usable at send information mails, account registration / confirm emails, and more.

Usage:

var Output = new TemplateGenerator()

.SetCharacterSet("utf-8")

.SetGlobalFonts("Tahoma", "10px", 400)

.LoadCssFromString("tr > td {padding: 5px;}")
.OpenTable("600px", 0, 0, "left", "")
.OpenTableBody()
.OpenTableRow()
.OpenAndCloseTableRowCell("", "", "Name:")
.OpenAndCloseTableRowCell("", "", "{{FirstName}}")
.OpenAndCloseTableRowCell("", "", "Surname:")
.OpenAndCloseTableRowCell("", "", "{{LastName}}")
.CloseTableRow()
.OpenTableRow()
.OpenAndCloseTableRowCell("", "", "Identity No:")
.OpenAndCloseTableRowCell("", "", "{{IdentityNo}}")
.OpenAndCloseTableRowCell("", "", "Email:")
.OpenAndCloseTableRowCell("", "", "{{Email}}")
.CloseTableRow()
.CloseTableBody()
.CloseTable()
.SetParserMarkup("{{Key}}")
.ParseValuesByObject(findedSimpleObject)
.GetOutput();
