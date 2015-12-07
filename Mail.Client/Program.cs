using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Mail.Library.Configuration;

namespace Mail.Client
{
    static class Program
    {
        private static string ApplicationPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var importer = new Importer();
            string configPath = Path.Combine(ApplicationPath, "config.ini");
            var configuration = new IniConfiguration(configPath);
            try
            {
                configuration.Parse();
            }
            catch (FileNotFoundException) // ignore missing file, we'll use default setting
            { }
            importer.Config = configuration.GetConfig<Configuration.Library>();
            importer.Compose(importer.Config);
            if (importer.Sender == null)
            {
                MessageBox.Show($"Can't load mail sender for {importer.Config.Sender}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new MainForm(importer.Sender, importer.Reader, configuration));
        }
    }
}
