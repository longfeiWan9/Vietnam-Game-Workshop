using Neo.Lux.Core;
using Neo.Lux.Cryptography;
using Neo.Lux.Utils;
using System;
using System.Collections;
using System.Numerics;
using UniRx;
using UnityEngine;

public class Nep5Manager : MonoBehaviour
{
    [SerializeField] private NeoManager neoManager;
    [SerializeField] private CompleteProject.PlayerHealth playerHealth;
    public string ContractHash = "0x889c6c7afdac4ac34201908e734ec45c2744cce9";

    [SerializeField] private int rewardThreshold = 0;

    private bool isGameOver;

    private void Update()
    {
        Debug.Log("updating result .....");
        Debug.Log("Current Health: " + playerHealth.currentHealth);

        if (playerHealth.currentHealth <= 0 && !isGameOver)
        {
            Debug.Log("Player is dead .....");
            StartCoroutine(OnGameOver());
            isGameOver = true;
        }
        else if (playerHealth.currentHealth > 0 && isGameOver)
        {
            isGameOver = false;
        }
    }

    private IEnumerator OnGameOver()
    {
        Debug.Log("Game Over .....");
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;

        if (CompleteProject.ScoreManager.score >= rewardThreshold)
        {
            Debug.Log("You should receive " + CompleteProject.ScoreManager.score + " nep5 token.");
            StartCoroutine(TryClaimRewards(CompleteProject.ScoreManager.score));
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private IEnumerator TryClaimRewards(int amount)
    {
        yield return null;

        try
        {
            Debug.Log("Start claim NEP5 token reward");
            //var tx = neoManager.API.SendAsset(neoManager.MasterKeyPair, neoManager.PlayerKeyPair.Value.address, NEOManager.AssetSymbol, (decimal)amount);
            UInt160 scScriptHash = new UInt160(NeoAPI.GetScriptHashFromString(this.ContractHash));
            byte[] addressBytes = LuxUtils.GetScriptHashFromAddress(neoManager.PlayerKeyPair.Value.address);
            BigInteger nepAmt = new BigInteger(amount);
            var tx = neoManager.API.CallContract(neoManager.PlayerKeyPair.Value, scScriptHash, "claimRewards", new object[] { addressBytes, nepAmt });

            if (tx == null)
            {
                Debug.LogError("Null Transaction returned");
                Time.timeScale = 1;
            }
            else
            {
                Debug.Log(tx);
                Observable.FromCoroutine(SyncBalance).Subscribe().AddTo(this);
            }
        }
        catch (NullReferenceException ee)
        {
            Debug.LogError("There was a problem..." + ee);
            Time.timeScale = 1;
        }
        catch (Exception ee)
        {
            Debug.LogError("There was a problem..." + ee);
            Time.timeScale = 1;
        }
    }

    private IEnumerator SyncBalance()
    {
        yield return null;

        try
        {
            var balances = neoManager.API.GetAssetBalancesOf(neoManager.PlayerKeyPair.Value);
            neoManager.NEOBalance = balances.ContainsKey(NeoManager.NEOSymbol) ? balances[NeoManager.NEOSymbol] : 0;
            neoManager.GASBalance = balances.ContainsKey(NeoManager.GASSymbol) ? balances[NeoManager.GASSymbol] : 0;
            neoManager.nep5Balance = neoManager.nep5.BalanceOf(neoManager.PlayerKeyPair.Value);
            neoManager.neoBalanceText.text = "NEO : " + neoManager.NEOBalance.ToString();
            neoManager.gasBalanceText.text = "GAS : " + neoManager.GASBalance.ToString();
            neoManager.nep5BalanceText.text = "GPT : " + neoManager.nep5Balance.ToString();
            Debug.Log("Balance synced!");
            Time.timeScale = 1;
        }
        catch (NullReferenceException exception)
        {
            Debug.LogWarning(exception);
            Time.timeScale = 1;
        }
    }
}