run the installer.

add C:\Program Files\MongoDB\Server\3.0\bin\ to USER path variables.

then in command line :

mongod --config C:\path\to\mongod.conf --install 



Start mongo service :
net start MongoDB

mongod --config C:\Users\Vincent\Documents\Projects\BrewMonitor\DB\mongod.conf --install  

starts the command line
>mongo C:\Users\Vincent\Documents\Projects\BrewMonitor\DB\CreateDBScript.js