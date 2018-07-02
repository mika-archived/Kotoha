# Kotoha

[![license](https://img.shields.io/github/license/mika-f/kotoha.svg?style=flat-square)](./blob/develop/LICENSE)

Kotoha is a .NET library that provides a common API for operations such as VOICEROID2.


## Usage

Very simple.

```csharp
using (var player = new KotohaPlayer())
{
    player.LoadPlugins($@"{Environment.CurrentDirectory}/plugins", recursive: true);
    player.Initialize();

    await player.SpeechAsync("こんにちは", "Aoi");
}
```

For more information, please see Sample Project.


## Plugins

### Engine

Engine is UI automation / API call implementation for Text-To-Speech engines.  
For example:

* Engine provides Web API / COM / DLL library -> You should implement API call for Kotoha.
* Engine doesn't provide APIs, but it has GUI -> You should implement UI automation for Kotoha.

Kotoha use `Codeer.Friendly` for UI automation. You can use and find it in `Kotoha.Plugin.Automation` namespace classes.

Example implementation is [`Kotoha.Engine.Voiceroid2`](Source/Kotoha.Engine.Voiceroid2/Voiceroid2Engine.cs).  


#### Lifecycle

All plugins are loaded when `LoadPlugins` is called by Application Host.  
But, Kotoha doesn't launch/find backend engines (e.g. VOICEROID, CeVIO).

```
    +-----------+-----------+
    |     LoadPlugins()     |
    +-----------+-----------+
                |
    +-----------+-----------+
    |     constructor()     |
    +-----------+-----------+
                +<------------------------------------------------------------------------------------+
    +-----------+-----------+                                                                         |
    | SpeechAsync(str, str) +                                                                         |
    +-----------+-----------+                                                                         |
                |                                                                                     |
                |                       +-----------+-----------+                                     |
       Already Initialized? ---- No --->+ FindCurrentProcess()  |                                     |
                |                       +-----------+-----------+                                     |
               Yes                                  |                     +-----------------------+   |
                |                           Already Launched? ---- No --->+  FindMainExecutable() |   |
                |                                   |                     +-----------+-----------+   |
                |                                  Yes                                |               |
                |                                   |                                 |               |
                |                       +-----------+-----------+                     |               |
                |                       |   Initialize(IntPtr)  +<--------------------+               |
                |                       +-----------+-----------+                                     |
                |                                   |                                                 |
                +<----------------------------------+                                                 |
                |                                                                                     |
    +-----------+-----------+                                                                         |
    | SpeechAsync(str,talk) +-------------------------------------------------------------------------+
    +-----------+-----------+
                |
    +-----------+-----------+
    |       Dispose()       |
    +-----------+-----------+             
```


### Talkers

You can choose 2-type plugin of talker.

1. .NET library.
2. JSON configuration.

Both plugins offer the same thing.

* `id` : Unique ID for plugin. I recommend roma-ji style name (e.g. 琴葉葵 is Akane)
* `name` : Plugin name that you like. You may need to follow the engine.
* `engine` : Text-To-Speech engine class such as VOICEROID2, CeVIO...


#### .NET library

1. Create a new project as .NET Desktop library.
1. Set reference to `Kotoha.Plugin`.
1. Implement `IKotohaTalker` interface.
1. Build


#### JSON configuration

You write JSON as below example.

```json
[
  {
    "id": "Akane",
    "name": "琴葉 茜",
    "engine": "Voiceroid2Engine"
  },
  {
    "id": "Aoi",
    "name": "琴葉 葵",
    "engine": "Voiceroid2Engine"
  }
]
```

and load.

```csharp
player.LoadConfigs("/path/to/talkers_configuration.json");
```



## Donation

If you want to support me, you can donate cryptocurrencies here.  
https://mochizuki.moe/donation
