using System;
using System.Collections.Generic;
using Frontend2;
using Frontend2.Hardware;

public class VendingMachineFactory : IVendingMachineFactory {
    List<VendingMachine> vendingMachines;
    public VendingMachineFactory()
    {
        this.vendingMachines = new List<VendingMachine>();
    }

    internal seng301_asgn2.src.VendingMachine1 VendingMachine
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }

    public int CreateVendingMachine(List<int> coinKinds, int selectionButtonCount, int coinRackCapacity, int popRackCapcity, int receptacleCapacity) {
        // TODO: Implement
        var index = this.vendingMachines.Count;
        this.vendingMachines.Add(new VendingMachine(coinKinds, selectionButtonCount,coinRackCapacity,popRackCapcity,receptacleCapacity));
        return index;
    }

    public void ConfigureVendingMachine(int vmIndex, List<string> popNames, List<int> popCosts) {
        // TODO: Implement
        this.vendingMachines[vmIndex].ConfigureVendingMachine(popNames, popCosts);
    }

    public void LoadCoins(int vmIndex, int coinKindIndex, List<Coin> coins) {
        // TODO: Implement
        this.vendingMachines[vmIndex].LoadCoins(coinKindIndex, coins);
    }

    public void LoadPops(int vmIndex, int popKindIndex, List<PopCan> pops) {
        // TODO: Implement
        this.vendingMachines[vmIndex].LoadPops(popKindIndex, pops);
    }

    public void InsertCoin(int vmIndex, Coin coin) {
        // TODO: Implement

    }

    public void PressButton(int vmIndex, int value) {
        // TODO: Implement
        this.vendingMachines[vmIndex].PressButton(value);
    }

    public List<IDeliverable> ExtractFromDeliveryChute(int vmIndex) {
        // TODO: Implement
        return new List<IDeliverable>();
    }

    public VendingMachineStoredContents UnloadVendingMachine(int vmIndex) {
        // TODO: Implement
        return this.vendingMachines[vmIndex].UnloadVendingMachine();
    }
}