using System;

namespace Mail.Plugin.MailKit.Test
{
	internal static class TimeExtension
	{
		internal static DateTime StripMiliSeconds(this DateTime dateTime) =>
			dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
	}
}
