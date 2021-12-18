
# General

**mcb1.bln** contains most files of the game, but they are copied in other archives *(dat/grp/scn/scn_sh/strap/ui.bin)*. We could work only on the mcb1.bln, but some files are copied in multiple locations within it, and others require to be modified in both the mcb1 and their bin archive.

[Kuriimu2](https://github.com/FanTranslatorsInternational/Kuriimu2) can help with modifying the BLN and some formats used in the game, but it can't import files to the BIN archives (unless you do it manually, which is harder).

With Strikers2013-Tools, you can extract the bin archives by going in **Archive**, selecting the desired one, and **Extract**. You can import files by creating a folder with all of the modified files, browse to the location of the mcb1.bln and the folder, then clicking on **Import**.

The files in the game are compressed with an LZ/RLE based compression, "Shade Lz" in Kuriimu. In most cases it's the regular one, but the files in dat.bin are compressed with the headerless version.

You can use [Scarlet](https://cdn.discordapp.com/attachments/697476778989584405/755511582833901578/Scarlet_Update.rar) (with a custom build that i'll describe later) or Kuriimu2 to decompress them all at once.

Don't forget to compress files before importing them.


# Text

There are three text files in the game (I'll use the dat.bin and mcb1.bln location):

- 0\14.bin (mcb1) / **1.bin** (dat) : main text file, contains most of the text of the game
- 31\37.bin and other locations (mcb1) / **5.bin** (dat): Moves descriptions, users and partners
- 61\2.bin / **113.bin** (dat) : Tutorial

In the **Text** tab, select the text file, and **Export** to export its contents to a txt file, and **Import** to import the txt file back to the original .bin file.

Texts use commands, described here (thanks to Alpha):
```
TEXT COMMANDS :
X refers to a number that you write
#f#= Tight outlines
#C#= Cancel previously used commands
#CXXXX#= custom rgba for the text
#dXXXX#= custom rgba for the outlines
#QX#= Outlines thickness
#Z#= Bold
#W#= Left alignment
#j#= Enables full width letters
#uXX#= letters width
#SXX#= Button size
#m#= No outlines
#I0#= Font used (three fonts : 0,1,2)
#Bx#= Buttons icons
#bx#= Wii icons
#N#= Return line
#M#= Shadow
#R#= Furigana
#zXX#= Scale
#oyXX#= Vertical position
#oxXX#= Horizontal position
#xXXX#= Spacing between letters
#P0#= gray text;
#P1#= gray shadow text
#P2#= dark blue text
#P3#= white / light gray text
#P4#= pure white text
#P5#= pure white text
#P6#= green text
#P7#= red text
#P8#= red text
#P9#= green text
#P10#= yellow text
#P11#= orange text
#P12#= orange text
```

# Textures

Start by extracting the whole **ui.bin** archive (that's the one that contains most of the **SHTXFS/SHTXFF/SHTXF4** - texture files). The regular version of Scarlet can handle the SHTXFS but the colors are inverted, which is why **Alpha** made a custom build : [Scarlet](https://cdn.discordapp.com/attachments/697476778989584405/755511582833901578/Scarlet_Update.rar). 

Drag-and-drop the extracted folder to Scarlet's executable, and it will convert all the textures to png.

To import a png back into a SHTX file, you can use Kuriimu2.


