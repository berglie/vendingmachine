using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vendee.VendingMachine.Exceptions;
using Vendee.VendingMachine.Models;
using Vendee.VendingMachine.Services;
using Vendee.VendingMachine.SmsSystem;

namespace Vendee.VendingMachine.UnitTests;

[TestClass]
public class VendingMachineUnitTests
{
    [TestMethod]
    public void InsertMoney_UpdatesBalance()
    {
        var insertAmount = 42;
        var expected = 42;

        var paymentService = new PaymentService();
        var vendingMachine = new Models.VendingMachine(null, paymentService, null, null, null);

        var balance =  vendingMachine.InsertMoney(insertAmount);

        Assert.AreEqual(expected, balance, "Money not added to balance correctly");
        Assert.AreEqual(expected, vendingMachine.Balance, "Money not added to balance correctly");
    }

    [TestMethod]
    public void RefundMoney_UpdatesBalance()
    {
        var paymentService = new PaymentService();
        var insertAmount = 42;
        var expected = 0;
        var vendingMachine = new Models.VendingMachine(null, paymentService, null, null, null);

        vendingMachine.InsertMoney(insertAmount);
        var returnedMoney = vendingMachine.RefundMoney();

        Assert.AreEqual(insertAmount, returnedMoney, "Money refund not the same amount as money inserted");
        Assert.AreEqual(expected, vendingMachine.Balance, "Money not deducted from balance correctly");
    }

    [TestMethod]
    public void OrderItem_ItemInStockAndSufficientFunds_DispensesItem()
    {
        var price = 42;
        var insertAmount = 50;
        var expectedDispensedSoda = Soda.CreateDefault("Coca Cola", "The Coca-Cola Company", price);

        var inventory = new Inventory();
        var paymentService = new PaymentService();
        var dispenseService = new DispenseService(inventory, paymentService);
        var vendingMachine = new Models.VendingMachine(inventory, paymentService, dispenseService, null, null);

        vendingMachine.InsertMoney(insertAmount);
        inventory.Add(expectedDispensedSoda);

        var dispensedItem = vendingMachine.DispenseItem(expectedDispensedSoda);

        Assert.AreEqual(expectedDispensedSoda, dispensedItem, "Soda dispensed is not the same as the requested soda");
    }

    [TestMethod]
    public void OrderItem_ItemOutOfStockAndSufficientFunds_ShouldThrowException()
    {
        var price = 42;
        var insertAmount = 42;

        var expectedDispensedSoda = Soda.CreateDefault("Coca Cola", "The Coca-Cola Company", price);
        var inventory = new Inventory();
        var paymentService = new PaymentService();
        var dispenseService = new DispenseService(inventory, paymentService);
        var vendingMachine = new Models.VendingMachine(inventory, paymentService, dispenseService, null, null);

        vendingMachine.InsertMoney(insertAmount);
        inventory.Add(expectedDispensedSoda, 0);

        Assert.ThrowsException<OutOfStockException>(() => vendingMachine.DispenseItem(expectedDispensedSoda));
    }

    [TestMethod]
    public void OrderItem_ItemInStockAndInsufficientFunds_ShouldThrowException()
    {
        var price = 42;
        var insertAmount = 24;

        var expectedDispensedSoda = Soda.CreateDefault("Coca Cola", "The Coca-Cola Company", price);
        var inventory = new Inventory();
        var paymentService = new PaymentService();
        var dispenseService = new DispenseService(inventory, paymentService);
        var vendingMachine = new Models.VendingMachine(inventory, paymentService, dispenseService, null, null);

        vendingMachine.InsertMoney(insertAmount);
        inventory.Add(expectedDispensedSoda);

        Assert.ThrowsException<InsufficientFundsException>(() => vendingMachine.DispenseItem(expectedDispensedSoda));
    }

    [TestMethod]
    public void SmsOrder_CodeWordValidAndProductIdInvalid_ShouldThrowException()
    {
        var codeWord = "VENDEE";
        var productId = "Coca Cocalium";

        using var smsService = new SmsService("@tcp://*:5556");
        using var smsServer = new MessageServer(">tcp://localhost:5556");
        var inventory = new Inventory();
        var vendingMachine = new Models.VendingMachine(inventory, null, null, null, smsService);

        smsServer.SendMessage($"{codeWord} {productId}");

        Assert.ThrowsException<ItemDoesNotExistException>(() => vendingMachine.GetItemFromSms());
    }


    [TestMethod]
    public void SmsOrder_CodeWordInvalid_ShouldThrowException()
    {
        var codeWord = "VENDE";
        var productId = "Coca Cola";

        using var smsService = new SmsService("@tcp://*:5557");
        using var smsServer = new MessageServer(">tcp://localhost:5557");
        var inventory = new Inventory();
        var vendingMachine = new Models.VendingMachine(inventory, null, null, null, smsService);

        smsServer.SendMessage($"{codeWord} {productId}");

        Assert.ThrowsException<SmsCodeWordException>(() => vendingMachine.GetItemFromSms());
    }

    [TestMethod]
    public void SmsOrder_ValidSms_ShouldDispense()
    {
        var codeWord = "VENDEE";
        var productId = "Coca Cola";
        var expectedDispensedSoda = Soda.CreateDefault("Coca Cola", "The Coca-Cola Company", 42);

        using var smsService = new SmsService("@tcp://*:5558");
        using var smsServer = new MessageServer(">tcp://localhost:5558");
        var inventory = new Inventory();
        var paymentService = new PaymentService();
        var dispenseService = new DispenseService(inventory, paymentService);
        var vendingMachine = new Models.VendingMachine(inventory, paymentService, dispenseService, null, smsService);
        inventory.Add(expectedDispensedSoda);

        smsServer.SendMessage($"{codeWord} {productId}");
        var dispensedItem = vendingMachine.GetItemFromSms();

        Assert.AreEqual(expectedDispensedSoda, dispensedItem, "Soda dispensed is not the same as the requested soda");
    }
}