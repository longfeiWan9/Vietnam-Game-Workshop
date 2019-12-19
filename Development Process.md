# Development Process

### Step 1: Setting Up Neo PrivateNet (neo-local)

> Recommend to setup neo-local

1. Start neo-local in Docker

   ```
   cd neo-local
   docker-compose up -d --build --remove-orphans
   ```

2. Blockchain Explorer: visit with [localhost:4000](http://192.168.99.100:4000/) or http://192.168.99.100:4000/

3. Using neo-python node

   ```
   docker exec -it neo-python np-prompt -p -v
   
   neo> help
   ```

   Learning how to use neo-python, please refer to [NEO-Python smart](https://github.com/neo-ngd/NEO-Tutorial/tree/master/neo_docs_neopython_tutorial)

   - Wallet：name: `neo-privnet.wallet` , password: `coz`

     ```
     neo> wallet open neo-privnet.wallet
     [password]> coz
     Opened wallet at neo-privnet.wallet
     
     neo> wallet send neo "wallet address" 10000
     ```

     

### Step 2: Smart Contract Development

> Recommend to use Neo Blockchain Toolkit
>
> Please see the [Neo Blockchain Toolkit for .NET Quickstart](https://github.com/neo-project/neo-blockchain-toolkit/blob/master/quickstart.md) for instructions on setting up the tools and creating your Neo smart contract.

1. Install Neo Blockchain Toolkit - **VS code extension**

   Search `Neo Blockchain Toolkit` in **VS code extension market place**, and the install it.  

2. Open a terminal window and execute the following commands to install the additional compiler and contract template:

   ```
   dotnet tool install -g Neo.neon-de
   dotnet new -i Neo.Contract.Templates
   ```

3. Create smart contract project

   - Create an empty folder for your smart contract project, then open this folder in VS code

   - Run the following command to create a smart contract template in VS Code terminal

     ```
     dotnet new neo-contract -de
     ```

4. Develop & debug your smart contract

   + Write your smart contract, then publish it

     ```
     dotnet publish
     
     cd .\bin\Debug\netstandard2.0\publish\
     neon nep5.dll --compatible
     ```

   + Debug your contract

     First, add a `launch.json` for debugging Neo contract.

     ![debugConfig](C:\Users\Surface\Documents\NEO\code\longfei\Vietnam-Game-Workshop\images\debugConfig.png)

     Then you can debug your contract like debugging any other program. 

5. Deploy and Test smart contract

   + Copy the `nep5.avm` files to `./neo-local/smart-contracts` folders so that neo-python can access it. 

   + Start neo-python and deploy your contract with the following command

     > need to open a wallet in neo-python node to pay the deployment fee

     ```
     $ docker exec -it neo-python np-prompt -p -v 
     
     neo> wallet open neo-privnet.wallet
     [password]> coz
     Opened wallet at neo-privnet.wallet
     
     neo> sc deploy help
     neo> sc load_run /smart-contracts/nep5.avm True False False 0710 05 name [] 
     
     neo> sc deploy /smart-contracts/nep5.avm True False False 0710 05 --fee=1 
     ```

   For more information about parameter and return type, please refer to [Smart Contract Parameters and Return Values](https://docs.neo.org/docs/en-us/sc/deploy/Parameter.html)

6. Invoke Smart Contract

   After the nep5 contract is deploy success on blockchain. We should initialized it which will send all token to owner account.  Required open your wallet first.

   ```
   neo> sc invoke 'contract ScriptHash' deploy []
   
   neo> sc invoke 'contract ScriptHash' balanceOf ['Address']
   ```

   

### Step 3: Create Game using Unity

Now we have our Neo Private Network running, and a nep5 token deployed. It is time to develop the game. :)

1. ##### Download and Open the [UnityDemo](./UnityDemo) Project in your Unity.

2. ##### Download and Import [neo-lux unity package](https://github.com/CityOfZion/neo-lux/raw/master/Unity/Neo.Lux.0.7.5.unitypackage)(`./neo-lux-master/Unity`) into your project;

   ![unity-import](C:\Users\Surface\Documents\NEO\code\longfei\Vietnam-Game-Workshop\images\unity-import.png)![unity-neolux](C:\Users\Surface\Documents\NEO\code\longfei\Vietnam-Game-Workshop\images\unity-neolux.png)

3. ##### Creating Script (GameRPC) to connect to Neo privateNet.

   Right-click under `Assets -> Scripts` and select `Create -> C# Script`. Name it GameRPC which will be used to config the connection with Neo Node via RPC request.  You copy the code from [GameRPC File](./assets/UnityScripts/GameRPC.cs)

4. ##### Creating Script (NeoManager) to query token balance of your wallet on Neo PrivateNet

   Create [NeoManager](./assets/UnityScripts/NeoManager.cs) following the same process as previous step. NeoManager is used to actually connect to Neo code  and query token balance of your own wallet.

5. ##### Add UI component to display wallet and tokens

   + Create text UI to display wallet info on Canvas by right-click canvas board and choose `UI->canvas, then UI->text`

   + Create Game Object call NeoManager by right-click canvas board and choose `Create Empty`

   + Then wire the NeoManager scipt and text UI to it by draging them to the corresponding fields.

     ![unity-1](C:\Users\Surface\Documents\NEO\code\longfei\Vietnam-Game-Workshop\images\unity-1.png)

6. ##### Creating Script to earn nep5 token based on your score and adding UI component as well.

   This step is similar as 4th and 5th step. First create a script call Nep5Manager refering to [Nep5Manager.cs](./assets/UnityScripts/Nep5Manager.cs). Then create a game object for it and wire up the NeoManager and PlayerHealth by dragging and dropping the references in the scene.

7. ##### Now let's start the game, play it and see how it goes.