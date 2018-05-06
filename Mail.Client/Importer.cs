using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using Mail.Library;

namespace Mail.Client
{
    public class Importer
	{
		[ImportMany]
		public IEnumerable<Lazy<IMailSender, IPluginMetadata>> Senders { get; set; }

		[ImportMany]
		public IEnumerable<Lazy<IMailReader, IPluginMetadata>> Readers { get; set; }

		internal IMailSender Sender { get; private set; }
		internal IMailReader Reader { get; private set; }
		internal Configuration.Library Config { get; set; }

		internal void Compose(Configuration.Library config)
		{
			var aggregateCatalog = new AggregateCatalog();
			aggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(IPluginMetadata).Assembly));
			string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
			aggregateCatalog.Catalogs.Add(new DirectoryCatalog(location, @"Mail.Plugin.*.dll"));
			var container = new CompositionContainer(aggregateCatalog);
			try
			{
				container.ComposeParts(this);
				Sender = Senders.FirstOrDefault(sender => sender.Metadata.Name.Equals(config.Sender))?.Value;
				Reader = Readers.FirstOrDefault(reader => reader.Metadata.Name.Equals(config.Reader))?.Value;
				aggregateCatalog.Dispose();
				container.Dispose();
			}
			catch (CompositionException)
			{
			}
		}
	}
}
