# LMS511Laser
SICK LMS511 laser driver
 About
-------
LMS511Laser is a SICK LMS 511 Pro Laser device driver package in Windows OS, Windows7, Windows 8.1. 
Device technical details:
https://www.sick.com/de/en/detection-and-ranging-solutions/2d-laser-scanners/lms5xx/lms511-10100-pro/p/p215941

 Building from sources
-----------------------
you don't need to install anything else.
At the moment, projects for Visual Studio 2013 are provided, used the.NET Framework 4.5.
Just open solution (sln) file: .\LMS511Laser\master\src\LMS511Laser.sln
After you build it.
The solution will download more dependent references dll binary files with Nuget tool of VS2013.
It will be builded to .\LMS511Laser\master\output\x86\ directory 
Shared.DeviceDrivers.LMS511Laser.dll and necessary dll files.

 Where can I get example, tester?
--------------------------------
GUI application is LMS511LaserApplication.exe;
Look at example of folder:
.\LMS511Laser\master\src\LMS511LaserApplication\bin\x86\Debug
Before running execute program you copy lms511.ini file from
.\LMS511Laser\master\enviroment\C\public\LMS511
to
c:\Users\Public\LMS511\ folder.

lms511.ini content:
[general]
factory=0
ip_address=192.168.0.66  // laser ip address
trigger_output_channel_number=1 // trigger output number
[password]
maintenance=B21ACE26
authorized_client=F4724744
service=81BE23AA

Passwords are defaults, so main, client, Service, there are generated SOPAS Engineering Tool.
https://www.sick.com/de/en/search?text=SOPAS 

 Using latest development versions
-----------------------------------
LMS511Laser uses git and and the sources are hosted on GitHub at
https://github.com/ptiszai/LMS511Laser

 Where to get Sick communication protocol?
-------------------------------------------
Telegrams_for_Configuring_and_Operating.pdf 
Look at doc of folder:
.\LMS511Laser\master\doc\
Product company site:
www.sick.com
