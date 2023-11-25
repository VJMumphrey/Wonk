Code Structure and Architecture
===============================

All helper classes/Enums and such should be in *Utils.cs*. This helps with code cleanliness down the road. This includes the list of ETW Providers and Events to add to a session.

Banner printing and all its related work should be in *Banner.cs*.

All of the main "Wonking" should be kept in *Program.cs* for now. This can be branched out later if things get to complex.