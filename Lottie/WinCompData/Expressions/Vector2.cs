// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace WinCompData.Expressions
{
#if !WINDOWS_UWP
    public
#endif
    sealed class Vector2 : Expression
    {
        public Expression X { get; }
        public Expression Y { get; }

        internal Vector2(Expression x, Expression y)
        {
            X = x;
            Y = y;
        }


        public static Vector2 operator *(Vector2 left, double right) => new Vector2(Multiply(left.X, Scalar(right)), Multiply(left.Y, Scalar(right)));

        public override Expression Simplified => this;
        public override string ToString() => $"Vector2({Parenthesize(X)},{Parenthesize(Y)})";

        internal override bool IsAtomic => true;

        public override ExpressionType InferredType => new ExpressionType(TypeConstraint.Vector2);
    }
}
