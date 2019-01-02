# MailClient

[![Build status](https://dev.azure.com/fakhrulhilal/OpenSource/_apis/build/status/MailClient-CI)](https://dev.azure.com/fakhrulhilal/OpenSource/_build/latest?definitionId=3)
![version](https://img.shields.io/github/release/fakhrulhilal/mailclient.svg)
![license](https://img.shields.io/github/license/fakhrulhilal/mailclient.svg)

Simple mail client that uses MEF for pluggable sender/reader. 

Originally, this project is created for learning [MEF](https://docs.microsoft.com/en-us/dotnet/framework/mef/). Currently, only `sender` is implemented.

## Creating plugin

The plugins use following naming format `Mail.Plugin.[name]`. Then we can configure which plugin to use in `config.ini` file for either `sender` or `reader`. To create new plugin, simply implement [`Mail.Library.IMailSender`](Mail.Library/IMailSender.cs) for `sender` and [`Mail.Library.IMailReader`](Mail.Library/IMailReader.cs) for `reader`.

The plugin should be placed in the same folder as exe file.

