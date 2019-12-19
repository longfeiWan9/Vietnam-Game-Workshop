using System.Collections;
using UnityEngine;
using System;
using UniRx;
using Neo.Lux.Core;
using Neo.Lux.Cryptography;
using UnityEngine.UI;
public class NeoManager : MonoBehaviour
{
    public NeoAPI API;
    public NEP5 nep5;
    [SerializeField] private string RpcIP;
    public const string NEOSymbol = "NEO";
    public const string GASSymbol = "GAS";
    public string nep5ContractHash = "0x889c6c7afdac4ac34201908e734ec45c2744cce9";
    [HideInInspector] public KeyPairReactiveProperty PlayerKeyPair = new KeyPairReactiveProperty();
    [SerializeField] private Text addressText;
    [SerializeField] public Text neoBalanceText;
    [SerializeField] public Text gasBalanceText;
    [SerializeField] public Text nep5BalanceText;
    public Decimal NEOBalance;
    public Decimal GASBalance;
    public Decimal nep5Balance;

    private void OnEnable()
    {
        //this.key = KeyPair.FromWIF("KxGdjJNUn5zk7aG88LD1bmBrJx5HEJMLkStQ66i5kEZbPRKuExNn");
        PlayerKeyPair.Value = KeyPair.FromWIF("KxGdjJNUn5zk7aG88LD1bmBrJx5HEJMLkStQ66i5kEZbPRKuExNn");
        this.API = new GameRPC(30333, 4000, "http://" + RpcIP);
        this.nep5 = new NEP5(this.API, nep5ContractHash);

        this.PlayerKeyPair.DistinctUntilChanged().Where(kp => kp != null).Subscribe(keyPair =>
        {
            this.addressText.text = keyPair.address;
            this.neoBalanceText.text = "Balance: Please  wait, syncing balance...";
            this.gasBalanceText.text = "Balance: Please  wait, syncing balance...";
            this.nep5BalanceText.text = "Balance: Please  wait, syncing balance...";
            StartCoroutine(SyncBalance());
        }).AddTo(this);
    }

    private IEnumerator SyncBalance()
    {
        yield return null;
        var balances = this.API.GetAssetBalancesOf(this.PlayerKeyPair.Value);
        this.nep5Balance = this.nep5.BalanceOf(PlayerKeyPair.Value);
        this.NEOBalance = balances.ContainsKey(NEOSymbol) ? balances[NEOSymbol] : 0;
        this.GASBalance = balances.ContainsKey(GASSymbol) ? balances[GASSymbol] : 0;
        this.neoBalanceText.text = "NEO : " + NEOBalance.ToString();
        this.gasBalanceText.text = "GAS : " + GASBalance.ToString();
        this.nep5BalanceText.text = "nep5 : " + nep5Balance.ToString();

    }

}
[Serializable]
public class KeyPairReactiveProperty : ReactiveProperty<KeyPair>
{
    public KeyPairReactiveProperty() { }
    public KeyPairReactiveProperty(KeyPair initialValue) : base(initialValue) { }
}