# Monitor-Screenshotter
Just a lightweight C# program that screenshots monitors... because there's no quick way or one button solution built into Windows to screenshot JUST ONE monitor.

# How To Use
1. Unzip somewhere
2. Run the EXE

Alternatively, you can send an argument whether it be through batch file or whatever (I personally use this program for my Stream Deck), to screenshot a specific monitor.
Just pass a number (0 being the main monitor, any number higher being any other monitor) and it will screenshot that given monitor instead.

# Settings
In the settings.txt file you can change the following:
- OutputDirectory (where to save the screenshots)
- PlayAudio (whether to play the screenshot sound, on by default)
- CopyToClipboard (copies the screenshot to your clipboard, on by default)
- OpenOnSuccess (opens the file in your default photo viewer, off by default)
