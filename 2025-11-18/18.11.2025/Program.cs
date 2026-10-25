using System;
using System.Collections.Generic;
using System.Linq;

//-------------------------------------------------------------------
public interface ISpell
{
    string Name { get; }
    int Power { get; }
    void Cast();
}

public interface IDarkMagic
{
    void DarkEffect();
}

public class Fireball : ISpell, IDarkMagic, IComparable<Fireball>
{
    public string Name { get; set; } = "Fireball";
    public int Power { get; set; } = 50;

    public void Cast() => Console.WriteLine($"{Name} casts a fiery explosion dealing {Power} damage!");
    public void DarkEffect() => Console.WriteLine($"{Name} unleashes dark fire!");
    public int CompareTo(Fireball? other) => other == null ? 1 : Power.CompareTo(other.Power);
}

public class HealingWave : ISpell, IComparable<HealingWave>
{
    public string Name { get; set; } = "HealingWave";
    public int Power { get; set; } = 30;

    public void Cast() => Console.WriteLine($"{Name} restores {Power} health!");
    public int CompareTo(HealingWave? other) => other == null ? 1 : Power.CompareTo(other.Power);
}

public class LightningStrike : ISpell, IDarkMagic, IComparable<LightningStrike>
{
    public string Name { get; set; } = "LightningStrike";
    public int Power { get; set; } = 40;

    public void Cast() => Console.WriteLine($"{Name} strikes lightning dealing {Power} damage!");
    public void DarkEffect() => Console.WriteLine($"{Name} crackles with dark energy!");
    public int CompareTo(LightningStrike? other) => other == null ? 1 : Power.CompareTo(other.Power);
}

public class Spellbook<T> where T : class, ISpell, IComparable<T>, new()
{
    private List<T> _spells = new List<T>();

    public IReadOnlyList<T> Spells => _spells.AsReadOnly();


    public void LearnSpell0(T spell)
    {
        if (!_spells.Any(s => s.CompareTo(spell) == 0))
        {
            _spells.Add(spell);
            _spells.Sort((a, b) => b.CompareTo(a));
        }
        else
        {
            Console.WriteLine($"Spell {spell.Name} already learned!");
        }
    }

    public void CastStrongest()
    {
        if (!_spells.Any())
        {
            Console.WriteLine("No spells learned!");
            return;
        }

        var strongest = _spells[0];
        Console.WriteLine($"\nCasting strongest spell: {strongest.Name} ({strongest.Power} power)");
        strongest.Cast();
    }

    public void InvokeRitual0()
    {
        if (_spells.All(s => s is IDarkMagic))
        {
            Console.WriteLine("\nInvoking dark ritual with all spells:");
            foreach (IDarkMagic spell in _spells)
                spell.DarkEffect();
        }
        else
        {
            throw new InvalidOperationException("All spells must be dark magic to invoke ritual!");
        }
    }
}


class Program
{
    static void Main()
    {
        var firebook = new Spellbook<Fireball>();
        firebook.LearnSpell0(new Fireball() { Power = 50 });
        firebook.LearnSpell0(new Fireball() { Power = 60 });
        firebook.LearnSpell0(new Fireball() { Power = 50 });

        var healingBook = new Spellbook<HealingWave>();
        healingBook.LearnSpell0(new HealingWave() { Power = 25 });
        healingBook.LearnSpell0(new HealingWave() { Power = 35 });

        firebook.CastStrongest();
        healingBook.CastStrongest();

        var darkBook = new Spellbook<LightningStrike>();
        darkBook.LearnSpell0(new LightningStrike() { Power = 40 });
        darkBook.LearnSpell0(new LightningStrike() { Power = 55 });

        try
        {
            darkBook.InvokeRitual0();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        var mixedBook = new Spellbook<Fireball>();
        mixedBook.LearnSpell0(new Fireball() { Power = 45 });

        try
        {
            mixedBook.InvokeRitual0();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}