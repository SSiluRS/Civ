module Science 
    type Advance = {
        name: string
        requirements: Advance list
    }
    let Pottery = {
        name = "Pottery"
        requirements = []
    }
    let Alphabet = {
        name = "Alphabet" 
        requirements = []
    }
    let The_Wheel= {
        name = "The_Wheel"
        requirements = []
    }
    let Ceremonial_Burial = {
        name = "Ceremonial Burial"
        requirements = []
    }
    let Masonry = {
        name = "Masonry"
        requirements = []
    }
    let Horseback_Riding= {
        name = "Horseback Riding"
        requirements = []
    }
    let Bronze_Working = {
        name = "Bronze Working"
        requirements = []
    }
    let Iron_Working = {
        name = "Iron Working"
        requirements = [Bronze_Working]
    }
    let Map_Making = {
        name = "Map Making"
        requirements = [Alphabet]
    }
    let Mathematics = {
        name = "Mathematics"
        requirements = [Alphabet; Masonry]
    }
    let Mysticism = {
        name = "Mysticism"
        requirements = [Ceremonial_Burial]
    }
    let Astronomy = {
        name = "Astronomy"
        requirements = [Mathematics; Mysticism]
    }
    let Navigation = {
        name = "Navigation"
        requirements = [Map_Making; Astronomy]
    }
    let Physics = {
        name = "Physics"
        requirements = [Mathematics; Navigation]
    }
    let Currency = {
        name = "Currency"
        requirements = [Bronze_Working]
    }
    let Construction = {
        name = "Construction"
        requirements = [Masonry; Currency]
    }
    let Bridge_Building = {
        name = "Bridge Building"
        requirements = [Iron_Working; Construction]
    }
    let Engineering = {
        name = "Engineering"
        requirements = [The_Wheel; Construction]
    }
    let Writing = {
        name = "Writing"
        requirements = [Alphabet]
    }
    let Code_of_Laws = {
        name = "Code of Laws"
        requirements = [Alphabet]
    }
    let Trade = {
        name = "Trade"
        requirements = [Currency; Code_of_Laws]
    }
    let Monarchy = {
        name = "Monarchy"
        requirements = [Ceremonial_Burial; Code_of_Laws]
    }
    let Feudalism = {
        name = "Feudalism"
        requirements = [Masonry; Monarchy]
    }
    let Chivalry = {
        name = "Chivalry"
        requirements = [Horseback_Riding; Feudalism]
    }
    let Literacy = {
        name = "Literacy"
        requirements = [Writing; Code_of_Laws]
    }
    let Philosophy = {
        name = "Philosophy"
        requirements = [Mysticism; Literacy]
    }
    let Democracy = {
        name = "Democracy"
        requirements = [Literacy; Philosophy]
    }
    let Medicine = {
        name = "Medicine"
        requirements = [Philosophy; Trade]
    }
    let Religion = {
        name = "Religion"
        requirements = [Philosophy; Writing]
    }
    let University = {
        name = "University"
        requirements = [Mathematics; Philosophy]
    }
    let Chemistry = {
        name = "Chemistry"
        requirements = [Medicine; University]
    }
    let Theory_of_Gravity = {
        name = "Theory of Gravity"
        requirements = [Astronomy; University]
    }
    let Atomic_Theory = {
        name = "Atomic Theory"
        requirements = [Physics; Theory_of_Gravity]
    }
    let Republic = {
        name = "Republic"
        requirements = [Code_of_Laws; Literacy]
    }
    let Banking = {
        name = "Banking"
        requirements = [Trade; Republic]
    }
    let Invention = {
        name = "Invention"
        requirements = [Engineering; Literacy]
    }
    let Steam_Engine = {
        name = "Steam Engine"
        requirements = [Physics; Invention]
    }
    let Railroad = {
        name = "Railroad"
        requirements = [Steam_Engine; Bridge_Building]
    }
    let Industrialization = {
        name = "Industrialization"
        requirements = [Banking; Railroad]
    }
    let Communism = {
        name = "Communism"
        requirements = [Philosophy; Industrialization]
    }
    let Corporation = {
        name = "Corporation"
        requirements = [Banking; Industrialization]
    }
    let Genetic_Engineering = {
        name = "Genetic Engineering"
        requirements = [Medicine; Corporation]
    }
    let Refining = {
        name = "Refining"
        requirements = [Chemistry; Corporation]
    }
    let Gunpowder = {
        name = "Gunpowder"
        requirements = [Invention; Iron_Working]
    }
    let Explosives = {
        name = "Explosives"
        requirements = [Gunpowder; Chemistry]
    }
    let Combustion = {
        name = "Combustion"
        requirements = [Refining; Explosives]
    }
    let Flight = {
        name = "Flight"
        requirements = [Combustion; Physics]
    }
    let Conscription = {
        name = "Conscription"
        requirements = [Republic; Explosives]
    }
    let Metallurgy = {
        name = "Metallurgy"
        requirements = [Gunpowder; University]
    }
    let Steel = {
        name = "Steel"
        requirements = [Metallurgy; Industrialization]
    }
    let Automobile = {
        name = "Automobile"
        requirements = [Steel; Combustion]
    }
    let Mass_Production = {
        name = "Mass Production"
        requirements = [Automobile; Corporation]
    }
    let Recycling = {
        name = "Recycling"
        requirements = [Mass_Production; Democracy]
    }
    let Nuclear_Fission = {
        name = "Nuclear Fission"
        requirements = [Mass_Production; Atomic_Theory]
    }

    let Labor_Union = {
        name = "Labor Union"
        requirements = [Communism; Mass_Production]
    }
    let Magnetism = {
        name = "Magnetism"
        requirements = [Navigation; Physics]
    }
    let Electricity = {
        name = "Electricity"
        requirements = [Magnetism; Metallurgy]
    }
    let Electronics = {
        name = "Electronics"
        requirements = [Electricity; Engineering]
    }
    let Nuclear_Power = {
        name = "Nuclear Power"
        requirements = [Electronics; Nuclear_Fission]
    }
    let Computers = {
        name = "Computers"
        requirements = [Mathematics; Electronics]
    }
    let Advanced_Flight = { 
        name = "Advanced Flight"
        requirements = [Alphabet; Electricity]
    }
    let Rocketry = {
        name = "Rocketry"
        requirements = [Electronics; Advanced_Flight]
    }
    let Space_Flight = {
        name = "Space Flight"
        requirements = [Computers; Rocketry]
    }
    let Plastics = {
        name = "Plastics"
        requirements = [Refining; Space_Flight]
    }
    let Superconductor = {
        name = "Superconductor"
        requirements = [Plastics; Mass_Production]
    }
    let Fusion_Power = {
        name = "Fusion Power"
        requirements = [Nuclear_Power; Superconductor]
    }
    let Future_Tech = {
        name = "Future Tech"
        requirements = [Fusion_Power]
    }
    let Robotics = {
        name = "Robotics"
        requirements = [Plastics; Computers]
    }
    let allAdvances = [
        Pottery; Alphabet; The_Wheel; Ceremonial_Burial; Masonry; Horseback_Riding; Bronze_Working; 
        Iron_Working; Map_Making; Mathematics; Mysticism; Astronomy; Navigation; Physics; Currency; 
        Construction; Bridge_Building; Engineering; Writing; Code_of_Laws; Trade; Monarchy; Feudalism;
        Chivalry; Literacy; Philosophy; Democracy; Medicine; Religion; University; Chemistry; 
        Theory_of_Gravity; Atomic_Theory; Republic; Banking; Invention; Steam_Engine; Railroad; 
        Industrialization; Communism; Corporation; Genetic_Engineering; Refining; Gunpowder; 
        Explosives; Combustion; Flight; Conscription; Metallurgy; Steel; Automobile; Mass_Production; 
        Recycling; Nuclear_Fission; Labor_Union; Magnetism; Electricity; Electronics; Nuclear_Power; 
        Computers; Advanced_Flight; Rocketry; Space_Flight; Plastics; Superconductor; Fusion_Power; 
        Future_Tech; Robotics
    ]