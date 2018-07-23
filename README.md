![breakin logo](https://user-images.githubusercontent.com/6294155/43041061-0a0a2318-8d4d-11e8-862b-5fda00e82aac.png)

A Server Emulator for The Sims Bustin Out (PS2)'s Online Weekend mode, written in C# targeting .NET Core.

![image](https://user-images.githubusercontent.com/6294155/43072967-caabd6d4-8e6f-11e8-8b87-658ba5471cfe.png)


## FreeSO Hosted Server
A 24/7 Bustin Out PS2 server is hosted on http://freeso.org. Simply get a US copy of Bustin Out and PCSX2 with a DEV9 plugin. Set your DNS to `46.101.67.219` via either the plugin settings or ingame PS2 network configuration. This should be enough to connect to our server!

### Net Plugin
This plugin is a little crashy but it runs on even the latest version of PCSX2. Makes it very easy to change the DNS to our one!

![image](https://user-images.githubusercontent.com/6294155/43041312-84b754ee-8d54-11e8-9aba-940bdf656b2b.png)

https://forums.pcsx2.net/Thread-Experimental-Winsock-based-DEV9-plugin-Now-with-HDD-Support

### Keyboard
Think typing with an onscreen keyboard is a literal nightmare? Play the game with a controller and type with a keyboard instead, since you're emulating on a PC anyways.

**Plugin:**
https://forums.pcsx2.net/Thread-Another-USBQemu-plugin-this-time-in-VB-net-Keyboard-only

**Video:**
https://gyazo.com/9a956fd401e09523f00774d4937e70c8

## What is The Sims Bustin' Out Free Weekend?
The Free Weekend mode in Bustin' Out was the second arm of The Sims' short foray into online multiplayer. Unlike in The Sims Online, which is an MMO where you control one sim at a time, Bustin' Out lets the host control the entire family present at their house, while the invited player controls a visitor. 

The purpose of this mode was to let players help each other through the campaign mode (via trading money or unlocks), or show off their modifications to the different properties in the game.

## Game Instructions
First you'll want to find someone to play with. There is no chance you'll find a random player on the server, so you should invite your friends or some people from a Discord. The FreeSO official discord is probably a good place to do this.

Free Weekend mode is pretty easy to enter. It's available right from the main menu, then you basically just mash X until you get to the login screen. This includes mashing X through the PS2 Net Confgiuration, which will likely appear broken on your PCSX2.

![image](https://user-images.githubusercontent.com/6294155/43072944-b2b6451e-8e6f-11e8-86b6-cf2243ec40fb.png)

Join a lobby with someone else in it. You can chat in the lobby with the R1 button. If you invite someone to your game, they will come to the property you've saved on in single player mode.

Both players can pause the game at any time, though both players must hold R1 to enter high speed mode. You cannot enter Build/Buy, and job carpools are disabled, so make sure you've got everything you want on your lot before you mess around in multiplayer.


![image](https://user-images.githubusercontent.com/6294155/43041072-3938e78c-8d4d-11e8-863d-42484e9fc64f.png)
![image](https://user-images.githubusercontent.com/6294155/43072979-d6fe35b2-8e6f-11e8-927b-6ed1ad2a2718.png)

## Server Usage
Clone the repo into a folder and configure it using config.json. The only important thing to set here is your _external ip_, which is used by the redirection server the game first connects to.

Run the server using:
`dotnet run`
That's it for getting the server itself working. To get the game to connect to it, you need to trick the DNS into redirecting some EA and Sony URLs to your server.

A good way to do this is to host a DNS server. On Linux, the easiest way to do this is to install `dnsmasq`, then add the following to your hosts file:
```
199.195.251.151 gate1.us.dnas.playstation.org
<your public ip> tso-e.com
<your public ip> ps2sims04.ea.com
```
The top is a DNAS replacement server hosted by someone else.

Make sure ports `11100`, `10901` and `53` (dns) are open on your firewall.

## Future Stuff
There are a few unknowns in the server software right now, and some things are still a little messy. Specifically, the current mode, "challenge", boots into the player's current. It looks like there is another mode called "play", which might instead let the player use one of their freeplay houses. 

## Acknowledgements 

- Blayer98 from the FreeSO Community for noticing that Need for Speed Underground used the same connection packet as Bustin' Out sent.
- The creator of the NFSU Server, which was the basis of this server (although it was a bit incredibly unsafe for a server). (https://github.com/HarpyWar/nfsuserver/)
- Whoever recorded those packets for a (literally) underground PC/PS2 racing game in 2003, arguably before anyone even cared about this kind of preservation
- PCSX2 developers and the PS2 online community for making DNAS less of an insurmountable obstacle.
- FreeSO Team for not losing it when I decided to work on this for a whole day instead of important stuff. :laughing:
