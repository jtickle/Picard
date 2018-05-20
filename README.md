# Picard

This was a tool for updating Inara.cz with materials and data from your Elite Dangerous logfiles.

**Picard no longer works and will not be fixed.  Thank you for your interest!**  Inara now allows you to directly
upload your commander journal.  The next section covers how to do that, or Read "Story of This Project" below
if you want to know more.

# How to upload your game journals to Inara.cz

Log into Inara.cz and go to the [Cargo Screen](https://inara.cz/cmdr-cargo).  On the right side of the page
directly above "Favorite Blueprints", click "Import Cargo (Journal)."

The files you want to upload are in
C:\Users\YourName\Saved Games\Frontier Developments\Elite Dangerous\.  If you play Elite Dangerous in a language
other than English, some of these directory names will be localised to your language.

# Alternative

If you don't want to use Inara.cz, or if you just don't like The Cloud, consider trying
[EDEngineer](https://github.com/msarilar/EDEngineer).  It is an active project with a beautiful user interface
and integrates with the game nicely.

# Story of This Project

Me and my buddy were annoyed with having to manually update Inara with our materials.  For a long time, we talked
of how silly it was, and how easy it would be to automatically update Inara if we could just find out how many
materials a user has from the game itself.

We figured out that the Commander Journal files in Saved Games\Frontier Developments\Elite Dangerous did not
contain a count of materials, but that it would log every event in which you gained or lost a material.  That means
we could update automatically after an initial manual synchronization.  We figured it could be done in a weekend.

Two months later, I considered it "done" and put it out there for others to use.  Immediately, we were notified of
all sorts of bugs, and I was reminded that I am a terrible American who assumes that everyone speaks English.  I
enjoyed fixing these bugs and making new releases.

Picard worked well for about 100 users between the months of November, 2016 until March, 2017.
Outside professional work, it was my most successful software project to date.

Then, a major update was released for Elite Dangerous that changed how materials were logged.
Around the same time, inara.cz released an update that would allow you to import your log files directly.
Around the same time, I got busy with other things.  I also kind of became bored with Elite: Dangerous.
I love the explosion of space exploration games going on right now... but every single one of them has a
serious problem with being fun over the long term.

I named it "Picard" because it reads your "Captain's Logs."  After release, I realized that it doesn't make much sense
for Picard to speak to Inara.  While I am quite sure that Captain Picard would be as courteous to Inara as anyone else,
he doesn't seem to be the type to engage her services.
