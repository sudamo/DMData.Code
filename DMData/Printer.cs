using System.Text;
using System.Web.UI;

namespace DMData.Code
{
    public class Printer
    {
        public static void Register(Page page)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<script>var printerName=\"\";");
            stringBuilder.Append("function checkPrinter(printerName){var returnValue=false;try{var network=new ActiveXObject(\"WScript.Network\");}catch(exp){return returnValue;}var arrPrinters=network.EnumPrinterConnections();for(var i=0;i<arrPrinters.length;i+=2){if(arrPrinters.item(i+1)==printerName){returnValue=true;break;}}delete network;return returnValue;}");
            stringBuilder.Append("function getFielder(idx){var fso=new ActiveXObject(\"Scripting.FileSystemObject\");var folder=fso.GetSpecialFolder(1);var fielder;switch(idx){case 1:fielder=folder+'\\\\WinPrint\\\\WinPrint1.dll';break;case 2:fielder=folder+'\\\\WinPrint\\\\WinPrint2.dll';break;case 3:fielder=folder+'\\\\WinPrint\\\\WinPrint3.dll';break;case 4:fielder=folder+'\\\\WinPrint\\\\WinPrint4.dll';break;case 5:fielder=folder+'\\\\WinPrint\\\\WinPrint5.dll';break;}delete fso;return fielder;}");
            stringBuilder.Append("</script>");
            page.RegisterClientScriptBlock("PRINTER_SCRIPT", stringBuilder.ToString());
        }
    }
}