C64 Studio is a .NET based IDE.

The program supports project based C64 assembly and/or several Basic dialects and is geared towards game development.
The internal assembler is using the ACME syntax, but supports also several other syntaxes.

In connection with Vice the IDE allows you to debug through your code and watch variables/memory locations, registers and memory.
Any other emulator can be set up as well if it's startable via runtime arguments.

C64 Studio allows you to compile to raw binary, .prg, .t64, .d64 or several cartridge formats (.bin and .crt for 8k, 16k, MagicDesk, RGCD, easyflash, Ultimax, GMOD2)

Additionally to this C64 Studio comes with a charset, sprite and media editor (tape and disk), supporting C64, C128, Mega65, VIC20.

An encompassing help documentation is part of the program. 


The latest WIP binary is always available at https://www.georg-rottensteiner.de/webmisc/C64StudioRelease.zip

The archive contains binaries for .NET 3.5, .NET 4.8 and .NET 6.0. They are technically identical but the 6.0 is the only one with direct GIT support.







How to run under Linux:
=======================
1)
I've had success using PlayOnLinux on Ubuntu, which does require to run DotNETFX installer first.

2)
Tested on Linux Mint 21.3, Cinnamon Edition

follow the instructions in the following guide, reproduced below for convenience:
https://wine.htmlvalidator.com/install-wine-on-linux-mint-21.html

sudo mkdir -pm755 /etc/apt/keyrings
wget -O /etc/apt/keyrings/winehq-archive.key https://dl.winehq.org/wine-builds/winehq.key
sudo wget -NP /etc/apt/sources.list.d/ https://dl.winehq.org/wine-builds/ubuntu/dists/jammy/winehq-jammy.sources
sudo apt update
sudo apt install --install-recommends winehq-stable
wine --version
wine winecfg
wine iexplore

Download C64 Studio in your browser. It will appear in Downloads in your home folder.
Right-click and use the Extract Here command

To make a desktop shortcut in Mint:
right-click on the desktop
Create a new launcher here...
Name:
C64 Studio 7.8
Command:
wine /home/me/Downloads/C64StudioRelease/net4.8/C64Studio.exe
