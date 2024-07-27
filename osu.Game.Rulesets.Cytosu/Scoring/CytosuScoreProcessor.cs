// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Cytosu.Scoring
{
    public partial class CytosuScoreProcessor : ScoreProcessor
    {
        public CytosuScoreProcessor() : base(new CytosuRuleset())
        {
        }
        protected double DefaultComboPortion => 0.5;
        protected double DefaultAccuracyPortion => 0.5;
    }
}
