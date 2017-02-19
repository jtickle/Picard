using Microsoft.Win32;

namespace Picard.Lib
{
    public class Configuration
    {
        const string REGKEY = @"Software\TickleSoft\Picard";
        const string JOURNALPATH = "JournalPath";
        const string LASTCMDR = "LastCmdr";

        protected RegistryKey PicardKey;

        public string JournalPath
        {
            get
            {
                return GetValue(JOURNALPATH, null);
            }
            set
            {
                SetValue(JOURNALPATH, value);
            }
        }

        public string LastCmdr
        {
            get
            {
                return GetValue(LASTCMDR, null);
            }
            set
            {
                SetValue(LASTCMDR, value);
            }
        }

        public Configuration()
        {
            PicardKey = Registry.CurrentUser.OpenSubKey(REGKEY, true);
        }

        public string GetValue(string key, string defaultValue)
        {
            object o = PicardKey.GetValue(key);
            return (o == null)
                ? defaultValue
                : (string)o;
        }

        public void SetValue(string key, string value)
        {
            PicardKey.SetValue(key, value);
        }
    }
}
