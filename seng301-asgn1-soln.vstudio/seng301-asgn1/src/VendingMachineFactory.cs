using System.Collections;
using System.Collections.Generic;

using Frontend1;

public class VendingMachineFactory : IVendingMachineFactory {
    List<VendingMachine> vendingMachines;

    public VendingMachineFactory() {
        this.vendingMachines = new List<VendingMachine>();
    }

    public int createVendingMachine(List<int> coinKinds, int selectionButtonCount) {
        var index = this.vendingMachines.Count;
        this.vendingMachines.Add(new VendingMachine(coinKinds, selectionButtonCount));
        return index;
    }

    public void configureVendingMachine(int vmIndex, List<string> popNames, List<int> popCosts) {
        this.vendingMachines[vmIndex].ConfigureVendingMachine(popNames, popCosts);
    }

    public void loadCoins(int vmIndex, int coinKindIndex, List<Coin> coins) {
        this.vendingMachines[vmIndex].LoadCoins(coinKindIndex, coins);
    }

    public void loadPops(int vmIndex, int popKindIndex, List<Pop> pops) {
        this.vendingMachines[vmIndex].LoadPops(popKindIndex, pops);
    }

    public List<IList> unloadVendingMachine(int vmIndex) {
        return this.vendingMachines[vmIndex].UnloadVendingMachine();
    }

    public List<Deliverable> extractFromDeliveryChute(int vmIndex) {
        return this.vendingMachines[vmIndex].ExtractFromDeliveryChute();
    }

    public void insertCoin(int vmIndex, Coin coin) {
        this.vendingMachines[vmIndex].InsertCoin(coin);
    }

    public void pressButton(int vmIndex, int value) {
        this.vendingMachines[vmIndex].PressButton(value);
    }
}