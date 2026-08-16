// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Cytosu.Scoring
{
    public class CytosuHitWindows : HitWindows
    {
        private static readonly DifficultyRange perfect_window_range = new DifficultyRange(22.4D, 19.4D, 13.9D);
        private static readonly DifficultyRange great_window_range = new DifficultyRange(80, 50, 20);
        private static readonly DifficultyRange good_window_range = new DifficultyRange(140, 100, 60);
        private static readonly DifficultyRange meh_window_range = new DifficultyRange(200, 150, 100);

        private const double miss_window = 400;

        private double perfect;
        private double great;
        private double good;
        private double meh;

        public override bool IsHitResultAllowed(HitResult result) => result switch
        {
            HitResult.Perfect or HitResult.Great or HitResult.Good or HitResult.Meh or HitResult.Miss => true,
            _ => false,
        };

        public override void SetDifficulty(double difficulty)
        {
            perfect = Math.Floor(IBeatmapDifficultyInfo.DifficultyRange(difficulty, perfect_window_range)) - 0.5;
            great = Math.Floor(IBeatmapDifficultyInfo.DifficultyRange(difficulty, great_window_range)) - 0.5;
            good = Math.Floor(IBeatmapDifficultyInfo.DifficultyRange(difficulty, good_window_range)) - 0.5;
            meh = Math.Floor(IBeatmapDifficultyInfo.DifficultyRange(difficulty, meh_window_range)) - 0.5;
        }

        public override double WindowFor(HitResult result) => result switch
        {
            HitResult.Perfect => perfect,
            HitResult.Great => great,
            HitResult.Good => good,
            HitResult.Meh => meh,
            HitResult.Miss => miss_window,
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null),
        };
    }
}