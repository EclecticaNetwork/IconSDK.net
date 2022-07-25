using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Numerics;
using NUnit.Framework;
using Newtonsoft;
using Newtonsoft.Json;

namespace IconSDK.Tests
{
    using RPCs;
    using Blockchain;
    using Types;
    using Extensions;
    using Crypto;

    public class TestRPC
    {
        [Test]
        public async Task Test_GetBalance1()
        {
            var getBalance = new GetBalance(Consts.ApiUrl.TestNet);
            var balance = await getBalance.Invoke("hx0001000000000000000000000000000000000000");
        }

        [Test]
        public async Task Test_GetBalance2()
        {
            var getBalance = GetBalance.Create(Consts.ApiUrl.TestNet);
            var balance = await getBalance("hx0000000001000000000000000000000000000000");
        }

       
      
        [Test]
        public async Task Test_GetLastBlock()
        {
            var getLastBlock = GetLastBlock.Create(Consts.ApiUrl.TestNet);
            var lastBlock = await getLastBlock();

            var getBlockByHeight = GetBlockByHeight.Create(Consts.ApiUrl.TestNet);
            var blockByHeight = await getBlockByHeight(lastBlock.Height.Value);

            Assert.AreEqual(lastBlock.Height, blockByHeight.Height);
            Assert.AreEqual(lastBlock.Hash, blockByHeight.Hash);

            var getBlockByHash = GetBlockByHash.Create(Consts.ApiUrl.TestNet);
            var blockByHash = await getBlockByHash(lastBlock.Hash);

            Assert.AreEqual(lastBlock.Height, blockByHash.Height);
            Assert.AreEqual(lastBlock.Hash, blockByHash.Hash);
        }

        [Test]
        public async Task Test_GetTotalSupply()
        {
            var getTotalSupply = new GetTotalSupply(Consts.ApiUrl.MainNet);
            var totalSupply = await getTotalSupply.Invoke();

            Assert.AreEqual(totalSupply.ToHex0x(), totalSupply.ToHex0x());
        }

        [Test]
        public async Task Test_GetScoreApi()
        {
            var getScoreApi = new GetScoreApi(Consts.ApiUrl.TestNet);
            var scoreApi = await getScoreApi.Invoke("cx0000000000000000000000000000000000000001");

            Assert.Greater(scoreApi.Length, 0);
        }

