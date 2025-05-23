A few notes for Geoffrey on what I would like reviewed/advised on, because I decided against regions in my scripts:

1) pretty much the entire WordInteractionManager.cs, specifically the TryLetter method, is a bit of a mess because of tweening. I would like to figure out a more elegant solution for "mirroring" tweens and generally reducing clutter.
2) scripts in the Assets/Scripts/Player, Assets/Scripts/Tiles, and InteractableFramework.cs deal with moving the player around, calculating traversal and managing the Approach/Use/Change mechanics. I want to get rid of the Approach mechanic to be able to "queue" action from afar, but unsure where to start.
