using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Frontend2;
using System.Collections;

namespace seng301_asgn2.src
{
    class VendingMachine1
    {

        Dictionary<int, int> coinKindToHopperIndex;
        Dictionary<string, int> popKindToHopperIndex;
        List<Coin>[] coinHopper;
        List<PopCan>[] popHopper;
        List<int> popCosts;
        List<Coin> coinsMade;
        List<Coin> coinsInLimbo;
        List<IDeliverable> deliveryChute;
        int coinRackCapacity;
        int popRackCapcity;
        int receptacleCapacity;

        //List<int> coinKinds, int selectionButtonCount, int coinRackCapacity, int popRackCapcity, int receptacleCapacity
        public VendingMachine1(List<int> coinKinds, int popKindCount, int coinRackCapacity, int popRackCapcity, int receptacleCapacity )
        {
            //the list of coin kinds shouldnt go anywhere i dont think????
            this.coinRackCapacity = coinRackCapacity;
            this.popRackCapcity = popRackCapcity;
            this.receptacleCapacity = receptacleCapacity;
            var hashCoinKinds = new HashSet<int>(coinKinds);
            if (hashCoinKinds.Count != coinKinds.Count)
            {
                throw new Exception("Non-unique coin kinds");
            }
            if (hashCoinKinds.Where(ck => ck <= 0).Count() > 0)
            {
                throw new Exception("Zero or negative coin kinds are not allowed");
            }

            this.coinKindToHopperIndex = new Dictionary<int, int>();
            this.coinHopper = new List<Coin>[coinKinds.Count];
            for (int i = 0; i < coinKinds.Count; i++)
            {
                this.coinHopper[i] = new List<Coin>();
                this.coinKindToHopperIndex[coinKinds[i]] = i;
            }
            this.popKindToHopperIndex = new Dictionary<string, int>();
            this.popHopper = new List<PopCan>[popKindCount];
            for (int i = 0; i < popKindCount; i++)
            {
                this.popHopper[i] = new List<PopCan>();
            }
            this.deliveryChute = new List<IDeliverable>();
            this.coinsMade = new List<Coin>();
            this.popCosts = new List<int>();
            this.coinsInLimbo = new List<Coin>();
        }
        // dont think i touch this calss

        //i think theres pop events to use here
        public void ConfigureVendingMachine(List<string> popNames, List<int> popCosts)
        {
            this.popKindToHopperIndex.Clear();
            for (int i = 0; i < popNames.Count; i++)
            {
                this.popKindToHopperIndex[popNames[i]] = i;
                //pop added event
            }
            this.popCosts.Clear();
            this.popCosts.AddRange(popCosts);
            //price event 
        }

        public void LoadCoins(int coinKindIndex, List<Coin> coins)
        {
            if (this.coinHopper[coinKindIndex].Count + 1 == coinRackCapacity)
            {
                throw new Exception("Coin rack is full! ");

            }
            else
            {
                this.coinHopper[coinKindIndex].AddRange(coins);
            }
            //add coins event
            //coin rack full

            //coins loaded?
        }

        public void LoadPops(int popKindIndex, List<PopCan> pops)
        {
            if(this.popHopper[popKindIndex].Count + 1 == popRackCapcity)
            {
                throw new Exception("Pop rack is full!");
            }
            else
            {
                this.popHopper[popKindIndex].AddRange(pops);
            }
            
            //add pops event
            //pop rack full


            //pop cans loaded?
        }

        public List<Object> UnloadVendingMachine()
        {
            var unusedCoins = new List<Coin>();
            foreach (var hopper in this.coinHopper)
            {
                unusedCoins.AddRange(hopper);
                //unload coins event 
                hopper.Clear();
            }
            var unsoldPops = new List<PopCan>();
            foreach (var hopper in this.popHopper)
            {
                unsoldPops.AddRange(hopper);
                //Remove pops until empty 
                hopper.Clear();
            }

            var returnList = new List<Object>();
            returnList.Add(unusedCoins);
            returnList.Add(new List<Coin>(this.coinsMade));
            returnList.Add(unsoldPops);
            if(returnList.Count>=receptacleCapacity)
            {
                throw new Exception("Receptacle shoot not large enough");
            }
            this.coinsMade.Clear();

            return returnList;
        }

        public List<IDeliverable> ExtractFromDeliveryChute()
        {
            var deliveryChuteContents = this.deliveryChute;
            this.deliveryChute = new List<IDeliverable>();
            if(deliveryChute.Count >= receptacleCapacity)
            {
                throw new Exception("Not big enough chute");
            }
            return deliveryChuteContents;
        }

        public void InsertCoin(Coin coin)
        {
            if (this.coinKindToHopperIndex.Keys.Contains(coin.Value))
            {
                this.coinsInLimbo.Add(coin);
                //add coins exception called
            }
            else
            {
                this.deliveryChute.Add(coin);
            }
        }

        public void PressButton(int popIndex)
        {
            var popCost = this.popCosts[popIndex];

            if (popIndex > this.popHopper.Length)
            {
                throw new Exception("Cannot press a button that doesn't exist!");
            }
            if (this.paidMoney() < popCost)
            {
                return;
            }
            if (this.popHopper[popIndex].Count == 0)
            {
                return;
            }
            var pop = this.popHopper[popIndex][0];
            this.deliveryChute.Add(pop);
            this.popHopper[popIndex].Remove(pop);
            this.deliveryChute.AddRange(this.makeChange(popCost));

            this.coinsMade.AddRange(this.coinsInLimbo);
            this.coinsInLimbo.Clear();
        }

        private int paidMoney()
        {
            return this.coinsInLimbo.Sum(c => c.Value);
        }

        private List<Coin> makeChange(int cost)
        {
            var money = this.paidMoney();
            var coinsInChute = new List<Coin>();
            var changeNeeded = money - cost;

            while (changeNeeded > 0)
            {
                var hoppersWithMoney = this.coinKindToHopperIndex.Where(h => h.Key <= changeNeeded && this.coinHopper[h.Value].Count > 0).OrderByDescending(h => h.Key);

                if (hoppersWithMoney.Count() == 0)
                {
                    //Console.WriteLine("Woohoo! Customer is screwed out of " + changeNeeded + " pennies!");
                    return coinsInChute;
                }

                var biggestHopper = hoppersWithMoney.First();

                changeNeeded = changeNeeded - biggestHopper.Key;
                var coin = this.coinHopper[biggestHopper.Value][0];
                this.coinHopper[biggestHopper.Value].Remove(coin);
                coinsInChute.Add(coin);
            }

            return coinsInChute;
        }

    }

}

