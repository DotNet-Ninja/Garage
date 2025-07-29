# DotNet Ninja's Garage
A Web Browser Start Page Application

> Note: This is a very early alpha version of the application. In general, it works, but there are some issues and improvements that need to be made including testing, a couple of now bug fixes, and more. You can certainly use it as it is, I am currently.


The Garage is intended as a customizable start page for web browsers (i.e. a page that is your default page when you launch a new browser). It allows you to add your favorite links in an organized fashion.  The application is built and published as a docker image making it super simple to run your own instance of the Garage on any machine with docker desktop installed (i.e. pretty much any developer today).


![Screen Shot](https://dotnetninja.net/wp-content/uploads/2025/07/GarageScreenshot.png)

## Design Requirements
I wanted a simple application to serve as a place to keep bookmarks.
I did not want it to require running a database server or any other external service, so the links are stored as json formatted text files. 

I also wanted the application to be easy to deploy and run, so I chose docker for this purpose and have included a docker-compose.yml file that should only require one small edit (your local file path to store the files) to run on any machine.

I also wanted to enable the scenario where the application could be run on a home server and support a separate site for each user in the home.

## How To Use
The data (links) are stored and displayed in a heirarchical structure using the following paradigm: Sites -> Pages -> Groups -> Bookmarks

- Sites: A collection of pages intended to be used by a single user as their own personal start "site".  (When running locally, you would likely only have one site, although you could have multiple if your wished)

- Pages: A Page represents an actual web page like the one in the screenshot above.  Each page gets its own link in the top navigation bar and can contain many groups of links.

- Groups: A Group is a collection of links simply for organizational purposes on a page.  This allows you to keep like links together on the page and make them quick and easy to find.

- Bookmarks: A Bookmark represents a link that has been added to the user's personal "site".  It can be used as a shortcut to quickly access a specific link.

The gear icon on the main navigation links to the admin panel where you can add, edit, and delete sites, pages, groups, and bookmarks by navigating up and down the heirarchy.

> Note: There is no authentication or authorization in this application.  It is intended to be used soley by a single user on a single machine, or on a home server within your network.  If you do use the site in a manner where multiple machines can access it, you will want to be sure that your links do not directly go to unprotected resources and that you trust the other users on your network not to "abuse" your site and make modifications to your links.

## Setup
Running the site requires that you have docker installed and have at least a minimal knowledge of how to use it.  I have made it as easy as possible to deploy it by using the provided docker-compose.yml file.

To get started, create a local directory for your data.  This directory will be mapped into the container as a volume so that your site can persist across restarts/upgrades etc..

Copy the docker-compose.yml file into a directory on your machine and edit the last line `- D:\Docker\data\Garage:/app/wwwroot/data` replacing `D:\Docker\data\Garage` with the path to your data directory.

```
version: "3"
services:
  web:
    image: "dotnetninjax/garage:latest"
    ports:
      - 6502:8080
    restart: always
    volumes:
      - <YOUR-DIRECTORY-PATH>:/app/wwwroot/data
```

From the directory containing the docker-compose.yml file, run the following command:

```
    docker compose up -d
```

Once the container image is up and running you can access the site by navigating to http://localhost:6502 in your web browser.  A "Default Site" will be created automatically and you can edit to your liking, or create a new site and start from scratch adding your own links.

## Keeping Up To Date

Each time the project is built on GitHub, a new Docker image is pushed to the Docker Hub.  I am tagging each build/image with the current semantic version of that build as well as "latest".  The default docker-compose.yml file will pull the latest tag when you run `docker-compose up -d`.  If you choose to, you can target a specific version by changing the image tag in the docker-compose.yml.  You can find the latest image tags on the [Docker Hub page for this project](https://hub.docker.com/r/dotnetninjax/garage/tags).

Upgrading, assuming you are using the `latest` tag is as simple as running the following from the directory containing the docker-compose.yml file:

```
    docker compose down # stop the application
    docker rmi dotnetninjax/garage:latest # remove the old image
    docker-compose up -d # start the application    
```
> Note: You could put this into a script file in your docker compose directory along side the `docker-compose.yml` file.  This would allow you simply run the script to upgrade.

## Roadmap

- **Bug Fixes:** I know of a couple bugs already.  I'll be writing those up as issues and fixing them as time allows.
- **Testing:** There are no tests!  This will be a priority for me in the future.
The project was put together very quickly based on another project I had previously done, just haven't gotten there yet.
- **Code Clean up/Refactoring:** I'll be working on cleaning up and refactoring the codebase.  Lots of room for improvement here
- **Themes:** I have plans to add some basic themes using various existing bootstrap themes.
- **Data Syncing:** I am considering a couple of ideas on how I might implement syncing data 
