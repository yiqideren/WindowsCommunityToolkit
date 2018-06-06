// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using static LottieData.SolidColorStroke;

namespace LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class LinearGradientStroke : ShapeLayerContent
    {
        public LinearGradientStroke(
            string name,
            string matchName,
            Animatable<double> opacityPercent,
            Animatable<double> strokeWidth,
            LineCapType capType,
            LineJoinType joinType,
            double miterLimit)
            : base(name, matchName)
        {
            OpacityPercent = opacityPercent;
            StrokeWidth = strokeWidth;
            CapType = capType;
            JoinType = joinType;
            MiterLimit = miterLimit;
        }

        public Animatable<double> OpacityPercent { get; }

        public Animatable<double> StrokeWidth { get; }

        public LineCapType CapType { get; }

        public LineJoinType JoinType { get; }

        public double MiterLimit { get; }

        public override ShapeContentType ContentType => ShapeContentType.LinearGradientStroke;
        public override LottieObjectType ObjectType => LottieObjectType.LinearGradientStroke;
    }
}
