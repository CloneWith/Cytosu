﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Cytosu.Scoring
{
    public class CytosuHitWindows : HitWindows
    {
        private static readonly DifficultyRange[] cytosu_ranges =
        {
            new DifficultyRange(HitResult.Perfect, 22.4D, 19.4D, 13.9D),
            new DifficultyRange(HitResult.Great, 80, 50, 20),
            new DifficultyRange(HitResult.Good, 140, 100, 60),
            new DifficultyRange(HitResult.Meh, 200, 150, 100),
            new DifficultyRange(HitResult.Miss, 400, 400, 400),
        };

        public override bool IsHitResultAllowed(HitResult result)
        {
            switch (result)
            {
                case HitResult.Perfect:
                case HitResult.Great:
                case HitResult.Good:
                case HitResult.Meh:
                case HitResult.Miss:
                    return true;
                default:
                    return false;
            }
        }

        protected override DifficultyRange[] GetRanges() => cytosu_ranges;
    }
}
