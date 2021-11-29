
====================================
    Yandere Simulator Launcher 2
====================================

Release Notes:
Version 2.9.0
 - Special thanks to GitHub users: counter185, Ramobo, and nolemretaWxd.
 - (GitHub PR #12): (Contributed by GitHub user counter185) Download progress is now updated on the launcher taskbar icon.
 - New yan/dere images and videos.
 - (GitHub Issue #7): Fixed "Unknown Error" if launcher could access YandereSimulator.com but not login to Mega.nz. (Logout code was called without checking that a login occurred.)
 - (GitHub Issue #13): Launcher will now let the user know if their Windows is 32-bit (and thus cannot play Yandere Simulator since the Unity upgrade on April 10th, 2020).
 - (GitHub Issue #3): Launcher will now attempt to warn users if they are running from a Temp folder (or the system folder). Running the launcher from within a Zip prevented the launcher from finding previous installs of Yandere Simulator. (I also found out that it could cause occasional Mega.nz issues if the user cannot write to the launcher's current working directory... although the bulk of Mega.nz failures are caused by network errors.)

Version 2.8.0
 - (GitHub Issue #5): Allow TLS 1.2 (and added more fallbacks in case settings are changed again).
 - Updated Mega.nz from NuGet to support new URL format.
 - (GitHub Issue #2): Fixed launcher not reseting when an unknown exception is encountered on install.

Version 2.7.1
 - Initial public release

Known Issues:
 - The launcher can fail to download with an error message stating "Mega.nz"
     => Most of the time, it's just Mega.nz being overloaded. It usually fixes itself within a few (3-4) hours. Mega.nz is a free service and we connect to it anonymously.
     => Could be caused by internet problems, often related to cellular hotspots, school or office internet, etc. interfering with large file downloads.
     => If some progress is made (ex: 1%, 20%, etc.) then it could be unstable WiFi.
     => You could have places the launcher in a folder that you don't have write access to.
     => You could be running YandereSimulatorLauncher2.exe without extracting it from YandereSimLauncher.zip. I have seen Windows set the working directory to C:\Windows\System32, which the launcher cannot write to. Extract YandereSimLauncher.zip into a folder, and run the YandereSimulatorLauncher2.exe that is in that folder.

 - The launcher says "Install" every time that it is run.
     => Extract YandereSimLauncher.zip into a folder, and run the YandereSimulatorLauncher2.exe that is in that folder.
     => The launcher install the game in a folder called YandereSimulator that is in the same folder as YandereSimulatorLauncher2.exe. This is so the game can be installed onto external (ex: USB) hard drives.

 - Moving the window between monitors can make the video (temporarily) freeze and/or black-out.
     => The launcher uses Windows Media Video for file size and compatibility.

 - Some effects (ex: the video background) require Desktop Composition ("Aero") to be enabled.

 - Closing the launcher while it is unpacking can corrupt the unzip process.
     => The next time you open the launcher, the launcher should say "Install" or
        "Update available". That will attempt to fix (re-install) Yandere Simulator.
     => You can also download a new copy of the launcher from YandereDev's website.

=========================================================================================================
=========================================================================================================
=========================================================================================================

Source code is licensed under the permissive MIT license
Assets are copyright their respective owners (ex: YandereDev).

Copyright 2020 Scott Michaud

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
associated documentation files (the "Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

=========================================================================================================
=========================================================================================================
=========================================================================================================

Contains the "League Gothic" font. 
License included below:

Copyright (c) 2010, Caroline Hadilaksono & Micah Rich <caroline@hadilaksono, micah@micahrich.com>,
with Reserved Font Name: "League Gothic".

	This Font Software is licensed under the SIL Open Font License, Version 1.1.
	This license is copied below, and is also available with a FAQ at:
	http://scripts.sil.org/OFL

	Version 1.1 - 26 February 2007


SIL Open Font License
====================================================


Preamble
----------

The goals of the Open Font License (OFL) are to stimulate worldwide
development of collaborative font projects, to support the font creation
efforts of academic and linguistic communities, and to provide a free and
open framework in which fonts may be shared and improved in partnership
with others.

The OFL allows the licensed fonts to be used, studied, modified and
redistributed freely as long as they are not sold by themselves. The
fonts, including any derivative works, can be bundled, embedded, 
redistributed and/or sold with any software provided that any reserved
names are not used by derivative works. The fonts and derivatives,
however, cannot be released under any other type of license. The
requirement for fonts to remain under this license does not apply
to any document created using the fonts or their derivatives.

Definitions
-------------

`"Font Software"` refers to the set of files released by the Copyright
Holder(s) under this license and clearly marked as such. This may
include source files, build scripts and documentation.

`"Reserved Font Name"` refers to any names specified as such after the
copyright statement(s).

`"Original Version"` refers to the collection of Font Software components as
distributed by the Copyright Holder(s).

`"Modified Version"` refers to any derivative made by adding to, deleting,
or substituting -- in part or in whole -- any of the components of the
Original Version, by changing formats or by porting the Font Software to a
new environment.

`"Author"` refers to any designer, engineer, programmer, technical
writer or other person who contributed to the Font Software.

Permission & Conditions
------------------------

Permission is hereby granted, free of charge, to any person obtaining
a copy of the Font Software, to use, study, copy, merge, embed, modify,
redistribute, and sell modified and unmodified copies of the Font
Software, subject to the following conditions:

1. Neither the Font Software nor any of its individual components,
in Original or Modified Versions, may be sold by itself.

2. Original or Modified Versions of the Font Software may be bundled,
redistributed and/or sold with any software, provided that each copy
contains the above copyright notice and this license. These can be
included either as stand-alone text files, human-readable headers or
in the appropriate machine-readable metadata fields within text or
binary files as long as those fields can be easily viewed by the user.

3. No Modified Version of the Font Software may use the Reserved Font
Name(s) unless explicit written permission is granted by the corresponding
Copyright Holder. This restriction only applies to the primary font name as
presented to the users.

4. The name(s) of the Copyright Holder(s) or the Author(s) of the Font
Software shall not be used to promote, endorse or advertise any
Modified Version, except to acknowledge the contribution(s) of the
Copyright Holder(s) and the Author(s) or with their explicit written
permission.

5. The Font Software, modified or unmodified, in part or in whole,
must be distributed entirely under this license, and must not be
distributed under any other license. The requirement for fonts to
remain under this license does not apply to any document created
using the Font Software.

Termination
-----------

This license becomes null and void if any of the above conditions are
not met.


	DISCLAIMER

	THE FONT SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
	EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO ANY WARRANTIES OF
	MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT
	OF COPYRIGHT, PATENT, TRADEMARK, OR OTHER RIGHT. IN NO EVENT SHALL THE
	COPYRIGHT HOLDER BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
	INCLUDING ANY GENERAL, SPECIAL, INDIRECT, INCIDENTAL, OR CONSEQUENTIAL
	DAMAGES, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
	FROM, OUT OF THE USE OR INABILITY TO USE THE FONT SOFTWARE OR FROM
	OTHER DEALINGS IN THE FONT SOFTWARE.

=========================================================================================================
=========================================================================================================
=========================================================================================================

Contains Font Awesome brand icons
Licensed under the CC BY 4.0 license.
See: https://fontawesome.com/license/free

=========================================================================================================
=========================================================================================================
=========================================================================================================

Contains MegaApiClient licensed under the MIT license.

The MIT License (MIT)

Copyright (c) 2015 gpailler

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

=========================================================================================================
=========================================================================================================
=========================================================================================================

Contains Newtonsoft.JSON licensed under the MIT license.

The MIT License (MIT)

Copyright (c) 2007 James Newton-King

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in the
Software without restriction, including without limitation the rights to use, copy,
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
and to permit persons to whom the Software is furnished to do so, subject to the
following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

=========================================================================================================
=========================================================================================================
=========================================================================================================

Contains Fody licensed under the MIT license.

MIT License

Copyright (c) Simon Cropp

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.