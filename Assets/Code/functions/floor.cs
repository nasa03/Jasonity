using Assets.Code.AsSyntax;
using System;

namespace Assets.Code.functions
{
    public partial class floor : DefaultArithFunction
    {

        public string GetName()
        {
            return "math.floor";
        }

        public double Evaluate(TransitionSystem ts, ITerm[] args)
        {
            if (args[0].IsNumeric())
            {
                double n = ((INumberTerm)args[0]).Solve();
                return Math.Floor(n);
            }
            else
            {
                throw new JasonException("The argument '" + args[0] + "' is not numeric!");
            }
        }

        public bool CheckArity(int a)
        {
            return a == 1;
        }
    }
}