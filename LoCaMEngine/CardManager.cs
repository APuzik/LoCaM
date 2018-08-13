using LoCaMEngine.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMEngine
{
    public enum CardDefs
    {
        Slimer = 0,
        Scuttler,
        Beavrat,
        PlatedToad,
        GrimeGnasher,
        Murgling,
        RootkinSapling,
        Psyshroom,
        CorruptedBeavrat,
        CarnivorousBush,
        Snowsaur,
        Woodshroom,
        SwampTerror,
        FangedLunger,
        PouncingFlailmouth,
        WranglerFish,
        AshWalker,
        AcidGolem,
        Foulbeast,
        HedgeDemon,
        CrestedScuttler,
        Sigbovak,
        TitanCaveHog,
        ExplodingSkitterbug,
        SpineyChompleaf,
        RazorCrab,
        NutGatherer,
        InfestedToad,
        SteelplumeNestling,
        VenomousBogHopper,
        WoodlandHunter,
        Sandsplat,
        Chameleskulk,
        EldritchCyclops,
        SnaileyedHulker,
        PossessedSkull,
        EldritchMulticlops,
        Imp,
        VoraciousImp,
        RockGobbler,
        BlizzardDemon,
        FlyingLeech,
        ScreechingNightmare,
        Deathstalker,
        NightHowler,
        SoulDevourer,
        Gnipper,
        VenomHedgehog,
        ShinyProwler,
        PuffBiter,
        EliteBilespitter,
        Bilespitter,
        PossessedAbomination,
        ShadowBiter,
        HermitSlime,
        GiantLouse,
        DreamEater,
        DarkscalePredator,
        SeaGhost,
        GritsuckTroll,
        AlphaTroll,
        MutantTroll,
        RootkinDrone,
        CoppershellTortoise,
        SteelplumeDefender,
        StaringWickerbeast,
        FlailingHammerhead,
        GiantSquid,
        ChargingBoarhound,
        Murglord,
        FlyingMurgling,
        ShufflingNightmare,
        BogBounder,
        Crusher,
        TitanProwler,
        CrestedChomper,
        LumberingGiant,
        Shambler,
        ScarletColossus,
        CorpseGuzzler,
        FlyingCorpseGuzzler,
        SlitheringNightmare,
        RestlessOwl,
        FighterTick,
        HeartlessCrow,
        CrazedNosePincher,
        BloatDemon,
        AbyssNightmare,
        Boombeak,
        EldritchSwooper,
        Flumpy,
        Wurm,
        Spinekid,
        RootkinDefender,
        Wildum,
        PrairieProtector,
        Turta,
        LillyHopper,
        CaveCrab,
        Stalagopod,
        Engulfer,
        MoleDemon,
        MutatingRootkin,
        DeepwaterShellcrab,
        KingShellcrab,
        FarReachingNightmare,
        WorkerShellcrab,
        RootkinElder,
        ElderEngulfer,
        Gargoyle,
        TurtaKnight,
        RootkinLeader,
        TamedBilespitter,
        Gargantua,
        RootkinWarchief,
        EmperorNightmare,
        Protein,
        RoyalHelm,
        SerratedShield,
        Venomfruit,
        EnchantedHat,
        BolsteringBread,
        Wristguards,
        BloodGrapes,
        HealthyVeggies,
        HeavyShield,
        ImperialHelm,
        EnchantedCloth,
        EnchantedLeather,
        HelmofRemedy,
        HeavyGauntlet,
        HighProtein,
        PieofPower,
        LightTheWay,
        ImperialArmour,
        Buckler,
        Ward,
        GrowHorns,
        GrowStingers,
        GrowWings,
        ThrowingKnife,
        StaffofSuppression,
        PierceArmour,
        RuneAxe,
        CursedSword,
        CursedScimitar,
        QuickShot,
        HelmCrusher,
        RootkinRitual,
        ThrowingAxe,
        Decimate,
        MightyThrowingAxe,
        HealingPotion,
        Poison,
        ScrollofFirebolt,
        MajorLifeStealPotion,
        LifeSapDrop,
        TomeofThunder,
        VialofSoulDrain,
        MinorLifeStealPotion
}

    public class CardManager
    {
        int currentId = 0;
        public List<Card> AllPossibleCards = new List<Card>();
        public List<int> lastDraftIds;
        Random rnd = new Random();

        public CardManager()
        {
            LoadCards("Cards.txt");
        }

        private void LoadCards(string path)
        {
            string[] values = File.ReadAllLines(path);
            foreach (string val in values)
            {
                string[] props = val.Split(new char[] { ';' });
                Card card = new Card
                {
                    Abils = props[6].Trim(),
                    Attack = int.Parse(props[4]),
                    Cost = int.Parse(props[3]),
                    Defense = int.Parse(props[5]),
                    Draw = int.Parse(props[9]),
                    Id = -1,
                    Location = -2,
                    MyHealthChange = int.Parse(props[7]),
                    OppHealthChange = int.Parse(props[8]),
                    Number = int.Parse(props[0]),
                    Type = GetType(props[2].Trim()),
                };
                AllPossibleCards.Add(card);
            }
        }

        private int GetType(string type)
        {
            if (type == "creature")
                return 0;
            else if (type == "itemGreen")
                return 1;
            else if (type == "itemRed")
                return 2;
            else if (type == "itemBlue")
                return 3;
            else
                throw new Exception($"Invalid type: {type}");
        }

        public List<Card> GetDraft()
        {
            const int DRAFT_SIZE = 3;            
            List<int> result = new List<int>();
            for (int i = 0; i < DRAFT_SIZE; i++)
            {
                int next = rnd.Next(0, AllPossibleCards.Count - 1);
                while (result.Exists(c => c == next))
                {
                    next = rnd.Next(0, AllPossibleCards.Count - 1);
                }

                result.Add(next);
            }
            lastDraftIds = result;
            return new List<Card> { AllPossibleCards[result[0]], AllPossibleCards[result[1]], AllPossibleCards[result[2]] };
        }

        public Card CreateCardFromDraft(int draftPos)
        {
            int pos = lastDraftIds[draftPos];
            return CreateCardByNumber(pos);
        }

        public Card CreateCardByNumber(int pos)
        {
            return new Card
            {
                Abils = AllPossibleCards[pos].Abils,
                Attack = AllPossibleCards[pos].Attack,
                Cost = AllPossibleCards[pos].Cost,
                Defense = AllPossibleCards[pos].Defense,
                Draw = AllPossibleCards[pos].Draw,
                Id = currentId++,
                Location = -2,//AllPossibleCards[pos].Location,
                MyHealthChange = AllPossibleCards[pos].MyHealthChange,
                Number = AllPossibleCards[pos].Number,
                OppHealthChange = AllPossibleCards[pos].OppHealthChange,
                Type = AllPossibleCards[pos].Type
            };
        }
    }
}
