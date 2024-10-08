﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zap_program2024.Spawning
{
    public class NextEnemyPicker
    {
        private Random rnd = new Random();
        private const int requiredProbability = 100;
        //list in following functions has to be sorted based on probabilities(in decreasing order)
        private int CalculateProbability(List<(int, int)> valueProbabilityDoubles)
        {
            var totalProbability = 0;
            foreach (var value in valueProbabilityDoubles)
            {
                totalProbability += value.Item2;
            }
            return totalProbability;
        }
        private bool ValidProbability(List<(int, int)> valueProbabilityDoubles)
        {
            var totalProbability = 0;
            totalProbability = CalculateProbability(valueProbabilityDoubles);
            if (totalProbability == requiredProbability)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int GetBasedOnProbability(List<(int, int)> valueProbabilityDoubles)
        {
            if (ValidProbability(valueProbabilityDoubles))
            {
                var pickedNumber = rnd.Next(0, requiredProbability);
                var checkpoint = 0;
                foreach (var value in valueProbabilityDoubles)
                {
                    checkpoint += value.Item2;
                    if (pickedNumber <= checkpoint)
                    {
                        return value.Item1;
                    }
                }
            }
            return -1;
        }
    }
}
