using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MarkMars.PDF
{
    public class PXCp_Error
    {
        public static bool IS_DS_SUCCESSFUL(int x)
        {
            return (((x) & 0x80000000) == 0);
        }
        public static bool IS_DS_FAILED(int x)
        {
            return (((x) & 0x80000000) != 0);
        }

        public const int PS_ERR_REQUIRED_PROP_NOT_SET = unchecked((int)0x820F2716);

        [DllImport("xcpro40")]
        public static extern int PXCp_Err_FormatErrorCode(int errorcode, byte[] buf, int maxlen);
        [DllImport("xcpro40")]
        public static extern int PXCp_Err_FormatFacility(int errorcode, byte[] buf, int maxlen);
        [DllImport("xcpro40")]
        public static extern int PXCp_Err_FormatSeverity(int errorcode, byte[] buf, int maxlen);

        public static string BytesToString(byte[] bytes, int len)
        {
            string ret = "";
            for (int i = 0; i < len; i++)
            {
                if (bytes[i] == 0)
                    break;
                ret += (char)bytes[i];
            }
            return ret;
        }

        public static string GetDSErrorString(int x)
        {
            int sevLen = 0;
            int facLen = 0;
            int descLen = 0;

            byte[] sevBuf = null;
            byte[] facBuf = null;
            byte[] descBuf = null;

            sevLen = PXCp_Err_FormatSeverity(x, sevBuf, 0);
            facLen = PXCp_Err_FormatFacility(x, facBuf, 0);
            descLen = PXCp_Err_FormatErrorCode(x, descBuf, 0);

            sevBuf = new byte[sevLen];
            facBuf = new byte[facLen];
            descBuf = new byte[descLen];

            string s = "";
            if (PXCp_Err_FormatSeverity(x, sevBuf, sevLen) > 0)
                s = BytesToString(sevBuf, sevLen);
            s += " [";
            if (PXCp_Err_FormatFacility(x, facBuf, facLen) > 0)
                s += BytesToString(facBuf, facLen);
            s += "]: ";
            if (PXCp_Err_FormatErrorCode(x, descBuf, descLen) > 0)
                s += BytesToString(descBuf, descLen);
            return s;
        }

        public static void ShowDSErrorString(IWin32Window owner, int x)
        {
            MessageBox.Show(owner, GetDSErrorString(x), "PDFXCPro Demo");
        }
    }
}
