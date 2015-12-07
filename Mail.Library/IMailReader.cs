using System.Collections.Generic;

namespace Mail.Library
{
	public interface IMailReader
	{
		IEnumerable<Message> Read(ReadConnection connection);
	}
}
