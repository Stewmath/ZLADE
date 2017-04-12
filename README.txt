=Zelda Link's Awakening Level Editor=

First off, I'd like to thank Jigglysaint and everyone who helped me with this. This would have never been made if Jigglysaint didn't help me. The programming was done completely by me (Lin), and was done in C# .NET.

-Error Help-
"Access Denied":
	Make sure the ROM you're trying to open is NOT read-only. This will throw an exception.
Error on start-up:
	One or more of the files are missing. Make sure all the images and the sprites.txt is there.

File Structure
	-Images Folder
		-Sprites Folder
		-All the tilesets
	-ZLADE.exe
	-Map Planner.exe
	-README.txt
	-Sprites.txt

If one or more of the files are missing, re-extract the zip and make sure the file structure is the same as the one above.

-Q&A-
Q: I add a key block in the editor, but it doesn't show properly in-game. Why?
A: The key block is a special object, and it loads depending on the room. I'd recommend re-doing the room that has the proper graphics and properties to use it. That, or change the graphics it uses.

Q: How do I make a chest from an event not go away after exiting the screen?
A: For every object that appears from an event, place an object with the ID AFTER it where it would appear. For example, place the empty chest at the 8, 2 position.

Q: Why does the minimap not load properly?
A: You're using an unsupported version with different data addresses than expected. Try changing the version from the Edit menu.

Q: Will there ever be an overworld editor?
A: Hopefully.

That's it for the read-me. Thanks for using ZLADE, and happy hacking!