        [Test]
        public async Task Test_GetTransactionResult()
        {
            // Link : https://trackerdev.icon.foundation/transaction/0xef5c1fca6bfe5a7f818930d7dd9b8c81c0dcd604064caf8cba8d6c51a7bdb5c6
            var getTransactionResult = GetTransactionResult.Create(Consts.ApiUrl.TestNet);
            var transactionResult = await getTransactionResult("0xef5c1fca6bfe5a7f818930d7dd9b8c81c0dcd604064caf8cba8d6c51a7bdb5c6");

            Assert.AreEqual(transactionResult.BlockHeight.ToHex0x(), "0x9dcaae");
            Assert.AreEqual(transactionResult.BlockHash.ToHex0x(), "0xa23ecce791583fdb868a66063080e85df125113954b7837e49e633d0921a4ee7");
            Assert.AreEqual(transactionResult.TxHash, "0xef5c1fca6bfe5a7f818930d7dd9b8c81c0dcd604064caf8cba8d6c51a7bdb5c6");
            Assert.AreEqual(transactionResult.TxIndex.ToHex0x(), "0x0");
            Assert.AreEqual(transactionResult.LogsBloom, "0x00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002000000000000000000000000000000000000000000000008000000010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000008000000000000000000000000000000000000080000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000004000000000000000000000000000000000000000");
            Assert.AreEqual(transactionResult.StepPrice.ToHex0x(), "0x0");
            Assert.AreEqual(transactionResult.StepUsed.ToHex0x(), "0x0");
            Assert.AreEqual(transactionResult.CumulativeStepUsed.ToHex0x(), "0x0");
            Assert.AreEqual(transactionResult.ScoreAddress, null);
            Assert.AreEqual(transactionResult.To.ToString(), "hx1000000000000000000000000000000000000000");
            Assert.AreEqual(transactionResult.Status, true);
            Assert.AreEqual(transactionResult.Failure, null);
            Assert.AreEqual(transactionResult.EventLogs[0].ScoreAddress, "cx0000000000000000000000000000000000000000");
            Assert.AreEqual(transactionResult.EventLogs[0].Indexed[0], "ICXIssued(int,int,int,int)");
            Assert.AreEqual(transactionResult.EventLogs[0].Data[0], "0x0");
            Assert.AreEqual(transactionResult.EventLogs[0].Data[1], "0x0");
        }

         
        [Test]
        public async Task Test_Call()
        {
            var getScoreApi = new GetScoreApi(Consts.ApiUrl.TestNet);
            var scoreApi = await getScoreApi.Invoke("cx0000000000000000000000000000000000000001");


            var privateKey = PrivateKey.Random();
            var address = Addresser.Create(privateKey);

            var call = new Call(Consts.ApiUrl.TestNet);
            var result = await call.Invoke(
                address,
                "cx0000000000000000000000000000000000000001",
                "getVersion"
              
            );

            // 0x0
            
            var call0 = new Call<string>(Consts.ApiUrl.TestNet);
            var result0 = await call0.Invoke(
                address,
                "cx0000000000000000000000000000000000000001",
                "getVersion"
              
             );

            // false
            Console.WriteLine(result0);

            var call1 = new Call<string>(Consts.ApiUrl.TestNet);
            var result1 = await call1.Invoke(
                address,
                "cx0000000000000000000000000000000000000001",
                "getVersion"
               
             );

            // false
            Console.WriteLine(result1);

            var call2 = new Call<BigInteger>(Consts.ApiUrl.TestNet);
            var result2 = await call2.Invoke(
                address,
                "cx0000000000000000000000000000000000000001",
                "getStepPrice"
            );

            Console.WriteLine(result2);

            var call3 = new Call<string>(Consts.ApiUrl.TestNet);
            var result3 = await call3.Invoke(
                address,
                "cx0000000000000000000000000000000000000001",
                "getRevision"
            );

            Console.WriteLine(result3);
            Console.WriteLine(result3);

            var call4 = new Call<Dictionary<string, BigInteger>>(Consts.ApiUrl.TestNet);
            var result4 = await call4.Invoke(
                address,
                "cx0000000000000000000000000000000000000001",
                "getStepCosts"
            );

            Console.WriteLine(JsonConvert.SerializeObject(result4));
        }

        [Test]
        public void Test_RPCMethodNotFoundException()
        {
            var getBalance = new GetBalance(Consts.ApiUrl.TestNet);

            GetBalanceRequestMessage requestMessage = new GetBalanceRequestMessage("hx0000000000000000000000000000000000000000");
            FieldInfo methodFieldInfo = typeof(GetBalanceRequestMessage).GetField("Method");
            methodFieldInfo.SetValue(requestMessage, "icx_GetBalance");  // 'icx_getBalance' is correct

            Assert.ThrowsAsync(typeof(RPCMethodNotFoundException), async () => await getBalance.Invoke(requestMessage));
        }

        [Test]
        public void Test_RPCInvalidParamsException()
        {
            var getBalance = new GetBalance(Consts.ApiUrl.TestNet);

            GetBalanceRequestMessage requestMessage = new GetBalanceRequestMessage("hx0000000000000000000000000000000000000000");
            FieldInfo addressFieldInfo = requestMessage.Parameters.GetType().GetField("Address");
            addressFieldInfo.SetValue(requestMessage.Parameters, "hxz000000000000000000000000000000000000000");  // 'hx0000000000000000000000000000000000000000' is correct

            Assert.ThrowsAsync(typeof(RPCInvalidParamsException), async () => await getBalance.Invoke(requestMessage));
        }

        
        class IsDeployerRequestParam
        {
            public Address Address;
        }

        class GetRevisionResponseParam
        {
            public BigInteger Code = 0;
            public string Name = string.Empty;
        }
    }
}