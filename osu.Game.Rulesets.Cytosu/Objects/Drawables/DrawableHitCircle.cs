// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cytosu.Objects.Drawables.Piece;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Cytosu.Objects.Drawables
{
    public partial class DrawableHitCircle : DrawableCytosuHitObject
    {
        public HitReceptor HitArea { get; private set; } = null!;
        private Drawable ringPiece = null!;
        private Drawable bodyPiece = null!;

        public override double LifetimeStart
        {
            get => base.LifetimeStart;
            set
            {
                base.LifetimeStart = value;
                bodyPiece.LifetimeStart = value;
            }
        }

        public override double LifetimeEnd
        {
            get => base.LifetimeEnd;
            set
            {
                base.LifetimeEnd = value;
                bodyPiece.LifetimeEnd = value;
            }
        }

        private readonly IBindable<Vector2> positionBindable = new Bindable<Vector2>();

        public DrawableHitCircle(CytosuHitObject hitObject)
            : base(hitObject) { }

        [BackgroundDependencyLoader]
        private void load()
        {
            Origin = Anchor.Centre;
            Position = HitObject.Position;

            AddRangeInternal([
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.75f),
                    Children =
                    [
                        HitArea = new HitReceptor
                        {
                            Hit = () =>
                            {
                                if (AllJudged)
                                    return false;

                                UpdateResult(true);
                                return true;
                            },
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Margin = new MarginPadding(RingPiece.RING_THICKNESS),
                            Child = bodyPiece = new BodyPiece
                            {
                                Alpha = 0,
                                Scale = Vector2.Zero,
                            },
                        },
                        ringPiece = new RingPiece(),
                    ],
                },
            ]);

            Size = HitArea.DrawSize;

            positionBindable.BindValueChanged(_ => Position = HitObject.Position);
            positionBindable.BindTo(HitObject.PositionBindable);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            Debug.Assert(HitObject.HitWindows != null);

            if (!userTriggered)
            {
                if (ShouldPerfectlyJudged && timeOffset > 0)
                    ApplyMaxResult();

                if (!HitObject.HitWindows.CanBeHit(timeOffset))
                    ApplyMinResult();

                return;
            }

            var result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None || result == HitResult.Miss && Time.Current < HitObject.StartTime)
                return;

            ApplyResult(result);
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();

            using (BeginAbsoluteSequence(HitObject.StartTime - HitObject.TimePreempt))
            {
                this.ScaleTo(0.5f).Then().ScaleTo(1, HitObject.TimePreempt, Easing.OutSine);
                ringPiece.FadeInFromZero(HitObject.TimePreempt / 2);

                bodyPiece.FadeIn(Math.Min(HitObject.TimeFadeIn * 2, HitObject.TimePreempt));
                bodyPiece.ScaleTo(1f, HitObject.TimePreempt);
            }
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            base.UpdateHitStateTransforms(state);

            Debug.Assert(HitObject.HitWindows != null);

            switch (state)
            {
                case ArmedState.Idle:
                    this.Delay(HitObject.TimePreempt).FadeOut(500);

                    Expire(true);

                    HitArea.HitAction = null;
                    break;

                case ArmedState.Miss:
                    this.FadeOut(100);
                    break;

                case ArmedState.Hit:
                    ringPiece
                        .ScaleTo(1.5f, 200, Easing.InCubic)
                        .FadeOut(200);
                    bodyPiece.FadeOut(200);

                    this.Delay(800).FadeOut();
                    break;
            }
        }

        public partial class HitReceptor : CompositeDrawable, IKeyBindingHandler<CytosuAction>
        {
            public override bool HandlePositionalInput => true;

            public Func<bool>? Hit;

            public CytosuAction? HitAction;

            public HitReceptor()
            {
                Size = new Vector2(CytosuHitObject.CIRCLE_RADIUS * 2);

                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                CornerRadius = CytosuHitObject.CIRCLE_RADIUS;
                CornerExponent = 2;
            }

            public bool OnPressed(KeyBindingPressEvent<CytosuAction> e)
            {
                switch (e.Action)
                {
                    case CytosuAction.Action1:
                    case CytosuAction.Action2:
                        if (IsHovered && (Hit?.Invoke() ?? false))
                        {
                            HitAction = e.Action;
                            return true;
                        }

                        break;
                }

                return false;
            }

            public void OnReleased(KeyBindingReleaseEvent<CytosuAction> e)
            {
            }
        }
    }
}