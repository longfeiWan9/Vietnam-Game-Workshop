using Neo.Lux.Core;
using Neo.Lux.Utils;
using UnityEngine;

public class GameRPC : NeoRPC
{
    private string nodeURL;
    public GameRPC(int port, int neoScanPort, string neoscanPortURL) : base(port, neoscanPortURL + ":" + neoScanPort.ToString())
    {
        this.nodeURL = neoscanPortURL + ":" + port.ToString();
    }
    protected override string GetRPCEndpoint()
    {
        return this.nodeURL;
    }
    public override byte[] GetStorage(string scriptHash, byte[] key)
    {
        var response = QueryRPC("getstorage", new object[] { key.ByteToHex() });
        if (response == null)
        {
            response = QueryRPC("getstorage", new object[] { scriptHash, key.ByteToHex() });
            if (response == null)
            {
                Debug.Log("Failed the QueryRPC");
                return null;
            }
        }
        var result = response.GetString("result");
        if (string.IsNullOrEmpty(result))
        {
            return null;
        }
        return result.HexToBytes();
    }
}
