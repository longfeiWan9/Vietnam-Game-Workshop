using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.ComponentModel;
using System.Numerics;

public class nep5 : SmartContract
{
    
    [DisplayName("transfer")]
    public static event Action<byte[], byte[], BigInteger> Transferred;

    private static readonly byte[] Owner = "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y".ToScriptHash(); //Owner Address

    private const ulong factor = 1; //decided by Decimals()
    private const ulong total_amount = 100000000 * factor; //token amount

    public static object Main(string method, object[] args)
    {
        if (Runtime.Trigger == TriggerType.Verification)
        {
            return Runtime.CheckWitness(Owner);
        }
        else if (Runtime.Trigger == TriggerType.Application)
        {
            var callscript = ExecutionEngine.CallingScriptHash;

            if (method == "deploy") return Deploy();

            if (method == "balanceOf") return BalanceOf((byte[])args[0]);

            if (method == "decimals") return Decimals();

            if (method == "name") return Name();

            if (method == "symbol") return Symbol();

            if (method == "supportedStandards") return SupportedStandards();

            if (method == "totalSupply") return TotalSupply();

            if (method == "claimRewards") return ClaimRewards((byte[])args[0], (BigInteger)args[1], callscript);

            if (method == "transfer") return Transfer((byte[])args[0], (byte[])args[1], (BigInteger)args[2], callscript);
        }
        return false;
    }

    // initialization parameters, only once
    [DisplayName("deploy")]
    public static bool Deploy()
    {
        if (TotalSupply() != 0) return false;
        StorageMap contract = Storage.CurrentContext.CreateMap(nameof(contract));
        contract.Put("totalSupply", total_amount);
        StorageMap asset = Storage.CurrentContext.CreateMap(nameof(asset));
        asset.Put(Owner, total_amount);
        Transferred(null, Owner, total_amount);
        return true;
    }

    [DisplayName("balanceOf")]
    public static BigInteger BalanceOf(byte[] account)
    {
        if (account.Length != 20)
            throw new InvalidOperationException("The parameter account SHOULD be 20-byte addresses.");
        StorageMap asset = Storage.CurrentContext.CreateMap(nameof(asset));
        return asset.Get(account).AsBigInteger();
    }

    [DisplayName("decimals")]
    public static byte Decimals() => 0;

    private static bool IsPayable(byte[] to)
    {
        var c = Blockchain.GetContract(to);
        return c == null || c.IsPayable;
    }

    [DisplayName("name")]
    public static string Name() => "2019 Vietnam Worshop"; //name of the token

    [DisplayName("symbol")]
    public static string Symbol() => "VWT"; //symbol of the stoken

    [DisplayName("supportedStandards")]
    public static string[] SupportedStandards() => new string[] { "NEP-5", "NEP-7", "NEP-10" };

    [DisplayName("totalSupply")]
    public static BigInteger TotalSupply()
    {
        StorageMap contract = Storage.CurrentContext.CreateMap(nameof(contract));
        return contract.Get("totalSupply").AsBigInteger();
    }

    //Methods of actual execution
    private static bool Transfer(byte[] from, byte[] to, BigInteger amount, byte[] callscript)
    {
        //Check parameters
        if (from.Length != 20 || to.Length != 20)
            throw new InvalidOperationException("The parameters from and to SHOULD be 20-byte addresses.");
        if (amount <= 0)
            throw new InvalidOperationException("The parameter amount MUST be greater than 0.");
        if (!IsPayable(to))
            return false;
        if (!Runtime.CheckWitness(from) && from.AsBigInteger() != callscript.AsBigInteger())
            return false;
        StorageMap asset = Storage.CurrentContext.CreateMap(nameof(asset));
        var fromAmount = asset.Get(from).AsBigInteger();
        if (fromAmount < amount)
            return false;
        if (from == to)
            return true;

        //Reduce payer balances
        if (fromAmount == amount)
            asset.Delete(from);
        else
            asset.Put(from, fromAmount - amount);

        //Increase the payee balance
        var toAmount = asset.Get(to).AsBigInteger();
        asset.Put(to, toAmount + amount);

        Transferred(from, to, amount);
        return true;
    }


    /* *
        * winning game points
        * Unsafe, only for demo purpose
        * */
    [DisplayName("claimRewards")]
    public static bool ClaimRewards(byte[] to, BigInteger amount, byte[] callscript)
    {
        StorageMap asset = Storage.CurrentContext.CreateMap(nameof(asset));
        var fromAmount = asset.Get(Owner).AsBigInteger();
        if (fromAmount < amount)
            return false;

        //Reduce payer balances
        if (fromAmount == amount)
            asset.Delete(Owner);
        else
            asset.Put(Owner, fromAmount - amount);

        //Increase the payee balance
        var toAmount = asset.Get(to).AsBigInteger();
        asset.Put(to, toAmount + amount);

        Transferred(Owner, to, amount);
        return true;
    }
}
