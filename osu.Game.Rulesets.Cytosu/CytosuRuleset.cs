﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Cytosu.Beatmaps;
using osu.Game.Rulesets.Cytosu.Configurations;
using osu.Game.Rulesets.Cytosu.Mods;
using osu.Game.Rulesets.Cytosu.Replays;
using osu.Game.Rulesets.Cytosu.Scoring;
using osu.Game.Rulesets.Cytosu.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Replays.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Cytosu
{
    public class CytosuRuleset : Ruleset
    {
        public override string Description => "Cytosu";

        public override string PlayingVerb => "Scanning...";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) =>
            new DrawableCytosuRuleset(this, beatmap, mods);

        public override ScoreProcessor CreateScoreProcessor() => new CytosuScoreProcessor();

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) =>
            new CytosuBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) =>
            new CytosuDifficultyCalculator(this.RulesetInfo, beatmap);

        public override IConvertibleReplayFrame CreateConvertibleReplayFrame() => new CytosuReplayFrame();

        public override IRulesetConfigManager CreateConfig(SettingsStore settings) => new CytosuRulesetConfigManager(settings, RulesetInfo);

        public override RulesetSettingsSubsection CreateSettings() => new CytosuSettingsSubsection(this);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[] 
                    { 
                        new CytosuModNoFail(),
                        new MultiMod(new CytosuModHalfTime(), new CytosuModDaycore())
                    };

                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new MultiMod(new CytosuModDoubleTime(), new CytosuModNightcore())
                    };

                case ModType.Automation:
                    return new Mod[]
                    {
                        new CytosuModAutoplay(),
                        new CytosuModRelax()
                    };

                default:
                    return new Mod[] { null };
            }
        }

        public override string ShortName => "cytosu";

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.Z, CytosuAction.Action1),
            new KeyBinding(InputKey.X, CytosuAction.Action2),
            new KeyBinding(InputKey.MouseLeft, CytosuAction.Action1),
            new KeyBinding(InputKey.MouseRight, CytosuAction.Action2),
        };

        protected override IEnumerable<HitResult> GetValidHitResults()
        {
            return new[]
            {
                HitResult.Meh,
                HitResult.Good,
                HitResult.Great,
                HitResult.Perfect,
            };
        }

        public override Drawable CreateIcon() => new CytosuIcon();
    }
}
