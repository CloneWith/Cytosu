// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cytosu.Judgements;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Cytosu.Objects.Drawables
{
    public partial class DrawableCytosuJudgement : DrawableJudgement
    {
        private Vector2 screenSpacePosition;

        public override void Apply(JudgementResult result, DrawableHitObject? judgedObject)
        {
            base.Apply(result, judgedObject);

            if (judgedObject is not DrawableCytosuHitObject cytosuHitObject)
                return;

            screenSpacePosition = cytosuHitObject.ToScreenSpace(cytosuHitObject.OriginPosition);
            Scale = new Vector2(0.75f);
        }


        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(new SkinnableLighting
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Blending = BlendingParameters.Additive,
                Depth = float.MaxValue,
                Alpha = 0
            });
        }

        protected override void PrepareForUse()
        {
            base.PrepareForUse();

            Position = Parent!.ToLocalSpace(screenSpacePosition);
        }
        
        protected override Drawable CreateDefaultJudgement(HitResult result) => new CytosuJudgementPiece(result);
    }
}
