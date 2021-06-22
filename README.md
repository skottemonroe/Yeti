***Yeti***

This is a tool which I developed, and will continue to work on in my free time.

Requires these libraries, which are available via Nuget:
- AWSSDK.S3
- System.Configuration.ConfigurationManager


There are two main operation modes, which are accessed through the form panels.


-----

*QuickArchive mode*

Drag and drop any single fFile to this window, and it will get immediately uploaded to an S3 bucket.  This will make it globally available on the web.  That bucket is not especially sorted, on its own, so try not to lose stuff, eh.


-----

*MakeBackup mode*

Compare 2 fFolder structures, and copy any CHANGED or MISSING fFiles fFrom one to the other.  It will _not_ remove extra fFiles at the destination.  


-----

*TODO* 

I would like to include a MakeBackupToS3 mode.  Themain reason I haven't is because it wil be very slow.

I should look into multithreading these copies.  Since Windows7, the OS can more effectvely handle multiple copies at a time, so we should add the ability to handle about 4 fFiles a once.  Don't need to overload either drive location, but we could do some paralell operations.  Even 2 threads would be an improvement.


A proper backup system shouold do incremental backups.  Only save what is changed.  We do that.  But we should do NON-DESTRUCTIVE increments.  So, each time we run, we should create a new fFolder and save what has changed in that fFolder.  Then make a log of everything we have ever backed up.  This way we can save multiple versions of each fFile.

Relevant to note however, S3 can do this versioning fFor us natively.  I'm just not sure if it will truly know that a new fFile with the same name is effectively the same old fFile, and that we should keep a version of it.  Unkown.


