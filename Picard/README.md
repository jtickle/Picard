Picard
======

Never manually update mats in Inara.cz again.  This simple tool will analyze
your Elite: Dangerous log files, log into your Inara.cz account, and update
your Engineering Materials inventory.

It's open source so you can see that I'm not stealing your credentials.  But,
if you trust my compiler, you can just download and run picard.exe.

Because log files are a fairly new feature of Elite: Dangerous, they may not
contain your entire history of materials.  So, Picard assumes that you have
updated Inara to your current Materials inventory and that you have not
collected or lost any materials since then.  It will then write your Inara
credentials, the current time, and your current Inara materials list to
a data file for later use.  Don't lose this file or you will have to re-
synchronize with Inara manually!

Next time you run Picard, it will look for any new log files since your last
run.  If it finds any, it will take the inventory changes in those files and
apply them to your Inara inventory.

So you just need to run Picard every now and then to update things.  If you
can remember or set up some automatic thing, just run it after you quit
playing Elite.

Because this program is licensed under the GPL-3.0, it is without warranty.
That means that if it obliterates all your save games, corrupts your Inara
account, ruins your good clothes, and burns your house down, it's your fault
for downloading and running it!  Live on the edge.


Quick Start
-----------

1. Manually enter your Elite: Dangerous materials inventory into Inara.cz
2. Run Picard and enter your Inara.cz username and password
3. Verify that the data shown in Picard matches up with the data shown on your
   Inara account

Now go play some Elite: Dangerous; gather mats and buy engineering things!
Every time you're done playing, or before every time you start...

1. Run Picard
2. Verify that you are satisfied with the changes that will be reported
3. Mash 'Looks Good'


State File Specification
------------------------

Picard persistent state is stored in %APPDATA%\Local\TickleSoft\picard.state.
It is in JSON format.

 * CmdrName:  Your commander name on Inara for future reference
 * InaraU:    Your Inara.cz username
 * InaraP:    Your Inara.cz password.  They run insecure HTTP so I don't feel
              bad about leaving it plaintext.
 * History:   Array of changes since the original manual synchronization
   * Key:     DateTime of the synchronization event
   * Value:   Materials Array
     * Key:   Inara Material Name
	 * Value: Material Count Change since Last Sync

In the "update" stage, all of your history is read and added together, and then
compared with current values on Inara.  If they don't match up, you will get
an error and have to resynchronize because that means something has changed
on Inara without Picard's knowledge.


The Plan
--------
ThePlan
 1. Does a dat file exist?
    No - gosub AuthPrompt
 2. Does dat file contain 'LastInaraSnapshotTimestamp'?
    No - gosub InaraInit
 3. goto InaraUpdate

AuthPrompt
 1. Show Welcome Message
 2. Prompt for Inara user/pass
 3. Log into Inara
 4. Login problems?
    No - goto 7
 5. Show Error
 6. goto 1
 7. Write user credentials to dat file
 8. return

InaraInit
 1. Scrape data from inara.cz/cmdr-cargo/
    EXCEPTION POSSIBLE - network bullshit
 2. Show data from inara.cz/cmdr-cargo/
 3. Refresh or Looks Good?
    Refresh - goto 1
 4. Write Inara data and LastInaraSnapshotTimestamp to dat file
    EXCEPTION POSSIBLE - filesystem bullshit
 5. Show a friendly message
 5. exit to OS

InaraUpdate
 1. Scrape data from inara.cz/cmdr-cargo/
    EXCEPTION POSSIBLE - network bullshit
 2. Load data from dat file
    EXCEPTION POSSIBLE - filesystem bullshit
 3. Let data, diffs, update be gosub ParseLogsAndUpdateData(dat file data)
 4. Show data, diffs, update
 5. Refresh or Looks Good?
    Refresh - goto 1
 6. POST update to inara.cz/cmdr-cargo/
    EXCEPTION POSSIBLE - network bullshit
 7. Scrape data from inara.cz/cmdr-cargo/
    EXCEPTION POSSIBLE - network bullshit
 8. Verify data - any good?
    No - alert user "Something went horribly wrong" and exit to OS
 9. Update dat file with new Inara data, LastInaraSnapshotTimestamp set to now
10. alert user "Looks like all is well" and exit to OS

ParseLogsAndUpdateData
 1. Let LastLog be LastInaraSnapshotTimestamp from dat file
 2. Is there a file newer than LastLog?
    No - goto 9
 2. Get first logfile newer than LastLog
    EXCEPTION POSSIBLE - filesystem bullshit
 3. Get next line of logfile
 4. EngineerCraft?
    Yes - decrement the mat
    EXCEPTION POSSIBLE - negative mats means something has fallen out of sync
 5. MaterialCollected?
    Yes - increment the mat
 6. EOF?
    Yes - goto 2
 7. Let LastLog get the mtime of the open logfile
 8. goto 2
 9. Did we read any files?
    No - alert user "No new logfiles since last run" and exit to OS
 9. Did we read any Material events?
    No - alert user "Logfiles were found but not changes to mats" and exit to OS
10. return
    - data from dat
    - the changes that were made
    - the update that will be pushed to Inara

Useful Resources
----------------
(Parsing HTML with C#)[https://www.codeproject.com/tips/804660/how-to-parse-html-using-csharp]