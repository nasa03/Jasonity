// Interface for plans
// Allows the user to modify and check an agent's plans
// Previously IntendedMeans, was renamed
using System;
using Assets.Code.AsSyntax;
using Assets.Code.Logic;
using Assets.Code.ReasoningCycle;

namespace BDIManager.Intentions {
    public class IntendedPlan
    {
        protected IPlanBody planBody;
        protected Plan plan;
        private Trigger trigger;
        private Unifier unif = null;

        private Unifier renamedVars = null;

        public IntendedPlan(Option opt, Trigger te)
        {
            plan = opt.GetPlan();
            planBody = plan.GetBody();
        }

        public ITerm RemoveCurrentStep()
        {
            if (IsFinished())
            {
                return null;
            }
            else
            {
                ITerm r = planBody.GetBodyTerm();
                planBody = planBody.GetBodyNext();
                return r;
            }
        }

        public IPlanBody GetCurrentStep()
        {
            return planBody;
        }

        public IPlanBody InsertAsNextStep(IPlanBody pb)
        {
            planBody = new PlanBodyImpl(planBody.GetBodyType(), planBody.GetBodyTerm());
            planBody.SetBodyNext(pb);
            return planBody;
        }

        public Plan GetPlan()
        {
            return plan;
        }

        public Trigger GetTrigger()
        {
            return trigger;
        }

        public void SetTrigger(Trigger tr)
        {
            trigger = tr;
        }

        public bool IsAtomic()
        {
            return plan != null && plan.IsAtomic();
        }

        public bool IsFinished()
        {
            return planBody == null || planBody.IsEmptyBody();
        }

        public bool IsGoalAdd()
        {
            return trigger.IsAddition() && trigger.IsGoal();
        }

        public Unifier GetUnif()
        {
            return unif;
        }

        public Unifier GetRenamedVars()
        {
            return renamedVars;
        }

        public void SetUnif(Unifier current)
        {
            this.unif = current;
        }

        public void SetRenamedVars(Unifier renamedVars)
        {
            this.renamedVars = renamedVars;
        }

        public void Pop()
        {
            throw new NotImplementedException();
        }

        internal IntendedPlan Peek()
        {
            throw new NotImplementedException();
        }
    }
}