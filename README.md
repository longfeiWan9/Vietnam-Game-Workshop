<div align="center">  
<img src="./images/neo_logo.png" alt="NEO-logo" height="100">
</div>



# Vietnam-Game-Workshop

During this workshop, we will walk you through the whole process of developing a simple blockchain game on Neo. And we want to bring a playful and meaningful session for developers to get their hands on setting up local blockchain, building a simple but playful game as well as integrating it with Neo blockchain. 

The objective of this workshop is to using [Unity survival-shooter-tutorial](https://learn.unity.com/project/survival-shooter-tutorial) as a sample to create a game with Neo Blockchain to earn your game points token. This is a easy example for beginner to learn the basic steps to create a dApp or blockchain game.  

This workshop is based on a community article [Making a game with NEO + Unity](https://medium.com/@tbriley/making-a-game-with-neo-unity-part-1-4099ec7d7a82)

## Pre-setup

+ [Git](https://git-scm.com/downloads)

+ [Docker](https://www.docker.com/products/docker-desktop)

+ [Unity](https://store.unity.com/download-nuo)

+ [.Net Core 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1)& [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

+ **neo-local**

  + Clone neo-local to a folder

    ```
    git clone https://github.com/CityOfZion/neo-local.git
    ```

  + Start neo-local in docker

    > Highly Recommended to pull all images in advance
    
    ```
    cd neo-local
    docker-compose up -d --build --remove-orphans
    ```

+ [neo-python](https://github.com/CityOfZion/neo-python)

+ [VS code](https://code.visualstudio.com/download)

+ [Visual Studio](https://visualstudio.microsoft.com/downloads/) : please add Unity support for debugging purpose

## Tools we are using

+ **Docker & neo-local** - Setting up local blockchain
+ **VS code** - Develop smart contract
+ **Unity & Visual Studio** - Develop game
+ **Neo-lux** - Neo C# SDK for Unity
+ **[Online Data Convertor](https://peterlinx.github.io/DataTransformationTools/)**  - Convert various data return from Blockchain

Now, if you want to get your hands dirty, please click [HERE](./DevelopmentProcess.md)

