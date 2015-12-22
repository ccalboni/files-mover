# Files Mover

A simple command-line program that moves files from a place to another following user-defined rules

## Language and envinronment

This is written in C# and compiles on both Xamarin Studio and Visual Studio. Currently runs on Windows and Mac, never tested on Linux but probably runs there too if Mono is available

## Rules

Rules are specified in a Json file placed in the same directory as the executable. Here's a sample:

    [
	    {
	      "Name": "Generic human-readable identifier for rule",
	      "ComputerName": "MyMac",
	      "SourceDirectory": "\\users\\myself\\downloads\\",
	      "DestinationDirectory": "\\users\\myself\\downloads\\videos\\",
	      "IncludeSubdirectories": false,
	      "Extensions": [
		    "mkv",
		    "avi",
		    "mp4",
		    "m4v"
	      ],
	      "OverwriteDestinationFiles": false
	    },
	    {
	      "Name": "Another rule name",
	      "ComputerName": "MyWin",
	      "SourceDirectory": "C:\\Users\\myself\\Desktop",
	      "DestinationDirectory": "C:\\Users\\myself\\Desktop\\Pictures",
	      "IncludeSubdirectories": false,
	      "Extensions": [
		    "jpg",
		    "png",
		    "gif",
		    "bmp",
		    "ico",
		    "psd"
	      ],
	      "OverwriteDestinationFiles": false
	    }
    ]

The structure is pretty self-explanatory. Here's a brief list of what each field do:

* **Name**: a generic name, optional (useful when reading application log), that identifies this rule
* **ComputerName**: you can run the program on different machines from, say, Dropbox. This helps when you want to deal with different rules on different machines. The software shows the machine it's running on at startup, just place that name in this field. Leave blank to run everywhere.
* **SourceDirectory**: path where to search files
* **DestinationDirectory**: path where files must be moved
* **IncludeSubdirectories**: include or not subdirectories of SourceDirectory. Valid values are true or false
* **Extensions**: a list, in form of json array, of extensions that you want to include. Use * to include every file found in SourceDirectory
* **OverwriteDestinationFiles**: true or false, tells the program what to do when a destination file with the same name exists.

## Startup options, command line parameters

No options is available to date. 

## FAQ

### Is the source directory structure preserved? 

No, it's not. All files are placed in the same destination folder. I'll look into this.

### What happens if an error occurs while overwriting?

The destination file is initially renamed (appending .backup to its name); only after a complete move of the new file the existing one is deleted. If something goes wrong while moving, you may end up with your original file renamed in something.ext.backup: just remove the .backup suffix.

### Is there a log file?

The log file "log.txt" is created in the same directory where program runs
