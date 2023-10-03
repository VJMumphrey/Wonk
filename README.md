## About
***Wonk is a security tool meant for competition purposes only. This is a work in progress and not meant to be ran in a professional setting.***

It reads the event logs among many things and monitors users and their actions.
If a user does a bad action their session is *"Wonked"*.
This was inspired by its predecessor, Bonk, written by Kevin Oubre.
It can be found here https://github.com/KevOub/bonk.
This is the Windows version hence Wonk.

Further details are in the documentation
```
____    __    ____   ______   .__   __.  __  ___ 
\   \  /  \  /   /  /  __  \  |  \ |  | |  |/  / 
 \   \/    \/   /  |  |  |  | |   \|  | |  '  /  
  \            /   |  |  |  | |  . `  | |    <   
   \    /\    /    |  `--'  | |  |\   | |  .  \  
    \__/  \__/      \______/  |__| \__| |__|\__\ 
                                                 
                                  _
                                 \  \
             ___________________  \  \
            |         |	        |  \  \
            |         |	        |   \  \
            |         |	        |    \  \
            |_________|_________|     \  \
            |         |	        |      \  \
            |	      |	        |        \\
            |	      |	        |        \__\
            |_________|_________|
```

## Installation
Install .net 7 x64 and compile with 

> dotnet build --configureation release