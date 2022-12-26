using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act_Eat : Activity
{
    // Activity Base
    public override ActivityId Id => ActivityId.Eat;
    public override string DisplayString => _DisplayString;

    // Individual
    private bool TargetReached;
    private TileObjectBase FoodTarget;
    private string _DisplayString;

    public Act_Eat(AnimalBase animal) : base(animal) { }

    protected override void OnActivityStart()
    {
        TileObjectBase closestFood = FindClosestFood();
        if (closestFood != null)
        {
            FoodTarget = closestFood;
            if (FoodTarget.Tile == Animal.Tile) StartEating();
            else
            {
                _DisplayString = "Moving to eat " + FoodTarget.Name;
                TargetReached = false;
                Animal.MoveTo(FoodTarget.Tile);
            }
        }
        else End(); // don't see any food
    }

    public override void OnTick()
    {
        if (FoodTarget == null || Animal.Nutrition.Ratio > 0.95f)
        {
            End();
            return;
        }

        if (!Animal.IsMoving)
        {
            if (!TargetReached) StartEating();
            else Eat();
        }
    }

    /// <summary>
    /// Starts eating the target food.
    /// </summary>
    private void StartEating()
    {
        _DisplayString = "Eating " + FoodTarget.Name;
        TargetReached = true;
    }

    /// <summary>
    /// Finds the closest object that is edible for this animal.
    /// </summary>
    private TileObjectBase FindClosestFood()
    {
        int range = 0;
        List<WorldTile> currentRangeTiles = new List<WorldTile>() { Animal.Tile };
        while (range < (int)Animal.VisionRange)
        {
            foreach (WorldTile tile in currentRangeTiles)
            {
                foreach (TileObjectBase obj in tile.TileObjects)
                    if (obj.GetNutrientsForAnimal(Animal) > 0) return obj;
            }
            range++;
            currentRangeTiles = Pathfinder.GetAllReachablePositionsWithRange(Animal, Animal.Tile, range);
            if (currentRangeTiles == null) return null; // No tiles reachable with that range (we are in a closed space)
        }
        return null;
    }

    /// <summary>
    /// Removes some health of the food target and adds some nutrition to the animal eating.
    /// </summary>
    private void Eat()
    {
        // Calculate how much % of the object gets consumed this frame
        float chunkEaten = FoodTarget.GetEatingSpeed(Animal) * Simulation.Singleton.TickTime;

        // Reduces objects health by that amount
        float lostHealth = FoodTarget.Health.MaxValue * chunkEaten;
        FoodTarget.Health.ChangeValue(-lostHealth);

        // Increase animals nutrition by that amount
        float gainedNutrition = FoodTarget.NutrientValue * chunkEaten;
        Animal.Nutrition.ChangeValue(gainedNutrition);
    }

    public override float GetUrgency()
    {
        return (1f - Animal.Nutrition.Ratio) * 2f;
    }
}
