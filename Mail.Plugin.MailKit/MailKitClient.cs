using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Mail.Library;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Mail.Plugin.MailKit
{
    /// <summary>
    /// Email client using MailKit library
    /// </summary>
    [Export(typeof(IMailSender))]
    [ExportMetadata("Name", "MailKit")]
    public class MailKitClient : IMailSender, IPluginMetadata, IDisposable
    {
        private SmtpClient client;

        private readonly Type[] connectionExceptions =
        {
            typeof(NotSupportedException), typeof(IOException),
            typeof(SmtpCommandException), typeof(SmtpProtocolException)
        };

        private readonly Type[] authenticationExceptions =
        {
            typeof(NotSupportedException), typeof(AuthenticationException),
            typeof(SaslException), typeof(IOException),
            typeof(SmtpCommandException), typeof(SmtpProtocolException)
        };

        private readonly Type[] sendExceptions =
        {
            typeof(InvalidOperationException), typeof(NotSupportedException),
            typeof(IOException), typeof(SmtpCommandException), typeof(SmtpProtocolException)
        };

        public string Name => "MailKit";

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="connection">connection to SMTP server</param>
        /// <param name="message">email message</param>
        /// <param name="errorMessage">error message if any</param>
        /// <returns><code>false</code> when fails (see <paramref name="errorMessage"/> for more details) and vice versa</returns>
        public bool Send(SendConnection connection, Message message, out string errorMessage)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (message == null) throw new ArgumentNullException(nameof(message));
            var validation = connection.Validate();
            if (!validation.IsValid)
            {
                errorMessage = string.Join("\n", validation.Messages);
                return false;
            }

            validation = message.Validate();
            if (!validation.IsValid)
            {
                errorMessage = string.Join("\n", validation.Messages);
                return false;
            }

            errorMessage = null;
            using (client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                try
                {
                    client.Connect(
                        connection.Server,
                        connection.Port,
                        connection.Security == SecureType.None ? SecureSocketOptions.None : SecureSocketOptions.Auto);
                }
                catch (InvalidOperationException)
                { }
                catch (Exception exception) when (connectionExceptions.Any(ex => ex.IsAssignableFrom(exception.GetType())))
                {
                    errorMessage = $"Can't establish connection to {connection.Server}:{connection.Port} because of: {exception.GetLeafException()}";
                    return false;
                }

                if (!string.IsNullOrEmpty(connection.Username) &&
                    !string.IsNullOrEmpty(connection.Password))
                    try
                    {
                        client.Authenticate(connection.Username, connection.Password);
                    }
                    catch (InvalidOperationException)
                    { }
                    catch (Exception exception)
                        when (authenticationExceptions.Any(ex => ex.IsAssignableFrom(exception.GetType())))
                    {
                        errorMessage = $"Can't authenticate to {connection.Server}:{connection.Port} using username '{connection.Username}' because of: {exception.GetLeafException()}";
                        return false;
                    }

                try
                {
                    client.Send(message.Convert());
                }
                catch (Exception exception) when (sendExceptions.Any(ex => ex.IsAssignableFrom(exception.GetType())))
                {
                    errorMessage = $"Can't send message because of: {exception.GetLeafException()}";
                    return false;
                }
            }

            return true;
        }

        void IDisposable.Dispose()
        {
            if (client != null)
            {
                if (client.IsConnected) client.Disconnect(true);
                client.Dispose();
                client = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}
