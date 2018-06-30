# Kotoha

[![license](https://img.shields.io/github/license/mika-f/kotoha.svg?style=flat-square)](./blob/develop/LICENSE)

"Kotoha" : Text-to-speech library for .NET powered by VOICEROID2 technology. 


## Usage

Very simple.

```csharp
using (var player = new KotohaPlayer())
{
    player.LoadPlugins($@"{Environment.CurrentDirectory}/plugins", recursive: true);
    player.Initialize();

    player.Speech("こんにちは", "Akane");
}
```

For more information, please see Sample Project.


## Donation

* BTC : `1NFcYczriqTEzVgzfurTMJNbhxPY1vyki9`
* ETH : `0xDc48d5AF8dCa1284F8770553E6e01beeE510535D`
* MONA : `MC4x87u1ffmhsRhHof1sHz8UAtNtKCTQut`
