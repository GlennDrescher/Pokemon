using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon
{
    /// <summary>
    /// The possible elemental types
    /// </summary>
    public enum Elements
    {
        Fire,
        Water,
        Grass
    }

    public class Pokemon
    {
        //fields
        int level;
        int baseAttack;
        int baseDefence;
        int hp;
        int maxHp;
        Elements element;

        //properties, imagine them as private fields with a possible get/set property (accessors)
        //in this case used to allow other objects to read (get) but not write (no set) these variables
        public string Name { get; }
        //example of how to make the string Name readable AND writable  
        //  public string Name { get; set; }
        public List<Move> Moves { get; }
        //can also be used to get/set other private fields
        public int Hp { get => hp; }

        /// <summary>
        /// Constructor for a Pokemon, the arguments are fairly self-explanatory
        /// </summary>
        /// <param name="name"></param>
        /// <param name="level"></param>
        /// <param name="baseAttack"></param>
        /// <param name="baseDefence"></param>
        /// <param name="hp"></param>
        /// <param name="element"></param>
        /// <param name="moves">This needs to be a List of Move objects</param>
        public Pokemon(string name, int level, int baseAttack,
            int baseDefence, int hp, Elements element,
            List<Move> moves)
        {
            this.level = level;
            this.baseAttack = baseAttack;
            this.baseDefence = baseDefence;
            this.Name = name;
            this.hp = hp;
            this.maxHp = hp;
            this.element = element;
            this.Moves = moves;
        }

        /// <summary>
        /// performs an attack and returns total damage, check the slides for how to calculate the damage
        /// IMPORTANT: should also apply the damage to the enemy pokemon
        /// </summary>
        /// <param name="enemy">This is the enemy pokemon that we are attacking</param>
        /// <returns>The amount of damage that was applied so we can print it for the user</returns>
        public int Attack(Pokemon enemy)
        {
            // (basic_attack * level) * elemetalEffect - (defender_basic_defence * defender_level)

            int totalAttack = baseAttack * level;
            int elementalEffect = CalculateElementalEffects(totalAttack, enemy.element);
            int totalDefense = CalculateDefence();


            // Divided by 10 so not every attack is a one shot kill.
            int calculatedDamage = (elementalEffect - totalDefense) / 10;

            if (calculatedDamage < 0)
            {
                calculatedDamage = 0;
            }


            enemy.ApplyDamage(calculatedDamage);
            return calculatedDamage;
        }

        /// <summary>
        /// calculate the current amount of defence points
        /// </summary>
        /// <returns> returns the amount of defence points considering the level as well</returns>
        public int CalculateDefence()
        {
            return baseDefence * level;
        }

        /// <summary>
        /// Calculates elemental effect, check table at https://bulbapedia.bulbagarden.net/wiki/Type#Type_chart for a reference
        /// </summary>
        /// <param name="damage">The amount of pre elemental-effect damage</param>
        /// <param name="enemyType">The elemental type of the enemy</param>
        /// <returns>The damage post elemental-effect</returns>
        public int CalculateElementalEffects(int damage, Elements enemyType)
        {
            if (enemyType ==  element)
            {
                return damage;
            }
            if (element == Elements.Water &&  enemyType == Elements.Fire  )
            {
                return damage * 2;
            }
            if (element == Elements.Water && enemyType == Elements.Grass)
            {
                return damage / 2;
            }
            if (element == Elements.Fire && enemyType == Elements.Grass)
            {
                return damage * 2;
            }
            if (element == Elements.Fire && enemyType == Elements.Water)
            {
                return damage / 2;
            }
            if (element == Elements.Grass && enemyType == Elements.Water)
            {
                return damage * 2;
            }
            if (element == Elements.Grass && enemyType == Elements.Fire)
            {
                return damage / 2;
            }
            else
            {
                // Could Throw en exception here or handle the error better with a console.WriteLine
                return damage;
            }
        }

        /// <summary>
        /// Applies damage to the pokemon
        /// </summary>
        /// <param name="damage"></param>
        public void ApplyDamage(int damage)
        {


            if (damage < 0)
            {
                damage = 1;
            }

            hp = hp - damage;
            if (hp < 0)
            {
                hp = 0;
            }

            // if the enemies Defense is higher than the players attack and 
            // elemental vulnerable then the damage is negative.
            // Here I make sure that the pokemon does not absorb more health than it has max hp
            if (hp > maxHp)
            {
                hp = maxHp;
            }
        }

        /// <summary>
        /// Heals the pokemon by resetting the HP to the max
        /// </summary>
        public void Restore()
        {
            hp = maxHp;
            
        }
    }
}
