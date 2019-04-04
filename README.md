# historymap-server
Server side for [historymap](https://github.com/alancameronwills/historymap), deployed to Azure functions.


## Set up for editing

Edit here (or with git/VSCode). Code is automatically deployed to Azure Functions server.

* Installations - see [setup for client](https://github.com/alancameronwills/historymap/blob/master/README.md#to-edit-and-test).

## Structure

See detailed [user guide for Azure Functions](). 

In brief:

* Incoming HTTP reqs are translated by proxies.json, which defines mappings from incoming URLs to:
   * client code and images, kept in blob storage
   * REST calls to server functions in this folder
   * redirection from root URLs to specifics - e.g. '/' to 'index.htm'
* Each server function occupies a folder. Each function folder contains:
   * a .json that defines the trigger (incoming HTML req, storage event, timer, ...) and bindings; 
   * .js or .csx code
* `Shared` folder contains common code, and is referenced by host.json

REST Server functions in order of interest:

* places - returns a list of places, given a zone (i.e. geo area) and other filters. Used by the main index.htm page. Currently each place includes the complete description text, so the returned output can be quite long. The `fields` parameter projects just the titles and ids, and is used by the place editor client to resolve links to other places.
* place - returns the details of one place. Used by editor client page edit.htm. It isn't filtered by geo zone etc. It appends a property giving the zone in which the place lies; used by the main page to discover the zone of a place when following a link.
* census, graves - return those table entries related to a given place.
* updateplace - Used by the editor client page.
* fetch - used to get and relay an external blog page, such as Glen Johnson's. The feature for mashing his articles directly into some pages cannot access his page directly because of CORS restrictions.
* s - Serves css, js, image files

Worker functions:

* Backup - nightly backup of the places table. We'll write a restore routine when/if we need it.
* PicFix - looks for large pictures and reduces them. Triggered by blob upload. Has got into a loop before.

Other apps:
* apps - serves the appshare app
* bunnies - serves the RovingBand app
* bunnyup - upload for the RovingBand app

